using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WololoGame
{
    class Camera
    {
        public Vector3 Position;
        public Vector3 Target;
        public Vector3 Up;
        public float AspectRatio = 1;
        public float NearPlane = 1;
        public float FarPlane = 1000;
        public float FOV = MathHelper.PiOver4;

        public Matrix View
        {
            get
            {
                return Matrix.CreateLookAt(Position, Target, Up);
            }
        }

        public Matrix Projection
        {
            get
            {
                return Matrix.CreatePerspectiveFieldOfView(FOV, AspectRatio, NearPlane, FarPlane);
            }
        }

        public Camera CreateReflection()
        {
            var cam = (Camera)MemberwiseClone();
            cam.Position.Y = -cam.Position.Y;
            cam.Target.Y = -cam.Target.Y;
            cam.Up.X = -cam.Up.X;
            cam.Up.Z = -cam.Up.Z;
            return cam;
        }
        public Camera CreateMinimap()
        {
            return new Camera
            {
                AspectRatio = 1,
                NearPlane = 100,
                FarPlane = 1100,
                FOV = MathHelper.PiOver4,
                Position = new Vector3(Position.X, 200, Position.Z),
                Target = new Vector3(Position.X, 0, Position.Z),
                Up = new Vector3(0, 0, 1)
            };
        }

    }
}
