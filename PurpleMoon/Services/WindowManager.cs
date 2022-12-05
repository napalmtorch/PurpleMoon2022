using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PurpleMoon.Core;
using PurpleMoon.HAL;
using PurpleMoon.Graphics;
using PurpleMoon.Multitasking;
using PurpleMoon.GUI;

namespace PurpleMoon.Services
{
    public class WindowManager : Process
    {
        public Image BackBuffer;
        public Color BackColor;
        public Color ForeColor;
        public float Delta { get { return _delta; } }
        public string DebugText;

        private int   _fps, _frames, _last, _tps, _ticks;
        private float _timer, _delta, _tl, _tn;

        Window window;
        Button btn;

        public WindowManager() : base("winmgr")
        {
            BackBuffer = new Image(Renderer.GetSize().X, Renderer.GetSize().Y);
            BackColor = new Color(0xFF, 0x3A, 0x6E, 0xA5);
            ForeColor = new Color(0xFF, 0xFF, 0xFF, 0xFF);
        }
        
        public override void Start()
        {
            base.Start();

            window = new Window(150, 50, 320, 240, "Demo Window");
            btn = new Button(32, 32, "Button", window);
            window.AddControl(btn);
        }

        public override void Stop()
        {
            base.Stop();
        }

        public override void Main()
        {
            _ticks++;
            if (_last != DateTime.Now.Second)
            {
                _last   = DateTime.Now.Second;
                _fps    = _frames;
                _tps    = _ticks;
                _frames = 0;
                _ticks  = 0;
            }

            PIT pit = (PIT)Kernel.DriverMgr.Fetch("PIT");
            _tl     = _tn;
            _tn     = pit.TotalSeconds;
            _delta  = (float)(_tn - _tl);
            _timer += _delta;

            if (_timer >= 0.0166667f)
            {
                _timer = 0;
                _frames++;

                if (Assets.ImageExists("BG")) { BackBuffer.Swap(Assets.GetImage("BG")); }
                else { BackBuffer.Clear(BackColor); }

                window.Update();
                window.Draw();

                int y = 0;
                BackBuffer.DrawString(0,  y, "FPS   : " + _fps, Color.White, Color.Transparent, Assets.GetFont("Default"));
                BackBuffer.DrawString(96, y, "TPS   : " + _tps, Color.White, Color.Transparent, Assets.GetFont("Default")); y += 16;
                BackBuffer.DrawString(0,  y, "DELTA : " + _delta.ToString(), Color.White, Color.Transparent, Assets.GetFont("Default")); y += 16;
                BackBuffer.DrawString(0,  y, "TIME  : " + pit.TotalSeconds.ToString(), Color.White, Color.Transparent, Assets.GetFont("Default")); y += 16;
                BackBuffer.DrawString(0,  y, "VID   : " + Renderer.Device.GetName(), Color.White, Color.Transparent, Assets.GetFont("Default")); y += 16;
                BackBuffer.DrawString(0,  y, "RAM   : " + (Cosmos.Core.GCImplementation.GetUsedRAM() / 1048576) + "/" + Cosmos.Core.GCImplementation.GetAvailableRAM() + "MB", Color.White, Color.Transparent, Assets.GetFont("Default")); y += 16;
                BackBuffer.DrawString(0,  y, "GC'D  : " + Kernel.GCollectCount.ToString(), Color.White, Color.Transparent, Assets.GetFont("Default")); y += 16;
                BackBuffer.DrawString(0,  y, DebugText, Color.White, Color.Transparent, Assets.GetFont("Default")); y += 16;

                Mouse ms = (Mouse)Kernel.DriverMgr.Fetch("Mouse");
                if (ms != null) { ms.Draw(); }

                Renderer.Swap(BackBuffer);
            }
        }

    }
}
