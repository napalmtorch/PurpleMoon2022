using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PurpleMoon.Core;
using PurpleMoon.HAL;

namespace PurpleMoon.Graphics
{
    public static class Renderer
    {
        public static Point       DefaultResolution { get; private set; } = new Point(800, 600);
        public static VideoDriver Device            { get; private set; }  
        public static bool        Enabled;

        public static void Initialize()
        {
            Enabled = false;
            Device = (VideoDriver)Kernel.DriverMgr.Fetch("VMWareSVGAII");
            if (Device == null) { Device = (VideoDriver)Kernel.DriverMgr.Fetch("VBEAdapter"); }
            if (Device == null) { Debug.Panic("Failed to locate driver for graphical video device"); return; }

            Enabled = true;
            Debug.OK("Initialized graphical rendering pipeline");
        }

        public static void Disable()
        {
            if (Device != null) { Device.Stop(); }
        }

        public static void Update()
        {
            if (Device == null) { return; }
            Device.Update();
        }

        public static void Clear(Color color)
        {
            if (Device == null) { return; }
            Device.Clear(color);
        }

        public static void DrawPixel(int x, int y, Color color)
        {
            if (Device == null) { return; }
            Device.DrawPixel((uint)x, (uint)y, color);
        }

        public static void DrawChar(int x, int y, char c, Color fg, Color bg, PCScreenFont font)
        {
            if (Device == null) { return; }
            Device.DrawChar((uint)x, (uint)y, c, fg, bg, font);
        }

        public static void DrawString(int x, int y, string str, Color fg, Color bg, PCScreenFont font)
        {
            if (Device == null) { return; }
            Device.DrawString((uint)x, (uint)y, str, fg, bg, font);
        }

        public static void Swap(Image image)
        {
            if (Device == null) { return; }
            Device.Swap(image.Data);
        }

        public static Point GetSize() { return Device.Size; }
    }
}
