using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cosmos.HAL.BlockDevice.Registers;
using PurpleMoon.Core;
using PurpleMoon.Graphics;

namespace PurpleMoon.GUI
{
    public enum ControlType : byte
    {
        Control,
        Container,
        Window,
        Button,
        Label,
        CheckBox,
        TextBox,
        HorizontalScrollBar,
        VerticalScrollBar,
    }

    public struct ControlFlags
    {
        public bool Hover;
        public bool Down;
        public bool Up;
        public bool Clicked;
        public bool Active;
        public bool Enabled;
        public bool Visible;
        public bool Invalidated;

        public static ControlFlags Defaults { get; private set; } = new ControlFlags
        {
            Hover       = false,
            Down        = false,
            Up          = false,
            Clicked     = false,
            Active      = true,
            Enabled     = true,
            Visible     = true,
            Invalidated = true,
        };
    }

    public class Control
    {
        public Point Position { get { return Bounds.Position; } set { Bounds.Position = value; } }
        public Point Size     { get { return Bounds.Size; }     set { Bounds.Size = value; } }

        public Container    Parent;
        public ControlType  Type;
        public ControlFlags Flags;
        public Rectangle    Bounds;
        public Theme        Theme;
        public string       Name, Text;

        public Control(int x, int y, int w, int h, string name, ControlType type, Container parent = null)
        {
            this.Type   = type;
            this.Bounds = new Rectangle(x, y, w, h);
            this.Parent = parent;
            this.Flags  = ControlFlags.Defaults;
            this.Theme  = Theme.DefaultGeneric;
            this.Name   = name;
            this.Text   = name;
        }

        public virtual void Update()
        {

        }

        public virtual void Draw()
        {

        }        

        public void Invalidate()
        {
            Flags.Invalidated = true;
        }
    }
}
