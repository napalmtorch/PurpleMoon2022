using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PurpleMoon.Core;
using PurpleMoon.Graphics;

namespace PurpleMoon.GUI
{
    public class Terminal : Container
    {
        public Point SizeInChars { get { return new Point(Buffer.Size.X / Assets.GetFont(Font).GetWidth(), Buffer.Size.Y / Assets.GetFont(Font).GetHeight()); } }
        public string Font { get; private set; }
        public Point Cursor;
        public Color BackColor;
        public Color ForeColor;

        public Terminal(int x, int y, int w, int h) : base(x, y, w, h, "Terminal", null)
        {
            Cursor = Point.Zero;
            BackColor = Color.Black;
            ForeColor = Color.White;
            Font = "Default";
            Invalidate();
            Draw();
        }

        public override void Update()
        {
            base.Update();
            _draw_base = false;
        }

        public override void Draw()
        {
            base.Draw();

            Kernel.WindowMgr.BackBuffer.Draw(Bounds, Buffer.Data);
        }

        public void Clear() { Clear(BackColor); }

        public void Clear(Color color)
        {
            Buffer.Clear(color);
            Cursor = Point.Zero;
            BackColor = color;
        }

        public void PutChar(int x, int y, char c, Color fg, Color bg)
        {
            if ((uint)x >= (uint)SizeInChars.X || (uint)y >= (uint)SizeInChars.Y) { return; }
            PCScreenFont font = Assets.GetFont("Default");
            Buffer.DrawChar(x * font.GetWidth(), y * font.GetHeight(), c, fg, bg, font);
        }

        public void Newline(int lines = 1)
        {
            while (lines-- > 0)
            {
                Cursor.X = 0;
                Cursor.Y++;
                if (Cursor.Y >= SizeInChars.Y) { Scroll(); }
            }
        }

        public void Backspace(int chars = 1)
        {
            while (chars-- > 0)
            {
                if (Cursor.X > 0)
                {

                }
                else if (Cursor.Y > 0)
                {

                }
            }
        }

        public void Scroll(int lines = 1)
        {
            PCScreenFont font = Assets.GetFont("Default");
            while (lines-- > 0)
            {
                uint line = (uint)(Buffer.Size.X * font.GetHeight() * 4);
                uint size = (uint)(Buffer.Size.X * Buffer.Size.Y * 4);
                MemUtil.Copy(Buffer.Data, 0, Buffer.Data, line, size - line);
                for (int i = 0; i < SizeInChars.X; i++) { PutChar(i, SizeInChars.Y - 1, ' ', ForeColor, BackColor); }
                Cursor = new Point(0, SizeInChars.Y - 1);
            }
        }

        public void Write(char c) { Write(c, ForeColor, BackColor); }

        public void Write(char c, Color fg) { Write(c, fg, BackColor); }

        public void Write(char c, Color fg, Color bg)
        {
            if (c == '\n') { Newline(); }
            else
            {
                PutChar(Cursor.X, Cursor.Y, c, fg, bg);
                Cursor.X++;
                if (Cursor.X >= SizeInChars.X) { Newline(); }
            }
        }

        public void Write(string str) { Write(str, ForeColor, BackColor); }

        public void Write(string str, Color fg) { Write(str, fg, BackColor); }

        public void Write(string str, Color fg, Color bg)
        {
            for (int i = 0; i < str.Length; i++) { Write(str[i], fg, bg); }
        }

        public void WriteLine(string str) { WriteLine(str, ForeColor, BackColor); }

        public void WriteLine(string str, Color fg) { WriteLine(str, fg, BackColor); }

        public void WriteLine(string str, Color fg, Color bg)
        {
            Write(str, fg, bg);
            Newline();
        }
    }
}
