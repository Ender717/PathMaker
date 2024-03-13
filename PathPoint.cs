using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PathMaker
{
    internal class PathPoint
    {
        public float X { get; set; }
        public float Y { get; set; }

        public PathPoint(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static PathPoint operator +(PathPoint a, PathPoint b) => new PathPoint(a.X + b.X, a.Y + b.Y);
        public static PathPoint operator -(PathPoint a, PathPoint b) => new PathPoint(a.X - b.X, a.Y - b.Y);
        public static PathPoint operator *(PathPoint a, float b) => new PathPoint(a.X * b, a.Y * b);
        public static PathPoint operator *(float a, PathPoint b) => new PathPoint(a * b.X, a * b.Y);
        public static PathPoint operator /(PathPoint a, float b) => new PathPoint(a.X / b, a.Y / b);
    }
}
