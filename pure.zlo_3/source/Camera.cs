using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace pure.zlo_3.source
{
    class Camera
    {
        private Vector3 _position;      // position vector
        private Vector3 _strafe;        // strafe vector
        private Vector3 _up;            // up direction vector
        private Vector3 _view;          // cameras view vector

        public void look()
        {
            var lookAt = Matrix4.LookAt(_position, _view, _up);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref lookAt);
        }

        public void yMovement(float speed)
        {
            _position.Y += speed;
        }

        public void xzMovement(float speed)
        {
            var vector = Vector3.Normalize(_view - _position); // sight vector

            _position.X += vector.X * speed;
            _position.Z += vector.Z * speed;
            _view.X += vector.X * speed;
            _view.Z += vector.Z * speed;
        }

        public void zoom(float delta)
        {
            var vector = _view - _position;
            if (vector.Length < 30 || delta > 0)
            {
                vector = Vector3.Normalize(vector);
                var temp = _position + vector * delta;
                if (temp.X / _position.X > 0)
                {
                    _position = temp;
                } 
            }
        }

        public void yCameraMovement(float speed)
        {
            _view.Y += speed;
        }

        public void strafe(float speed)
        {
            // add strafe vector to current position, and then to sight
            _position.X += _strafe.X * speed;
            _position.Z += _strafe.Z * speed;
            _view.X += _strafe.X * speed;
            _view.Z += _strafe.Z * speed;
        }

        public void rotatePosition(float angle, float x, float y, float z)
        {
            _position = _position - _view;

            var vectorA = _position;
            Vector3 vectorB;

            var sinA = (float)Math.Sin(Math.PI * angle / 180.0);
            var cosA = (float)Math.Cos(Math.PI * angle / 180.0);

            // find new coords for a current point
            vectorB.X = (cosA + (1 - cosA) * x * x) * vectorA.X;
            vectorB.X += ((1 - cosA) * x * y - z * sinA) * vectorA.Y;
            vectorB.X += ((1 - cosA) * x * z + y * sinA) * vectorA.Z;

            vectorB.Y = ((1 - cosA) * x * y + z * sinA) * vectorA.X;
            vectorB.Y += (cosA + (1 - cosA) * y * y) * vectorA.Y;
            vectorB.Y += ((1 - cosA) * y * z - x * sinA) * vectorA.Z;

            vectorB.Z = ((1 - cosA) * x * z - y * sinA) * vectorA.X;
            vectorB.Z += ((1 - cosA) * y * z + x * sinA) * vectorA.Y;
            vectorB.Z += (cosA + (1 - cosA) * z * z) * vectorA.Z;

            _position = _view + vectorB;
        }

        public void cameraPosition(float pX, float pY, float pZ, float vX, float vY, float vZ, float uX, float uY, float uZ)
        {
            // cameras position
            _position.X = pX; 
            _position.Y = pY; 
            _position.Z = pZ;
            // sight
            _view.X = vX; 
            _view.Y = vY; 
            _view.Z = vZ;
            // up
            _up.X = uX;
            _up.Y = uY; 
            _up.Z = uZ; 
        }
        #region Camera && sight components getters
        public double getPosX()
        {
            return _position.X;
        }

        public double getPosY()
        {
            return _position.Y;
        }

        public double getPosZ()
        {
            return _position.Z;
        }

        public double getViewX()
        {
            return _view.X;
        }

        public double getViewY()
        {
            return _view.Y;
        }

        public double getViewZ()
        {
            return _view.Z;
        }
        #endregion

        private static Vector3 cross(Vector3 _v, Vector3 _p, Vector3 _u)
        {
            Vector3 norm;
            var vector = _v - _p;

            norm.X = vector.Y * _u.Z - vector.Z * _u.Y;

            norm.Y = vector.Z * _u.X - vector.X * _u.Z;

            norm.Z = vector.X * _u.Y - vector.Y * _u.X;

            return norm;
        }

        public void update()
        {
            var crossVector = cross(_view, _position, _up);

            _strafe = Vector3.Normalize(crossVector);
        }

        public void rotateXY(float fi, float theta)
        {
            rotatePosition(fi, 0, 1, 0);
            rotatePosition(theta, _strafe.X, _strafe.Y, _strafe.Z);
            if (_position.X * _position.X + _position.Z * _position.Z - _view.X * _view.X - _view.Z * _view.Z < 5 || _position.Y < 0.1)
            {
                rotatePosition(-theta, _strafe.X, _strafe.Y, _strafe.Z);
            }
        }
    }
}
