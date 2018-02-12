using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace pure.zlo_3.source
{
    static class Renderer
    {
        public static int texId;
        private static readonly float[,] matrix = new float[4, 4];
        private static readonly float[,] matrixR = new float[4, 4];
        private static readonly float[,] matrixS = new float[4, 4];
        private static readonly List<Vector3> scales = new List<Vector3>();
        private static readonly List<Vector3> normals = new List<Vector3>();
        private static readonly List<float> percents = new List<float>();

        private static readonly List<Vector3> smoothednormals = new List<Vector3>();
        private static readonly List<Vector3> replicationTray = new List<Vector3>();
        private static readonly List<Vector3> templateHexagon = new List<Vector3>(4);
        private static readonly List<List<Vector3>> figure = new List<List<Vector3>>();

        public static void axis()
        {
            GL.LineWidth(2f);
            GL.Begin(PrimitiveType.Lines);

            GL.Color3(1f, 0f, 0f);
            GL.Vertex3(0f, 0f, 0f);
            GL.Vertex3(1f, 0f, 0f);

            GL.Color3(0f, 1f, 0f);
            GL.Vertex3(0f, 0f, 0f);
            GL.Vertex3(0f, 1f, 0f);

            GL.Color3(0f, 0f, 1f);
            GL.Vertex3(0f, 0f, 0f);
            GL.Vertex3(0f, 0f, 1f);

            GL.End();
            GL.LineWidth(1f);
        }

        public static void grid(float iterations = 10, float space = 5)
        {
            var gridBound = iterations * space;

            GL.Color3(1f, 1f, 1f);
            GL.Begin(PrimitiveType.Lines);

            for (var x = -iterations;x<=iterations; x++)
            {
                GL.Vertex3(x * space, 0, -gridBound);
                GL.Vertex3(x * space, 0, gridBound);

                for (var z = -iterations; z <= iterations; z++)
                {
                    GL.Vertex3(-gridBound, 0, z * space);
                    GL.Vertex3(gridBound, 0, z * space);
                }
            }

            GL.End();
        }

        public static void lightOn(int lightSample)
        {
            GL.Enable(EnableCap.Lighting);
            GL.LightModel(LightModelParameter.LightModelLocalViewer, 1);
            GL.Enable(EnableCap.Light0);
            GL.Light(LightName.Light0, LightParameter.Position, new[] { 10f, 5f, 10f, 1f });
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Diffuse, new[] { 0.5f, 0.5f, 0.5f });
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Diffuse, new[] { 0.5f, 0.5f, 0.5f });
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Shininess, 50f);
            GL.Enable(EnableCap.ColorMaterial);
            GL.PointSize(10);
            GL.Color3(255f, 0, 0);
            GL.Begin(PrimitiveType.Points);
            GL.Vertex3(10f, 5f, 10f);
            GL.End();
        }

        public static void lightOff()
        {
            GL.Disable(EnableCap.Lighting);
        }

        private static Vector3 cross(Vector3 vectorA, Vector3 vectorB)
        {
            var normal = new Vector3
            {
                X = vectorA.Y * vectorB.Z - vectorA.Z * vectorB.Y,
                Y = vectorA.Z * vectorB.X - vectorA.X * vectorB.Z,
                Z = vectorA.X * vectorB.Y - vectorA.Y * vectorB.X
            };

            return normal.Normalized();
        }

        private static Vector3 normal(Vector3 point1, Vector3 point2, Vector3 point3)
        {
            var vector1 = point3 - point2;
            var vector2 = point2 - point1;
            var normal = cross(vector1, vector2);

            return normal.Normalized();
        }

        private static float scalar(Vector3 vectorA, Vector3 vectorB)
        {
            return vectorA.X * vectorB.X + vectorA.Y * vectorB.Y + vectorA.Z * vectorB.Z;
        }

        private static void makeMatrixRotate(float angle, Vector3 axis)
        {
            var cos = (float)Math.Cos(angle * Math.PI / 180.0);
            var sin = (float)Math.Sin(angle * Math.PI / 180.0);
            var os = axis.Normalized();

            matrixR[0, 0] = cos + (1 - cos) * os.X * os.X;
            matrixR[0, 1] = (1 - cos) * os.X * os.Y - sin * os.Z;
            matrixR[0, 2] = (1 - cos) * os.X * os.Z + sin * os.Y;
            matrixR[0, 3] = 0;
            matrixR[1, 0] = (1 - cos) * os.X * os.Y + sin * os.Z;
            matrixR[1, 1] = cos + (1 - cos) * os.Y * os.Y;
            matrixR[1, 2] = (1 - cos) * os.Z * os.Y - sin * os.X;
            matrixR[1, 3] = 0;
            matrixR[2, 0] = (1 - cos) * os.X * os.Z - sin * os.Y;
            matrixR[2, 1] = (1 - cos) * os.Z * os.Y + sin * os.X;
            matrixR[2, 2] = cos + (1 - cos) * os.Z * os.Z;
            matrixR[2, 3] = 0;
            matrixR[3, 0] = 0;
            matrixR[3, 1] = 0;
            matrixR[3, 2] = 0;
            matrixR[3, 3] = 1;
        }

        private static Vector3 multCoord(float[,] matrix, Vector3 vectorStart)
        {
            var vector = new[] { vectorStart.X, vectorStart.Y, vectorStart.Z, 1.0f };
            var resultVector = new float[4];

            for (var i = 0; i < 4; i++)
            {
                var s = 0.0f;
                for (var j = 0; j < 4; j++)
                    s += matrix[i, j] * vector[j];
                resultVector[i] = s;
            }

            return new Vector3(resultVector[0], resultVector[1], resultVector[2]);
        }

        public static void makeScaleMatrix(float sx, float sy, float sz)
        {
            matrixS[0, 0] = sx;
            matrixS[0, 1] = 0;
            matrixS[0, 2] = 0;
            matrixS[0, 3] = 0;
            matrixS[1, 0] = 0;
            matrixS[1, 1] = sy;
            matrixS[1, 2] = 0;
            matrixS[1, 3] = 0;
            matrixS[2, 0] = 0;
            matrixS[2, 1] = 0;
            matrixS[2, 2] = sz;
            matrixS[2, 3] = 0;
            matrixS[3, 0] = 0;
            matrixS[3, 1] = 0;
            matrixS[3, 2] = 0;
            matrixS[3, 3] = 1;
        }

        public static void makeMatrixTranslate(float dx, float dy, float dz)
        {
            matrix[0, 0] = 1;
            matrix[0, 1] = 0;
            matrix[0, 2] = 0;
            matrix[0, 3] = dx;
            matrix[1, 0] = 0;
            matrix[1, 1] = 1;
            matrix[1, 2] = 0;
            matrix[1, 3] = dy;
            matrix[2, 0] = 0;
            matrix[2, 1] = 0;
            matrix[2, 2] = 1;
            matrix[2, 3] = dz;
            matrix[3, 0] = 0;
            matrix[3, 1] = 0;
            matrix[3, 2] = 0;
            matrix[3, 3] = 1;
        }

        private static List<Vector3> transform(int num, float angle, Vector3 axis, Vector3 shift)
        {
            var triangle = templateHexagon.Select(t => new Vector3(t.X, t.Y, t.Z)).ToList();
            
            if (Math.Abs(angle) > float.Epsilon)
            {
                makeMatrixRotate(angle, axis);
                for (var i = 0; i < templateHexagon.Count; i++)
                    triangle[i] = multCoord(matrixR, triangle[i]);
            }
            
            makeScaleMatrix(scales[num].X, scales[num].Y, scales[num].Z);
            for (var i = 0; i < templateHexagon.Count; i++)
                triangle[i] = multCoord(matrixS, triangle[i]);

            makeMatrixTranslate(shift.X, shift.Y, shift.Z);
            for (var i = 0; i < templateHexagon.Count; i++)
                triangle[i] = multCoord(matrix, triangle[i]);

            return triangle;
        }

        public static void calcNormals()
        {
            var normalVector = normal(figure[0][0], figure[0][1], figure[0][2]);
            normals.Add(normalVector);

            // edges
            for (var i = 0; i < figure.Count - 1; i++)
            {
                //1
                normalVector = normal(figure[i][1], figure[i][0], figure[i + 1][0]);
                normals.Add(normalVector);

                //2
                normalVector = normal(figure[i][2], figure[i][1], figure[i + 1][1]);
                normals.Add(normalVector);

                //3
                normalVector = normal(figure[i][3], figure[i][2], figure[i + 1][2]);
                normals.Add(normalVector);

                //4
                normalVector = normal(figure[i][4], figure[i][3], figure[i + 1][3]);
                normals.Add(normalVector);

                //5
                normalVector = normal(figure[i][5], figure[i][4], figure[i + 1][4]);
                normals.Add(normalVector);

                //6
                normalVector = normal(figure[i][0], figure[i][5], figure[i + 1][5]);
                normals.Add(normalVector);
            }
            
            var j = figure.Count - 1;
            normals.Add(normal(figure[j][5], figure[j][4], figure[j][3]));
        }

        public static Vector3 smoothNormal(Vector3 vector1, Vector3 vector2, Vector3 vector3)
        {
            return (vector1.Normalized() + vector2.Normalized() + vector3.Normalized()).Normalized();
        }

        public static Vector3 smoothNormal(Vector3 vector1, Vector3 vector2, Vector3 vector3, Vector3 vector4)
        {
            return (vector1.Normalized() + vector2.Normalized() + vector3.Normalized() + vector4.Normalized());
        }

        public static void smoothNormals()
        {
            smoothednormals.Clear();
            
            smoothednormals.Add((normals[0] + normals[1] + normals[6]) / 3.0f);
            smoothednormals.Add((normals[0] + normals[1] + normals[2]) / 3.0f);
            smoothednormals.Add((normals[0] + normals[2] + normals[3]) / 3.0f);
            smoothednormals.Add((normals[0] + normals[3] + normals[4]) / 3.0f);
            smoothednormals.Add((normals[0] + normals[4] + normals[5]) / 3.0f);
            smoothednormals.Add((normals[0] + normals[5] + normals[6]) / 3.0f);

            var nN = 1;
            for (var i = 1; i < figure.Count - 1; i++)
            {
                smoothednormals.Add((normals[nN + 0] + normals[nN + 5] + normals[nN + 6] + normals[nN + 11]) / 4);
                smoothednormals.Add((normals[nN + 0] + normals[nN + 1] + normals[nN + 6] + normals[nN + 7]) / 4);
                smoothednormals.Add((normals[nN + 1] + normals[nN + 2] + normals[nN + 7] + normals[nN + 8]) / 4);
                smoothednormals.Add((normals[nN + 2] + normals[nN + 3] + normals[nN + 8] + normals[nN + 9]) / 4);
                smoothednormals.Add((normals[nN + 3] + normals[nN + 4] + normals[nN + 9] + normals[nN + 10]) / 4);
                smoothednormals.Add((normals[nN + 4] + normals[nN + 5] + normals[nN + 10] + normals[nN + 11]) / 4);

                nN += 6;
            }
            var j = normals.Count - 7;

            smoothednormals.Add((normals[j + 6] + normals[j + 5] + normals[j]) / 3.0f);
            smoothednormals.Add((normals[j + 6] + normals[j] + normals[j + 1]) / 3.0f);
            smoothednormals.Add((normals[j + 6] + normals[j + 1] + normals[j + 2]) / 3.0f);
            smoothednormals.Add((normals[j + 6] + normals[j + 2] + normals[j + 3]) / 3.0f);
            smoothednormals.Add((normals[j + 6] + normals[j + 3] + normals[j + 4]) / 3.0f);
            smoothednormals.Add((normals[j + 6] + normals[j + 4] + normals[j + 5]) / 3.0f);
        }

        //input
        public static void makeDuplication()
        {
            // triangle template
            var triangleFile = new StreamReader("res/hexagon.txt");
            var line = triangleFile.ReadLine();
            while (line != null)
            {
                var num = line.Split().Select(float.Parse).ToList();
                templateHexagon.Add(new Vector3(num[0], num[1], num[2]));
                line = triangleFile.ReadLine();
            }

            // trajectory
            var trajectoryFile = new StreamReader("res/trajectory.txt");
            line = trajectoryFile.ReadLine();
            while (line != null)
            {
                var num = line.Split().Select(float.Parse).ToList();
                replicationTray.Add(new Vector3(num[0], num[1], num[2]));
                line = trajectoryFile.ReadLine();
            }

            // percents
            var percentFile = new StreamReader("res/percent.txt");
            line = percentFile.ReadLine();
            while (line != null)
            {
                var num = line.Split().Select(float.Parse).ToList();
                var per = num[0];
                var scale = new Vector3
                {
                    X = num[1],
                    Y = num[2],
                    Z = num[3]
                };
                percents.Add(per);
                scales.Add(scale);
                line = percentFile.ReadLine();
            }

            var predPath = normal(templateHexagon[0], templateHexagon[1], templateHexagon[2]);
            var curPath = new Vector3();

            int i;
            float pathLength = 0;

            // path length
            for (i = 0; i < replicationTray.Count - 1; i++)
            {
                curPath.X = replicationTray[i + 1].X - replicationTray[i].X;
                curPath.Y = replicationTray[i + 1].Y - replicationTray[i].Y;
                curPath.Z = replicationTray[i + 1].Z - replicationTray[i].Z;
                pathLength += curPath.Length;
            }

            for (i = 0; i < percents.Count; i++)
            {
                // calculation the distance to the point of replication
                var pointDistance = (float)(pathLength * percents[i] / 100.0);
                
                var curPathLength = new Vector3();
                var curLength = 0.0f;
                var predLength = 0.0f;
                var found = false;
                int numPath;

                if (i == percents.Count - 1)
                    numPath = replicationTray.Count - 2;
                else
                {
                    int j;
                    for (j = 0; j < replicationTray.Count - 1 && !found; j++)
                    {
                        curPathLength.X = replicationTray[j + 1].X - replicationTray[j].X;
                        curPathLength.Y = replicationTray[j + 1].Y - replicationTray[j].Y;
                        curPathLength.Z = replicationTray[j + 1].Z - replicationTray[j].Z;
                        curLength += curPathLength.Length;
                        
                        if (pointDistance >= predLength && pointDistance <= curLength)
                            found = true;
                        else
                            predLength = curLength;
                    }
                    numPath = j - 1;
                }
                curPath.X = replicationTray[numPath + 1].X - replicationTray[numPath].X;
                curPath.Y = replicationTray[numPath + 1].Y - replicationTray[numPath].Y;
                curPath.Z = replicationTray[numPath + 1].Z - replicationTray[numPath].Z;
                
                var localPath = pointDistance - predLength;
                if (i == percents.Count - 1)
                    localPath = curPath.Length;
                var normCurPath = curPath.Length;
                
                var shift = new Vector3(replicationTray[numPath].X + localPath * curPath.X / normCurPath,
                    replicationTray[numPath].Y + localPath * curPath.Y / normCurPath,
                    replicationTray[numPath].Z + localPath * curPath.Z / normCurPath);
                
                var scal = scalar(curPath, predPath);
                
                var axis = cross(curPath, predPath);
                
                var angle = (float)(Math.Acos(scal / curPath.Length * predPath.Length) * 180.0 / Math.PI);
                
                if (scal < 0)
                    angle = 180 - angle;
                else
                    angle = -(180 + angle);

                if (Math.Abs(angle - 180) < float.Epsilon)
                    angle = 0;
                
                var triangle = transform(i, angle, axis, shift);

                figure.Add(triangle);
            }
        }

        public static void drawFigure(bool body, bool texture, bool smooth, Vector3 vec)
        {
            var nN = 0;

            GL.PushMatrix();
            GL.Translate(vec.X, vec.Y, vec.Z);

            if (body)
            {
                if (!smooth)
                {
                    if (texture)
                    {
                        #region C текстурой

                        texId = loadTexture("res/texture2.bmp");
                        GL.BindTexture(TextureTarget.Texture2D, texId);
                        GL.Color3(Color.White);

                        GL.Begin(PrimitiveType.Polygon);
                        GL.Normal3(normals[nN].X, normals[nN].Y, normals[nN].Z);
                        nN++;
                        for (var i = 0; i < templateHexagon.Count; i++)
                        {
                            GL.TexCoord2(figure[0][i].X / 3.5, figure[0][i].Y / 3.5);
                            GL.Vertex3(figure[0][i].X, figure[0][i].Y, figure[0][i].Z);
                        }

                        GL.End();

                        for (var i = 0; i < figure.Count - 1; i++)
                        {
                            GL.Begin(PrimitiveType.Polygon);
                            GL.Normal3(normals[nN].X, normals[nN].Y, normals[nN].Z);
                            nN++;

                            GL.TexCoord2(0, 1);
                            GL.Vertex3(figure[i][0].X, figure[i][0].Y, figure[i][0].Z);
                            GL.TexCoord2(1, 1);
                            GL.Vertex3(figure[i + 1][0].X, figure[i + 1][0].Y, figure[i + 1][0].Z);
                            GL.TexCoord2(1, 0);
                            GL.Vertex3(figure[i + 1][1].X, figure[i + 1][1].Y, figure[i + 1][1].Z);
                            GL.TexCoord2(0, 0);
                            GL.Vertex3(figure[i][1].X, figure[i][1].Y, figure[i][1].Z);

                            GL.End();
                            
                            GL.Begin(PrimitiveType.Polygon);
                            GL.Normal3(normals[nN].X, normals[nN].Y, normals[nN].Z);
                            nN++;

                            GL.TexCoord2(0, 1);
                            GL.Vertex3(figure[i][1].X, figure[i][1].Y, figure[i][1].Z);
                            GL.TexCoord2(1, 1);
                            GL.Vertex3(figure[i + 1][1].X, figure[i + 1][1].Y, figure[i + 1][1].Z);
                            GL.TexCoord2(1, 0);
                            GL.Vertex3(figure[i + 1][2].X, figure[i + 1][2].Y, figure[i + 1][2].Z);
                            GL.TexCoord2(0, 0);
                            GL.Vertex3(figure[i][2].X, figure[i][2].Y, figure[i][2].Z);

                            GL.End();
                            
                            GL.Begin(PrimitiveType.Polygon);
                            GL.Normal3(normals[nN].X, normals[nN].Y, normals[nN].Z);
                            nN++;

                            GL.TexCoord2(0, 1);
                            GL.Vertex3(figure[i][2].X, figure[i][2].Y, figure[i][2].Z);
                            GL.TexCoord2(1, 1);
                            GL.Vertex3(figure[i + 1][2].X, figure[i + 1][2].Y, figure[i + 1][2].Z);
                            GL.TexCoord2(1, 0);
                            GL.Vertex3(figure[i + 1][3].X, figure[i + 1][3].Y, figure[i + 1][3].Z);
                            GL.TexCoord2(0, 0);
                            GL.Vertex3(figure[i][3].X, figure[i][3].Y, figure[i][3].Z);

                            GL.End();
                            
                            GL.Begin(PrimitiveType.Polygon);
                            GL.Normal3(normals[nN].X, normals[nN].Y, normals[nN].Z);
                            nN++;

                            GL.TexCoord2(0, 1);
                            GL.Vertex3(figure[i][3].X, figure[i][3].Y, figure[i][3].Z);
                            GL.TexCoord2(1, 1);
                            GL.Vertex3(figure[i + 1][3].X, figure[i + 1][3].Y, figure[i + 1][3].Z);
                            GL.TexCoord2(1, 0);
                            GL.Vertex3(figure[i + 1][4].X, figure[i + 1][4].Y, figure[i + 1][4].Z);
                            GL.TexCoord2(0, 0);
                            GL.Vertex3(figure[i][4].X, figure[i][4].Y, figure[i][4].Z);

                            GL.End();
                            
                            GL.Begin(PrimitiveType.Polygon);
                            GL.Normal3(normals[nN].X, normals[nN].Y, normals[nN].Z);
                            nN++;
                            GL.TexCoord2(0, 1);
                            GL.Vertex3(figure[i][4].X, figure[i][4].Y, figure[i][4].Z);
                            GL.TexCoord2(1, 1);
                            GL.Vertex3(figure[i + 1][4].X, figure[i + 1][4].Y, figure[i + 1][4].Z);
                            GL.TexCoord2(1, 0);
                            GL.Vertex3(figure[i + 1][5].X, figure[i + 1][5].Y, figure[i + 1][5].Z);
                            GL.TexCoord2(0, 0);
                            GL.Vertex3(figure[i][5].X, figure[i][5].Y, figure[i][5].Z);
                            GL.End();
                            
                            GL.Begin(PrimitiveType.Polygon);
                            GL.Normal3(normals[nN].X, normals[nN].Y, normals[nN].Z);
                            nN++;
                            GL.TexCoord2(0, 1);
                            GL.Vertex3(figure[i][5].X, figure[i][5].Y, figure[i][5].Z);
                            GL.TexCoord2(1, 1);
                            GL.Vertex3(figure[i + 1][5].X, figure[i + 1][5].Y, figure[i + 1][5].Z);
                            GL.TexCoord2(1, 0);
                            GL.Vertex3(figure[i + 1][0].X, figure[i + 1][0].Y, figure[i + 1][0].Z);
                            GL.TexCoord2(0, 0);
                            GL.Vertex3(figure[i][0].X, figure[i][0].Y, figure[i][0].Z);
                            GL.End();
                        }

                        var j = figure.Count - 1;
                        GL.Begin(PrimitiveType.Polygon);
                        GL.Normal3(normals[nN].X, normals[nN].Y, normals[nN].Z);
                        for (var i = 0; i < templateHexagon.Count; i++)
                        {
                            GL.TexCoord2(figure[j][i].X / 3.5, figure[j][i].Y / 3.5);
                            GL.Vertex3(figure[j][i].X, figure[j][i].Y, figure[j][i].Z);
                        }

                        GL.End();

                        GL.BindTexture(TextureTarget.Texture2D, 0); //выбрать текстуру

                        #endregion
                    }
                    else
                    {
                        #region Без текстуры
                        
                        GL.Begin(PrimitiveType.Polygon);
                        GL.Normal3(normals[nN].X, normals[nN].Y, normals[nN].Z);
                        nN++;
                        GL.Color3(1f, 0, 0);
                        for (var i = 0; i < templateHexagon.Count; i++)
                            GL.Vertex3(figure[0][i].X, figure[0][i].Y, figure[0][i].Z);
                        GL.End();

                        for (var i = 0; i < figure.Count - 1; i++)
                        {
                            GL.Begin(PrimitiveType.Polygon);
                            GL.Normal3(normals[nN].X, normals[nN].Y, normals[nN].Z);
                            nN++;
                            GL.Color3(1f, 0, 0);
                            GL.Vertex3(figure[i][0].X, figure[i][0].Y, figure[i][0].Z);
                            GL.Vertex3(figure[i + 1][0].X, figure[i + 1][0].Y, figure[i + 1][0].Z);
                            GL.Vertex3(figure[i + 1][1].X, figure[i + 1][1].Y, figure[i + 1][1].Z);
                            GL.Vertex3(figure[i][1].X, figure[i][1].Y, figure[i][1].Z);
                            GL.End();
                            
                            GL.Begin(PrimitiveType.Polygon);
                            GL.Normal3(normals[nN].X, normals[nN].Y, normals[nN].Z);
                            nN++;
                            GL.Color3(1f, 0, 0);
                            GL.Vertex3(figure[i][1].X, figure[i][1].Y, figure[i][1].Z);
                            GL.Vertex3(figure[i + 1][1].X, figure[i + 1][1].Y, figure[i + 1][1].Z);
                            GL.Vertex3(figure[i + 1][2].X, figure[i + 1][2].Y, figure[i + 1][2].Z);
                            GL.Vertex3(figure[i][2].X, figure[i][2].Y, figure[i][2].Z);
                            GL.End();
                            
                            GL.Begin(PrimitiveType.Polygon);
                            GL.Normal3(normals[nN].X, normals[nN].Y, normals[nN].Z);
                            nN++;
                            GL.Color3(1f, 0, 0);
                            GL.Vertex3(figure[i][2].X, figure[i][2].Y, figure[i][2].Z);
                            GL.Vertex3(figure[i + 1][2].X, figure[i + 1][2].Y, figure[i + 1][2].Z);
                            GL.Vertex3(figure[i + 1][3].X, figure[i + 1][3].Y, figure[i + 1][3].Z);
                            GL.Vertex3(figure[i][3].X, figure[i][3].Y, figure[i][3].Z);
                            GL.End();
                            
                            GL.Begin(PrimitiveType.Polygon);
                            GL.Normal3(normals[nN].X, normals[nN].Y, normals[nN].Z);
                            nN++;
                            GL.Color3(1f, 0, 0);
                            GL.Vertex3(figure[i][3].X, figure[i][3].Y, figure[i][3].Z);
                            GL.Vertex3(figure[i + 1][3].X, figure[i + 1][3].Y, figure[i + 1][3].Z);
                            GL.Vertex3(figure[i + 1][4].X, figure[i + 1][4].Y, figure[i + 1][4].Z);
                            GL.Vertex3(figure[i][4].X, figure[i][4].Y, figure[i][4].Z);
                            GL.End();
                            
                            GL.Begin(PrimitiveType.Polygon);
                            GL.Normal3(normals[nN].X, normals[nN].Y, normals[nN].Z);
                            nN++;
                            GL.Color3(1f, 0, 0);
                            GL.Vertex3(figure[i][4].X, figure[i][4].Y, figure[i][4].Z);
                            GL.Vertex3(figure[i + 1][4].X, figure[i + 1][4].Y, figure[i + 1][4].Z);
                            GL.Vertex3(figure[i + 1][5].X, figure[i + 1][5].Y, figure[i + 1][5].Z);
                            GL.Vertex3(figure[i][5].X, figure[i][5].Y, figure[i][5].Z);
                            GL.End();
                            
                            GL.Begin(PrimitiveType.Polygon);
                            GL.Normal3(normals[nN].X, normals[nN].Y, normals[nN].Z);
                            nN++;
                            GL.Color3(1f, 0, 0);
                            GL.Vertex3(figure[i][5].X, figure[i][5].Y, figure[i][5].Z);
                            GL.Vertex3(figure[i + 1][5].X, figure[i + 1][5].Y, figure[i + 1][5].Z);
                            GL.Vertex3(figure[i + 1][0].X, figure[i + 1][0].Y, figure[i + 1][0].Z);
                            GL.Vertex3(figure[i][0].X, figure[i][0].Y, figure[i][0].Z);
                            GL.End();
                        }

                        var j = figure.Count - 1;
                        GL.Begin(PrimitiveType.Polygon);
                        GL.Normal3(-normals[nN].X, -normals[nN].Y, -normals[nN].Z);
                        GL.Color3(1f, 0, 0);
                        for (var i = 0; i < templateHexagon.Count; i++)
                            GL.Vertex3(figure[j][i].X, figure[j][i].Y, figure[j][i].Z);
                        GL.End();

                        #endregion
                    }
                }
                else
                {
                    if (texture)
                    {
                        #region C текстурой

                        texId = loadTexture("res/photo.jpg");
                        GL.BindTexture(TextureTarget.Texture2D, texId);
                        GL.Color3(Color.Red);
                        
                        GL.Begin(PrimitiveType.Polygon);
                        GL.Normal3(smoothednormals[nN].X, smoothednormals[nN].Y, smoothednormals[nN].Z);
                        nN++;
                        for (var i = 0; i < templateHexagon.Count; i++)
                        {
                            GL.TexCoord2(figure[0][i].X / 3.5, figure[0][i].Y / 3.5);
                            GL.Vertex3(figure[0][i].X, figure[0][i].Y, figure[0][i].Z);
                        }

                        GL.End();

                        for (var i = 0; i < figure.Count - 1; i++)
                        {
                            GL.Begin(PrimitiveType.Polygon);
                            GL.Normal3(smoothednormals[nN].X, smoothednormals[nN].Y, smoothednormals[nN].Z);
                            nN++;

                            GL.TexCoord2(0, 1);
                            GL.Vertex3(figure[i][0].X, figure[i][0].Y, figure[i][0].Z);
                            GL.TexCoord2(1, 1);
                            GL.Vertex3(figure[i + 1][0].X, figure[i + 1][0].Y, figure[i + 1][0].Z);
                            GL.TexCoord2(1, 0);
                            GL.Vertex3(figure[i + 1][1].X, figure[i + 1][1].Y, figure[i + 1][1].Z);
                            GL.TexCoord2(0, 0);
                            GL.Vertex3(figure[i][1].X, figure[i][1].Y, figure[i][1].Z);

                            GL.End();
                            
                            GL.Begin(PrimitiveType.Polygon);
                            GL.Normal3(smoothednormals[nN].X, smoothednormals[nN].Y, smoothednormals[nN].Z);
                            nN++;

                            GL.TexCoord2(0, 1);
                            GL.Vertex3(figure[i][1].X, figure[i][1].Y, figure[i][1].Z);
                            GL.TexCoord2(1, 1);
                            GL.Vertex3(figure[i + 1][1].X, figure[i + 1][1].Y, figure[i + 1][1].Z);
                            GL.TexCoord2(1, 0);
                            GL.Vertex3(figure[i + 1][2].X, figure[i + 1][2].Y, figure[i + 1][2].Z);
                            GL.TexCoord2(0, 0);
                            GL.Vertex3(figure[i][2].X, figure[i][2].Y, figure[i][2].Z);

                            GL.End();
                            
                            GL.Begin(PrimitiveType.Polygon);
                            GL.Normal3(smoothednormals[nN].X, smoothednormals[nN].Y, smoothednormals[nN].Z);
                            nN++;

                            GL.TexCoord2(0, 1);
                            GL.Vertex3(figure[i][2].X, figure[i][2].Y, figure[i][2].Z);
                            GL.TexCoord2(1, 1);
                            GL.Vertex3(figure[i + 1][2].X, figure[i + 1][2].Y, figure[i + 1][2].Z);
                            GL.TexCoord2(1, 0);
                            GL.Vertex3(figure[i + 1][3].X, figure[i + 1][3].Y, figure[i + 1][3].Z);
                            GL.TexCoord2(0, 0);
                            GL.Vertex3(figure[i][3].X, figure[i][3].Y, figure[i][3].Z);

                            GL.End();
                            
                            GL.Begin(PrimitiveType.Polygon);
                            GL.Normal3(smoothednormals[nN].X, smoothednormals[nN].Y, smoothednormals[nN].Z);
                            nN++;

                            GL.TexCoord2(0, 1);
                            GL.Vertex3(figure[i][3].X, figure[i][3].Y, figure[i][3].Z);
                            GL.TexCoord2(1, 1);
                            GL.Vertex3(figure[i + 1][3].X, figure[i + 1][3].Y, figure[i + 1][3].Z);
                            GL.TexCoord2(1, 0);
                            GL.Vertex3(figure[i + 1][4].X, figure[i + 1][4].Y, figure[i + 1][4].Z);
                            GL.TexCoord2(0, 0);
                            GL.Vertex3(figure[i][4].X, figure[i][4].Y, figure[i][4].Z);

                            GL.End();
                            
                            GL.Begin(PrimitiveType.Polygon);
                            GL.Normal3(smoothednormals[nN].X, smoothednormals[nN].Y, smoothednormals[nN].Z);
                            nN++;
                            GL.TexCoord2(0, 1);
                            GL.Vertex3(figure[i][4].X, figure[i][4].Y, figure[i][4].Z);
                            GL.TexCoord2(1, 1);
                            GL.Vertex3(figure[i + 1][4].X, figure[i + 1][4].Y, figure[i + 1][4].Z);
                            GL.TexCoord2(1, 0);
                            GL.Vertex3(figure[i + 1][5].X, figure[i + 1][5].Y, figure[i + 1][5].Z);
                            GL.TexCoord2(0, 0);
                            GL.Vertex3(figure[i][5].X, figure[i][5].Y, figure[i][5].Z);
                            GL.End();
                            
                            GL.Begin(PrimitiveType.Polygon);
                            GL.Normal3(smoothednormals[nN].X, smoothednormals[nN].Y, smoothednormals[nN].Z);
                            nN++;
                            GL.TexCoord2(0, 1);
                            GL.Vertex3(figure[i][5].X, figure[i][5].Y, figure[i][5].Z);
                            GL.TexCoord2(1, 1);
                            GL.Vertex3(figure[i + 1][5].X, figure[i + 1][5].Y, figure[i + 1][5].Z);
                            GL.TexCoord2(1, 0);
                            GL.Vertex3(figure[i + 1][0].X, figure[i + 1][0].Y, figure[i + 1][0].Z);
                            GL.TexCoord2(0, 0);
                            GL.Vertex3(figure[i][0].X, figure[i][0].Y, figure[i][0].Z);
                            GL.End();
                        }

                        var j = figure.Count - 1;
                        GL.Begin(PrimitiveType.Polygon);
                        GL.Normal3(smoothednormals[nN].X, smoothednormals[nN].Y, smoothednormals[nN].Z);
                        for (var i = 0; i < templateHexagon.Count; i++)
                        {
                            GL.TexCoord2(figure[j][i].X / 3.5, figure[j][i].Y / 3.5);
                            GL.Vertex3(figure[j][i].X, figure[j][i].Y, figure[j][i].Z);
                        }

                        GL.End();

                        GL.BindTexture(TextureTarget.Texture2D, 0); //выбрать текстуру

                        #endregion
                    }
                    else
                    {
                        #region Без текстуры
                        
                        GL.Begin(PrimitiveType.Polygon);
                        GL.Normal3(smoothednormals[nN].X, smoothednormals[nN].Y, smoothednormals[nN].Z);
                        nN++;
                        GL.Color3(1f, 0, 0);
                        for (var i = 0; i < templateHexagon.Count; i++)
                            GL.Vertex3(figure[0][i].X, figure[0][i].Y, figure[0][i].Z);
                        GL.End();

                        for (var i = 0; i < figure.Count - 1; i++)
                        {
                            GL.Begin(PrimitiveType.Polygon);
                            GL.Normal3(smoothednormals[nN].X, smoothednormals[nN].Y, smoothednormals[nN].Z);
                            nN++;
                            GL.Color3(1f, 0, 0);
                            GL.Vertex3(figure[i][0].X, figure[i][0].Y, figure[i][0].Z);
                            GL.Vertex3(figure[i + 1][0].X, figure[i + 1][0].Y, figure[i + 1][0].Z);
                            GL.Vertex3(figure[i + 1][1].X, figure[i + 1][1].Y, figure[i + 1][1].Z);
                            GL.Vertex3(figure[i][1].X, figure[i][1].Y, figure[i][1].Z);
                            GL.End();
                            
                            GL.Begin(PrimitiveType.Polygon);
                            GL.Normal3(smoothednormals[nN].X, smoothednormals[nN].Y, smoothednormals[nN].Z);
                            nN++;
                            GL.Color3(1f, 0, 0);
                            GL.Vertex3(figure[i][1].X, figure[i][1].Y, figure[i][1].Z);
                            GL.Vertex3(figure[i + 1][1].X, figure[i + 1][1].Y, figure[i + 1][1].Z);
                            GL.Vertex3(figure[i + 1][2].X, figure[i + 1][2].Y, figure[i + 1][2].Z);
                            GL.Vertex3(figure[i][2].X, figure[i][2].Y, figure[i][2].Z);
                            GL.End();
                            
                            GL.Begin(PrimitiveType.Polygon);
                            GL.Normal3(smoothednormals[nN].X, smoothednormals[nN].Y, smoothednormals[nN].Z);
                            nN++;
                            GL.Color3(1f, 0, 0);
                            GL.Vertex3(figure[i][2].X, figure[i][2].Y, figure[i][2].Z);
                            GL.Vertex3(figure[i + 1][2].X, figure[i + 1][2].Y, figure[i + 1][2].Z);
                            GL.Vertex3(figure[i + 1][3].X, figure[i + 1][3].Y, figure[i + 1][3].Z);
                            GL.Vertex3(figure[i][3].X, figure[i][3].Y, figure[i][3].Z);
                            GL.End();
                            
                            GL.Begin(PrimitiveType.Polygon);
                            GL.Normal3(smoothednormals[nN].X, smoothednormals[nN].Y, smoothednormals[nN].Z);
                            nN++;
                            GL.Color3(1f, 0, 0);
                            GL.Vertex3(figure[i][3].X, figure[i][3].Y, figure[i][3].Z);
                            GL.Vertex3(figure[i + 1][3].X, figure[i + 1][3].Y, figure[i + 1][3].Z);
                            GL.Vertex3(figure[i + 1][4].X, figure[i + 1][4].Y, figure[i + 1][4].Z);
                            GL.Vertex3(figure[i][4].X, figure[i][4].Y, figure[i][4].Z);
                            GL.End();
                            
                            GL.Begin(PrimitiveType.Polygon);
                            GL.Normal3(smoothednormals[nN].X, smoothednormals[nN].Y, smoothednormals[nN].Z);
                            nN++;
                            GL.Color3(1f, 0, 0);
                            GL.Vertex3(figure[i][4].X, figure[i][4].Y, figure[i][4].Z);
                            GL.Vertex3(figure[i + 1][4].X, figure[i + 1][4].Y, figure[i + 1][4].Z);
                            GL.Vertex3(figure[i + 1][5].X, figure[i + 1][5].Y, figure[i + 1][5].Z);
                            GL.Vertex3(figure[i][5].X, figure[i][5].Y, figure[i][5].Z);
                            GL.End();
                            
                            GL.Begin(PrimitiveType.Polygon);
                            GL.Normal3(smoothednormals[nN].X, smoothednormals[nN].Y, smoothednormals[nN].Z);
                            nN++;
                            GL.Color3(1f, 0, 0);
                            GL.Vertex3(figure[i][5].X, figure[i][5].Y, figure[i][5].Z);
                            GL.Vertex3(figure[i + 1][5].X, figure[i + 1][5].Y, figure[i + 1][5].Z);
                            GL.Vertex3(figure[i + 1][0].X, figure[i + 1][0].Y, figure[i + 1][0].Z);
                            GL.Vertex3(figure[i][0].X, figure[i][0].Y, figure[i][0].Z);
                            GL.End();
                        }

                        var j = figure.Count - 1;
                        GL.Begin(PrimitiveType.Polygon);
                        GL.Normal3(smoothednormals[nN].X, smoothednormals[nN].Y, smoothednormals[nN].Z);
                        GL.Color3(1f, 0, 0);
                        for (var i = 0; i < templateHexagon.Count; i++)
                            GL.Vertex3(figure[j][i].X, figure[j][i].Y, figure[j][i].Z);
                        GL.End();

                        #endregion
                    }
                }
            }
            else
            {
                for (var i = 0; i < templateHexagon.Count; i++)
                {
                    GL.Begin(PrimitiveType.LineStrip);
                    GL.Color3(200f, 0, 0);
                    foreach (var t in figure)
                        GL.Vertex3(t[i].X, t[i].Y, t[i].Z);
                    GL.End();
                }
                
                GL.LineWidth(5);
                GL.Begin(PrimitiveType.LineStrip);
                GL.Color3(200f, 0, 200f);
                foreach (var t in replicationTray)
                    GL.Vertex3(t.X, t.Y, t.Z);
                GL.End();
                
                GL.LineWidth(1);
                foreach (var t in figure)
                {
                    GL.Begin(PrimitiveType.LineLoop);
                    GL.Color3(200f, 0, 0);
                    for (var j = 0; j < templateHexagon.Count; j++)
                        GL.Vertex3(t[j].X, t[j].Y, t[j].Z);
                    GL.End();
                }
            }
            GL.PopMatrix();
        }

        public static void DrawNormal(bool smoothNormal, Vector3 vec)
        {
            int nN;

            GL.PushMatrix();
            GL.Translate(vec.X, vec.Y, vec.Z);
            
            if (!smoothNormal)
            {
                for (var i = 0; i < templateHexagon.Count; i++)
                {
                    GL.Begin(PrimitiveType.LineStrip);
                    GL.Color3(Color.Cyan);
                    GL.Vertex3(figure[0][i].X, figure[0][i].Y, figure[0][i].Z);
                    GL.Vertex3(figure[0][i].X + normals[0].X, figure[0][i].Y + normals[0].Y,
                        figure[0][i].Z + normals[0].Z);
                    GL.End();
                }

                nN = 1;
                for (var i = 0; i < figure.Count - 1; i++)
                {
                    for (int k = 0, l = 1; k < templateHexagon.Count; k++, l++)
                    {
                        if (l > 5)
                            l = 0;

                        GL.Begin(PrimitiveType.LineStrip);
                        GL.Color3(Color.Cyan);
                        GL.Vertex3(figure[i][k].X, figure[i][k].Y, figure[i][k].Z);
                        GL.Vertex3(figure[i][k].X + normals[nN].X, figure[i][k].Y + normals[nN].Y,
                            figure[i][k].Z + normals[nN].Z);
                        GL.End();

                        GL.Begin(PrimitiveType.LineStrip);
                        GL.Color3(Color.Cyan);
                        GL.Vertex3(figure[i + 1][k].X, figure[i + 1][k].Y, figure[i + 1][k].Z);
                        GL.Vertex3(figure[i + 1][k].X + normals[nN].X, figure[i + 1][k].Y + normals[nN].Y,
                            figure[i + 1][k].Z + normals[nN].Z);
                        GL.End();

                        GL.Begin(PrimitiveType.LineStrip);
                        GL.Color3(Color.Cyan);
                        GL.Vertex3(figure[i + 1][l].X, figure[i + 1][l].Y, figure[i + 1][l].Z);
                        GL.Vertex3(figure[i + 1][l].X + normals[nN].X, figure[i + 1][l].Y + normals[nN].Y,
                            figure[i + 1][l].Z + normals[nN].Z);
                        GL.End();

                        GL.Begin(PrimitiveType.LineStrip);
                        GL.Color3(Color.Cyan);
                        GL.Vertex3(figure[i][l].X, figure[i][l].Y, figure[i][l].Z);
                        GL.Vertex3(figure[i][l].X + normals[nN].X, figure[i][l].Y + normals[nN].Y,
                            figure[i][l].Z + normals[nN].Z);
                        GL.End();

                        nN++;
                    }
                }

                var j = figure.Count - 1;

                for (var i = 0; i < templateHexagon.Count; i++)
                {
                    GL.Begin(PrimitiveType.LineStrip);
                    GL.Color3(Color.Cyan);
                    GL.Vertex3(figure[j][i].X, figure[j][i].Y, figure[j][i].Z);
                    GL.Vertex3(figure[j][i].X + normals[nN].X, figure[j][i].Y + normals[nN].Y,
                        figure[j][i].Z + normals[nN].Z);
                    GL.End();
                }
            }
            
            else
            {
                nN = 0;
                for (var i = 0; i < figure.Count; i++)
                {
                    GL.Color3(255f, 0, 0);
                    GL.Begin(PrimitiveType.LineStrip);
                    GL.Vertex3(figure[i][0].X, figure[i][0].Y, figure[i][0].Z);
                    GL.Vertex3(figure[i][0].X + smoothednormals[nN].X, figure[i][0].Y + smoothednormals[nN].Y,
                        figure[i][0].Z + smoothednormals[nN].Z);
                    GL.End();

                    GL.Color3(255f, 0, 0);
                    GL.Begin(PrimitiveType.LineStrip);
                    GL.Vertex3(figure[i][1].X, figure[i][1].Y, figure[i][1].Z);
                    GL.Vertex3(figure[i][1].X + smoothednormals[nN + 1].X, figure[i][1].Y + smoothednormals[nN + 1].Y,
                        figure[i][1].Z + smoothednormals[nN + 1].Z);
                    GL.End();

                    GL.Color3(255f, 0, 0);
                    GL.Begin(PrimitiveType.LineStrip);
                    GL.Vertex3(figure[i][2].X, figure[i][2].Y, figure[i][2].Z);
                    GL.Vertex3(figure[i][2].X + smoothednormals[nN + 2].X, figure[i][2].Y + smoothednormals[nN + 2].Y,
                        figure[i][2].Z + smoothednormals[nN + 2].Z);
                    GL.End();

                    GL.Color3(255f, 0, 0);
                    GL.Begin(PrimitiveType.LineStrip);
                    GL.Vertex3(figure[i][3].X, figure[i][3].Y, figure[i][3].Z);
                    GL.Vertex3(figure[i][3].X + smoothednormals[nN + 3].X, figure[i][3].Y + smoothednormals[nN + 3].Y,
                        figure[i][3].Z + smoothednormals[nN + 3].Z);
                    GL.End();

                    GL.Color3(255f, 0, 0);
                    GL.Begin(PrimitiveType.LineStrip);
                    GL.Vertex3(figure[i][4].X, figure[i][4].Y, figure[i][4].Z);
                    GL.Vertex3(figure[i][4].X + smoothednormals[nN + 4].X, figure[i][4].Y + smoothednormals[nN + 4].Y,
                        figure[i][4].Z + smoothednormals[nN + 4].Z);
                    GL.End();

                    GL.Color3(255f, 0, 0);
                    GL.Begin(PrimitiveType.LineStrip);
                    GL.Vertex3(figure[i][5].X, figure[i][5].Y, figure[i][5].Z);
                    GL.Vertex3(figure[i][5].X + smoothednormals[nN + 5].X, figure[i][5].Y + smoothednormals[nN + 5].Y,
                        figure[i][5].Z + smoothednormals[nN + 5].Z);
                    GL.End();

                    nN += 6;
                }
            }
            GL.PopMatrix();
        }

        public static int loadTexture(string filename)
        {
            var id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);
            var bmp = new Bitmap(filename);
            var bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmpData.Width, bmpData.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmpData.Scan0);

            bmp.UnlockBits(bmpData);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                (int)TextureMagFilter.Linear);
            return id;
        }
    }
}
