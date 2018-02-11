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

        private static readonly List<Vector3> smoothedNormals = new List<Vector3>();
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
    }
}
