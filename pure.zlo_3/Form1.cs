using System;
using System.Drawing;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using pure.zlo_3.source;

namespace pure.zlo_3
{
    public partial class Canvas : Form
    {
        private bool _initialized;
        private bool _isOrtho;
        private readonly int _lightMode;
        private readonly Camera _camera = new Camera();

        private bool _mouseRotate;
        private bool _mouseXzMove;
        private bool _mouseYMove;
        private int _myMouseYcoord;
        private int _myMouseXcoord;
        private int _myMouseYcoordVar;
        private int _myMouseXcoordVar;

        private Vector3 _mov = new Vector3(0, 0, 0);

        public Canvas()
        {
            InitializeComponent();
            _lightMode = 1;
        }

        private void glV_Paint(object sender, PaintEventArgs e)
        {
            if (!_initialized) return;
            GL.ClearColor(Color.Black);
            GL.LoadIdentity();
            mouseEvents();
            _camera.update();

            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
            _camera.look();

            var vec = new Vector3(_mov.X, _mov.Y, _mov.Z);

            if (coordinateGrid_checkBox.Checked)
            {
                Renderer.grid();
                //Renderer.Axis();
            }

            Renderer.lightOn(_lightMode);
            Renderer.drawFigure(surfaces_checkBox.Checked, texturing_checkBox.Checked, smoothing_checkBox.Checked, vec);

            if (showNormals_checkBox.Checked)
                Renderer.DrawNormal(smoothing_checkBox.Checked, vec);
            GL.Disable(EnableCap.Light0);

            glV.SwapBuffers();
        }

        private void glV_Resize(object sender, EventArgs e)
        {
            var c = sender as GLControl;
            if (c == null) return;

            if (c.ClientSize.Height == 0)
                c.ClientSize = new Size(c.ClientSize.Width, 1);

            GL.Viewport(0, 0, c.ClientSize.Width, c.ClientSize.Height);
            CanvasRefresh();
        }

        private void glV_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.A: _mov.X -= 2; break;
                case Keys.D: _mov.X += 2; break;
                case Keys.S: _mov.Y -= 2; break;
                case Keys.W: _mov.Y += 2; break;

                case Keys.Q: _mov.Z -= 2; break;
                case Keys.E: _mov.Z += 2; break;
            }
            CanvasRefresh();
        }

        private void glV_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.None) return;
            _myMouseXcoordVar = e.Y;
            _myMouseYcoordVar = e.X;
            CanvasRefresh();
        }

        private void glV_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                _mouseRotate = true; // Если нажата левая кнопка мыши

            if (e.Button == MouseButtons.Right)
                _mouseXzMove = true; // Если нажата правая кнопка мыши

            if (e.Button == MouseButtons.Middle)
                _mouseYMove = true; // Если нажата средняя кнопка мыши

            _myMouseYcoord = e.X; // Передаем в нашу глобальную переменную позицию мыши по Х
            _myMouseXcoord = e.Y;
            CanvasRefresh();
        }

        private void glV_MouseUp(object sender, MouseEventArgs e)
        {
            glV.Cursor = Cursors.Arrow; //меняем указатель
            _mouseRotate = _mouseXzMove = _mouseYMove = false;
            CanvasRefresh();
        }

        private void glV_Load(object sender, EventArgs e)
        {
            glV.MakeCurrent();
            GL.Enable(EnableCap.DepthTest);
            GL.Viewport(0, 0, glV.ClientSize.Width, glV.ClientSize.Height);

            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            try
            {
                switchProjection();

                _camera.cameraPosition(19, 9, 4, 0, 0, 0, 0, 10, 0);
                _camera.zoom(-3 / 50.0f);

                Renderer.makeDuplication();
                Renderer.calcNormals();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message + "\n\n" + err.StackTrace, "Error during initialization");
                Application.Exit();
            }

            _initialized = true;
        }

        private void prospect_radioButton_Click(object sender, EventArgs e)
        {
            _isOrtho = false;
            switchProjection();
            CanvasRefresh();
        }

        private void ortografia_radioButton_Click(object sender, EventArgs e)
        {
            _isOrtho = true;
            switchProjection();
            CanvasRefresh();
        }

        private void showNormals_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            smoothing_checkBox.Enabled = showNormals_checkBox.Checked;
            CanvasRefresh();
        }

        private void smoothing_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (smoothing_checkBox.Checked)
                Renderer.smoothNormals();
            CanvasRefresh();
        }

        private void surfaces_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            texturing_checkBox.Enabled = surfaces_checkBox.Checked;
            CanvasRefresh();
        }

        private void texturing_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            CanvasRefresh();
        }

        private void mouseEvents()
        {
            if (_mouseRotate) // Если нажата левая кнопка мыши
            {
                glV.Cursor = Cursors.SizeAll; //меняем указатель

                _camera.rotateXY(_myMouseYcoordVar - _myMouseYcoord, _myMouseXcoordVar - _myMouseXcoord);

                _myMouseYcoord = _myMouseYcoordVar;
                _myMouseXcoord = _myMouseXcoordVar;
            }
            else if (_mouseXzMove)
            {
                glV.Cursor = Cursors.SizeAll;

                _camera.xzMovement((float)(_myMouseXcoordVar - _myMouseXcoord) / 50);
                _camera.strafe(-((float)(_myMouseYcoordVar - _myMouseYcoord) / 50));

                _myMouseYcoord = _myMouseYcoordVar;
                _myMouseXcoord = _myMouseXcoordVar;
            }
            else if (_mouseYMove)
            {
                glV.Cursor = Cursors.SizeAll;

                _camera.yCameraMovement((float)(_myMouseXcoordVar - _myMouseXcoord) / 50);

                _myMouseYcoord = _myMouseYcoordVar;
                _myMouseXcoord = _myMouseXcoordVar;
            }
            else
            {
                glV.Invoke(new Action(() => glV.Cursor = Cursors.Default)); // возвращаем курсор
            }
        }
        private void CanvasRefresh()
        {
            glV.Invalidate();
        }

        private void switchProjection()
        {
            GL.MatrixMode(MatrixMode.Projection);

            var perpective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, glV.AspectRatio, 1, 100);
            var ortho = Matrix4.CreateOrthographic(20 * glV.AspectRatio, 20, 1, 100);

            if (_isOrtho)
                GL.LoadMatrix(ref ortho);
            else
                GL.LoadMatrix(ref perpective);

            GL.MatrixMode(MatrixMode.Modelview);
            CanvasRefresh();
        }

        private void coordinateGrid_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            CanvasRefresh();
        }
    }
}
