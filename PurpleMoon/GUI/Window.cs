using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PurpleMoon.Core;
using PurpleMoon.Graphics;

namespace PurpleMoon.GUI
{
    public class Window : Container
    {
        public Button BtnClose  { get; private set; }
        public Button BtnResize { get; private set; }
        public Button BtnMin    { get; private set; }

        public Window(int x, int y, int w, int h, string title) : base(x, y, w, h, title, ControlType.Window, null)
        {
            BtnClose  = new Button(w - 21,                  4, 16, 14, "BtnClose",  this) { Text = "" };
            BtnResize = new Button(BtnClose.Bounds.X  - 18, 4, 16, 14, "BtnResize", this) { Text = "" };
            BtnMin    = new Button(BtnResize.Bounds.X - 18, 4, 16, 14, "BtnMin",    this) { Text = "" };

            _draw_base = false;
            Invalidate();
            Update();
            Draw();
        }

        public override void Update()
        {
            base.Update();
            if (BtnClose  != null) { BtnClose.Position  = new Point(Bounds.W - 21, 4);            BtnClose.Update(); }
            if (BtnResize != null) { BtnResize.Position = new Point(BtnClose.Bounds.X  - 18, 4); BtnResize.Update(); }
            if (BtnMin    != null) { BtnMin.Position    = new Point(BtnResize.Bounds.X - 18, 4); BtnMin.Update(); }
            _draw_base = false;
        }

        public override void Draw()
        {
            base.Draw();

            if (Flags.Invalidated)
            {
                Buffer.Clear(Theme.GetColor(ColorIndex.Background));
                if (Theme.Border == BorderStyle.Fixed3D) { Buffer.DrawRectPopup(0, 0, Bounds.W, Bounds.H, Theme.GetColor(ColorIndex.BorderTopLeft), Theme.GetColor(ColorIndex.BorderBottomRight), Theme.GetColor(ColorIndex.BorderInner)); }
                Buffer.DrawFilledRect(2, 2, Bounds.W - 5, 18, Theme.GetColor(Flags.Active ? ColorIndex.TitleBar : ColorIndex.TitleBarInactive));
                if (Name != null) { Buffer.DrawString(6, 2, Name, Theme.GetColor(Flags.Active ? ColorIndex.TitleText : ColorIndex.TitleTextInactive), Color.Transparent, Assets.GetFont(Theme.Font)); }
                if (BtnClose  != null) { BtnClose.Flags.Invalidated  = true; BtnClose.Draw(); }
                if (BtnResize != null) { BtnResize.Flags.Invalidated = true; BtnResize.Draw(); }
                if (BtnMin    != null) { BtnMin.Flags.Invalidated    = true; BtnMin.Draw(); }

                for (int i = 0; i < Controls.Count; i++)
                {
                    Controls[i].Flags.Invalidated = true;
                    Controls[i].Draw();
                }

                Flags.Invalidated = false;
            }
            Kernel.WindowMgr.BackBuffer.Draw(Bounds, Buffer.Data);
        }
    }
}
