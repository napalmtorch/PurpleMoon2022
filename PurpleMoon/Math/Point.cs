using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurpleMoon
{
    public struct Point
    {
        public static Point Zero { get { return new Point(0, 0); } }

        public int X, Y;

        public Point() { X = 0; Y = 0; }
        public Point(int x, int y) { X = x; Y = y; }

        public bool Equals(Point p) { return (X == p.X && Y == p.Y); }
    }
}
