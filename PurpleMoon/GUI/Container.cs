using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PurpleMoon.Core;
using PurpleMoon.Graphics;

namespace PurpleMoon.GUI
{
    public class Container : Control
    {
        public List<Control> Controls;
        public Image         Buffer;

        protected bool _draw_base;

        public Container(int x, int y, int w, int h, string name, Container parent = null) : base(x, y, w, h, name, ControlType.Container, parent)
        {
            this.Controls   = new List<Control>();
            this.Buffer     = new Image(w, h);
            this._draw_base = true;
            Invalidate();
            Draw();
        }

        public Container(int x, int y, int w, int h, string name, ControlType type, Container parent) : this(x, y, w, h, name, parent)
        {
            this.Type       = type;
            this.Controls   = new List<Control>();
            this.Buffer     = new Image(w, h);
            this._draw_base = true;
        }

        public override void Update()
        {
            base.Update();

            for (int i = 0; i < Controls.Count; i++)
            {
                Controls[i].Update();
            }
       
        }

        public override void Draw()
        {
            base.Draw();

            if (_draw_base)
            {
                if (Flags.Invalidated)
                {
                    Buffer.Clear(Theme.GetColor(ColorIndex.Background));
                    if (Theme.Border == BorderStyle.Fixed3D) { Buffer.DrawRectPopup(0, 0, Bounds.W, Bounds.H, Theme.GetColor(ColorIndex.BorderTopLeft), Theme.GetColor(ColorIndex.BorderBottomRight), Theme.GetColor(ColorIndex.BorderInner)); }

                    for (int i = 0; i < Controls.Count; i++)
                    {
                        Controls[i].Draw();
                    }

                    Flags.Invalidated = false;
                }
                Kernel.WindowMgr.BackBuffer.Draw(Bounds, Buffer.Data);
            }
        }

        public void AddControl(Control control)
        {
            Controls.Add(control);
            Invalidate();
        }
    }
}
