using PurpleMoon.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurpleMoon.Graphics
{
    public struct Color
    {
        public byte A, R, G, B;

        public Color() { A = 0; R = 0; G = 0; B = 0; }
        public Color(byte a, byte r, byte g, byte b) { A = a; R = r; G = g; B = b; }

        public void Unpack(uint color)
        {
            A = (byte)((color & 0xFF000000) >> 24);
            R = (byte)((color & 0x00FF0000) >> 16);
            G = (byte)((color & 0x0000FF00) >> 8);
            B = (byte)((color & 0x000000FF) >> 0);
        }

        public uint Pack() { return (uint)((A << 24) | (R << 16) | (G << 8) | B); }

        public bool Equals(Color color) { return (A == color.A) && (R == color.R) && (G == color.G) && (B == color.B); }

        public override bool Equals([NotNullWhen(true)] object obj)
        {
            if (obj.GetType() != typeof(Color)) { Debug.Panic("Attempt to compare 'Color' to invalid type"); return false; }
            return Equals((Color)obj);
        }

        public override int GetHashCode() { return base.GetHashCode(); }

        public static bool operator ==(Color c1, Color c2) { return c1.Equals(c2); }        

        public static bool operator !=(Color c1, Color c2) { return !c1.Equals(c2); }

        public static Color Transparent  { get; private set; } = new Color(0, 0xFF, 0xFF, 0xFF);
        public static Color Black        { get; private set; } = new Color(0xFF, 0x00, 0x00, 0x00);
        public static Color White        { get; private set; } = new Color(0xFF, 0xFF, 0xFF, 0xFF);
        public static Color Magenta      { get; private set; } = new Color(0xFF, 0xFF, 0x00, 0xFF);
        public static Color Red          { get; private set; } = new Color(0xFF, 0xFF, 0x00, 0x00);
    }
}
