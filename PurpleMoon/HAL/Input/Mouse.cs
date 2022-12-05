using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cosmos.System.Graphics;
using PurpleMoon.Core;
using PurpleMoon.Graphics;

namespace PurpleMoon.HAL
{
    public enum MouseButton : byte
    {
        None    = 0,
        Left    = 1,
        Right   = 2,
        Middle  = 4,
        Forth   = 8,
        Fifth   = 16,
    }

    public class Mouse : Driver
    {
        public Point Position { get; private set; }

        private Point _lastpos;

        public Mouse() : base("Mouse")
        {

        }

        public override void Start()
        {
            Position = Point.Zero;
        }

        public override void Stop()
        {

        }

        public override void Update()
        {
            base.Update();

            if (!IsMouseBoundsValid()) { UpdateBounds(); }

            Position = new Point((int)Cosmos.System.MouseManager.X, (int)Cosmos.System.MouseManager.Y);
            if (!_lastpos.Equals(Position)) 
            { 
                _lastpos = Position;
            }
        }

        public void Draw()
        {
            Kernel.WindowMgr.BackBuffer.Draw(Position.X, Position.Y, Assets.DefaultCursorSize.X, Assets.DefaultCursorSize.Y, Assets.DefaultCursor);
        }

        public void SetPosition(int x, int y)
        {
            Cosmos.System.MouseManager.X = (uint)x;
            Cosmos.System.MouseManager.Y = (uint)y;
        }

        private void UpdateBounds()
        {
            Cosmos.System.MouseManager.ScreenWidth  = (uint)Renderer.GetSize().X;
            Cosmos.System.MouseManager.ScreenHeight = (uint)Renderer.GetSize().Y;
        }

        private bool IsMouseBoundsValid()
        {
            if (Cosmos.System.MouseManager.ScreenWidth  != Renderer.GetSize().X) { return false; }
            if (Cosmos.System.MouseManager.ScreenHeight != Renderer.GetSize().Y) { return false; }
            return true;
        }

        public bool IsPressed(MouseButton btn) { return Cosmos.System.MouseManager.MouseState == (Cosmos.System.MouseState)btn; }
        
        public bool IsReleased(MouseButton btn) { return !IsPressed(btn); }
    }
}
