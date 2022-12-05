using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PurpleMoon.Core;
using PurpleMoon.Graphics;

namespace PurpleMoon.HAL
{
    public enum VideoDevice
    {
        Invalid,
        VGA,
        VBE,
        VMWareSVGAII,
    }

    public class VideoDriver : Driver
    {
        public VideoDevice Type { get; private set; }
        public Point       Size { get { return _sz; } }

        protected Point _sz;

        public VideoDriver(string name) : base(name)
        {
            Type = VideoDevice.Invalid;
            _sz  = Point.Zero;
        }

        public override void Start() { }       

        public override void Stop() { }

        public virtual void Swap(uint[] data) { }
        public virtual void Clear(uint color) { }
        public virtual void Clear(Color color) { }
        public virtual void DrawPixel(uint x, uint y, uint color) { }
        public virtual void DrawPixel(uint x, uint y, Color color) { }
        public virtual void DrawFilledRect(uint x, uint y, uint w, uint h, uint color) { }

        public void DrawFilledRect(uint x, uint y, uint w, uint h, Color color) { DrawFilledRect(x, y, w, h, color.Pack()); }

        public void DrawChar(uint x, uint y, char c, uint fg, uint bg, PCScreenFont font)
        {
            uint xx = x;
            for (uint j = 0; j < font.Height; j++)
            {
                uint glyph = font.Data[c * font.Height + j];
                for (uint i = 0; i < 8; i++)
                {
                    if ((glyph & 0x80) >= 1) { DrawPixel(xx, y, fg); }
                    else if (bg != 0x00FFFFFF) { DrawPixel(xx, y, bg); }
                    glyph <<= 1;
                    xx++;
                }
                y++;
                xx = x;
            }
        }

        public void DrawChar(uint x, uint y, char c, Color fg, Color bg, PCScreenFont font)
        {
            uint fp = fg.Pack(), bp = bg.Pack();
            uint xx = x;
            for (uint j = 0; j < font.Height; j++)
            {
                uint glyph = font.Data[c * font.Height + j];
                for (uint i = 0; i < 8; i++)
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

        public void DrawString(uint x, uint y, string txt, uint fg, uint bg, PCScreenFont font)
        {
            int i = 0;
            uint xx = x, yy = y, cw = (uint)font.GetWidth(), ch = (uint)font.GetHashCode();
            while (i < txt.Length)
            {
                if (txt[i] == '\n') { xx = x; yy += ch; }
                else { DrawChar(xx, yy, txt[i], fg, bg, font); xx += cw; }
                i++;
            }
        }

        public void DrawString(uint x, uint y, string txt, Color fg, Color bg, PCScreenFont font)
        {
            int i = 0;
            uint xx = x, yy = y, cw = (uint)font.GetWidth(), ch = (uint)font.GetHashCode();
            while (i < txt.Length)
            {
                if (txt[i] == '\n') { xx = x; yy += ch; }
                else { DrawChar(xx, yy, txt[i], fg, bg, font); xx += cw; }
                i++;
            }
        }
    }
}
