using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PurpleMoon.Core;

namespace PurpleMoon
{
    public struct Rectangle
    {
        public Point Position { get { return new Point(X, Y); } set { X = value.X; Y = value.Y; } }
        public Point Size     { get { return new Point(W, H); } set { W = value.X; H = value.Y; } }

        public int X, Y, W, H;

        public Rectangle() { X = 0; Y = 0; W = 0; H = 0; }
        public Rectangle(int x, int y, int w, int h) { X = x; Y = y; W = w; H = h; }

        public override bool Equals([NotNullWhen(true)] object obj)
        {
            if (obj.GetType() != typeof(Rectangle)) { Debug.Panic("Attempt to compare 'Rectangle' to invalid type"); return false; }
            return Equals((Rectangle)obj);
        }

        public bool Equals(Rectangle r) { return (X == r.X && Y == r.Y && W == r.W && H == r.H); }

        public bool Contains(int x, int y) { return (x >= X && y >= Y && x < X + W && y < Y + H); }

        public bool Contains(Point pos) { return Contains(pos.X, pos.Y); }

        public bool Contains(Rectangle r) { return Contains(r.X, r.Y); }

        public override int GetHashCode() { return base.GetHashCode(); }

        public static bool operator ==(Rectangle r1, Rectangle r2) { return r1.Equals(r2); }

        public static bool operator !=(Rectangle r1, Rectangle r2) { return !r1.Equals(r2); }

        public static Rectangle Empty { get { return new Rectangle(0, 0, 0, 0); } }

    }
}
