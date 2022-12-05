using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PurpleMoon.Core;

namespace PurpleMoon.Graphics
{
    public class PCScreenFont
    {
        public int Height { get; private set; }
        public Point Spacing { get; private set; }
        public byte[] Data { get; private set; }

        public PCScreenFont()
        {
            Height = 0;
            Spacing = Point.Zero;
            Data = null;
        }

        public PCScreenFont(string fname) { Load(fname, Point.Zero); }

        public PCScreenFont(string fname, Point spacing) { Load(fname, spacing); }

        public void Load(string fname, Point spacing)
        {
            if (!File.Exists(fname)) { Debug.Panic("Unable to locate PSF font at '%s'", fname); }
            byte[] data = FileSystem.ReadBytes(fname);
            ushort magic = (ushort)((data[1] << 8) | data[0]);
            byte mode = data[2], charsz = data[3];

            if (magic != 0x0436) { Debug.Panic("Invalid magic number for PSF font at '%s'", fname); }
            Height = charsz;
            Spacing = spacing;
            Data = new byte[data.Length];

            int i, j = 0;
            for (i = 4; i < data.Length; i++) { Data[j++] = data[i]; }

            Debug.OK("Loaded font - Size:%dx%d Spacing:%dx%d File:%s", 8, Height, Spacing.X, Spacing.Y, fname);
        }

        public int GetWidth(bool spacing = true) { return 8 + (spacing ? Spacing.X : 0); }

        public int GetWidth(string str, bool spacing = true) { return str.Length * GetWidth(spacing); }

        public int GetHeight(bool spacing = true) { return Height + (spacing ? Spacing.Y : 0); }
    }
}
