using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PurpleMoon.Core;
using PurpleMoon.HAL;
using PurpleMoon.Lib;

namespace PurpleMoon.Graphics
{
    public class Image
    {
        public Point  Size { get; private set; }
        public uint[] Data { get; private set; }

        public Image(int w, int h)
        {
            this.Size = new Point(w, h);
            this.Data = new uint[w * h];
        }

        public Image(string fname) { Load(fname); }

        public void Load(string fname)
        {
            Debug.Info("Attempting to load bitmap at '%s'", fname);
            BinaryStream stream = new BinaryStream(fname);
            stream.Seek(14);
            int size = stream.ReadInt(), w = stream.ReadInt(), h = stream.ReadInt();
            ushort planes = stream.ReadUShort(), bpp = stream.ReadUShort();
            if (w == 0 || h == 0) { Debug.Panic("Invalid size while loading bitmap - %dx%d", w, h); return; }

            stream.Seek(10);
            uint off_bits = stream.ReadUInt();
            Debug.Info("Bitmap - Size:%dx%d Planes:%d BPP:%d, Offset:%p", w, h, planes, bpp, off_bits);

            this.Size = new Point(w, h);
            this.Data = new uint[w * h];

            for (int yy = h - 1; yy >= 0; yy--)
            {
                for (int xx = 0; xx < w; xx++)
                {
                    uint o = (uint)(4 * (yy * w + xx));
                    uint c = (uint)((0xFF << 24) | (stream.Data[off_bits + o + 2] << 16) | (stream.Data[off_bits + o + 1] << 8) | stream.Data[off_bits + o]);
                    uint d = (uint)(xx + ((h - yy - 1) * w));
                    Data[d] = c;
                }
            }
            Debug.Info("Successfully loaded bitmap - Size:%dx%d File:%s", w, h, fname);
        }

        public void Resize(int w, int h)
        {
            if (w == Size.X && h == Size.Y) { return; }
            uint[] data = new uint[w * h];
            double xr = (double)Size.X / (double)w;
            double yr = (double)Size.Y / (double)h;

            int i, j, px, py;
            for (i = 0; i < h; i++)
            {
                for (j = 0; j < w; j++)
                {
                    px = (int)Math.Floor(j * xr);
                    py = (int)Math.Floor(i * yr);
                    data[i * w + j] = Data[py * Size.X + px];
                }
            }
            Data = null;
            Data = data;
            Size = new Point(w, h);
        }

        public void Swap(Image image) { Cosmos.Core.MemoryOperations.Copy(Data, image.Data); }

        public void Clear(Color color)
        {
            Cosmos.Core.MemoryOperations.Fill(Data, color.Pack());
            //Array.Fill(Data, color.Pack(), 0, Size.X * Size.Y);
        }

        public void DrawPixel(int x, int y, uint color)
        {
            if ((uint)x >= (uint)Size.X || (uint)y >= (uint)Size.Y) { return; }
            Data[y * Size.X + x] = color;
        }

        public void DrawPixel(int x, int y, Color color)
        {
            if ((uint)x >= (uint)Size.X || (uint)y >= (uint)Size.Y) { return; }
            Data[y * Size.X + x] = color.Pack();
        }

        public void DrawFilledRect(int x, int y, int w, int h, Color color)
        {
            uint c = color.Pack();
            for (int i = 0; i < w * h; i++) { DrawPixel(x + (i % w), y + (i / w), c); }
        }

        public void DrawRect(int x, int y, int w, int h, int t, Color color)
        {

        }

        public void DrawRectPopup(int x, int y, int w, int h, Color ctl, Color cbro, Color cbri)
        {
            DrawFilledRect(x, y, w - 1, 1, ctl);
            DrawFilledRect(x, y, 1, h - 1, ctl);
            DrawFilledRect(x + 1, y + h - 2, w - 2, 1, cbri);
            DrawFilledRect(x + w - 2, y + 1, 1, h - 2, cbri);
            DrawFilledRect(x, y + h - 1, w, 1, cbro);
            DrawFilledRect(x + w - 1, y, 1, h, cbro);
        }

        public void DrawChar(int x, int y, char c, Color fg, Color bg, PCScreenFont font)
        {
            uint fp = fg.Pack(), bp = bg.Pack();
            int xx = x;
            for (int j = 0; j < font.Height; j++)
            {
                int glyph = font.Data[c * font.Height + j];
                for (int i = 0; i < 8; i++)
                {
                    if ((glyph & 0x80) >= 1) { DrawPixel(xx, y, fp); }
                    else if (bg != Color.Transparent) { DrawPixel(xx, y, bp); }
                    glyph <<= 1;
                    xx++;
                }
                y++;
                xx = x;
            }
        }

        public void DrawString(int x, int y, string txt, Color fg, Color bg, PCScreenFont font)
        {
            int i = 0, xx = x, yy = y, cw = font.GetWidth(), ch = font.GetHeight();
            while (i < txt.Length)
            { 
                if (txt[i] == '\n') { xx = x; yy += ch; }
                else { DrawChar(xx, yy, txt[i], fg, bg, font); xx += cw; }
                i++;
            }
        }

        public void Draw(int x, int y, int w, int h, Color[] data)
        {
            int xx = 0, yy = 0, i = 0;
            for (yy = 0; yy < h; yy++)
            {
                for (xx = 0; xx < w; xx++)
                {
                    i = yy * w + xx;
                    if (data[i] != Color.Transparent) { DrawPixel(x + xx, y + yy, data[i]); }
                }
            }
        }

        public void Draw(Rectangle bounds, Color[] data) { Draw(bounds.X, bounds.Y, bounds.W, bounds.H, data); }

        public void Draw(int x, int y, int w, int h, uint[] data)
        {
            int xx = 0, yy = 0, i = 0;
            for (yy = 0; yy < h; yy++)
            {
                for (xx = 0; xx < w; xx++)
                {
                    i = yy * w + xx;
                    if (data[i] != 0x00FFFFFF) { DrawPixel(x + xx, y + yy, data[i]); }
                }
            }
        }

        public void Draw(Rectangle bounds, uint[] data) { Draw(bounds.X, bounds.Y, bounds.W, bounds.H, data); }
    }
}
