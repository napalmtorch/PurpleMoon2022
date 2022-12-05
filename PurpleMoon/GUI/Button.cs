using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PurpleMoon.Core;
using PurpleMoon.Graphics;
using PurpleMoon.Services;

namespace PurpleMoon.GUI
{
    public class Button : Control
    {
        public Button(int x, int y, int w, int h, string name, Container parent = null) : base(x, y, w, h, name, ControlType.Button, parent)
        {
            Invalidate();
            Update();
            Draw();
        }

        public Button(int x, int y, string name, Container parent = null) : this(x, y, 75, 23, name, parent) { }

        public override void Update()
        {
            base.Update();
        }

        public override void Draw()
        {
            base.Draw();

            if (Flags.Invalidated)
            {
                Image img = (Parent != null ? Parent.Buffer : Kernel.WindowMgr.BackBuffer);
                Point pos = new Point(Bounds.X, Bounds.Y);

                ColorIndex bg = ColorIndex.Background, fg = ColorIndex.Text;
                if (Flags.Hover) { bg = ColorIndex.BackgroundHover; fg = ColorIndex.TextHover; }
                if (Flags.Down)  { bg = ColorIndex.BackgroundDown; fg = ColorIndex.TextDown; }

                img.DrawFilledRect(pos.X, pos.Y, Bounds.W, Bounds.H, Theme.GetColor(bg));
                if (Theme.Border == BorderStyle.Fixed3D) { img.DrawRectPopup(pos.X, pos.Y, Bounds.W, Bounds.H, Theme.GetColor(ColorIndex.BorderTopLeft), Theme.GetColor(ColorIndex.BorderBottomRight), Theme.GetColor(ColorIndex.BorderInner)); }
                if (Text != null)
                {
                    PCScreenFont font = Assets.GetFont(Theme.Font);
                    img.DrawString(pos.X + (Bounds.W / 2) - (font.GetWidth(Text) / 2), pos.Y + (Bounds.H / 2) - (font.GetHeight() / 2), Text, Theme.GetColor(fg), Color.Transparent, font);
                }

                Flags.Invalidated = false;
            }
        }
    }
}
