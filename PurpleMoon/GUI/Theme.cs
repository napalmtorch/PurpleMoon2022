using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PurpleMoon.Core;
using PurpleMoon.Graphics;
using PurpleMoon.HAL;

namespace PurpleMoon.GUI
{
    public enum BorderStyle : byte
    {
        None,
        FixedSingle,
        Fixed3D,
    }

    public enum ColorIndex : byte
    {
        Background,
        BackgroundHover,
        BackgroundDown,
        BackgroundDisabled,
        Text,
        TextHover,
        TextDown,
        TextDisabled,
        Border,
        BorderHover,
        BorderDown,
        BorderDisabled,
        BorderTopLeft     = Border,
        BorderBottomRight = BorderHover,
        BorderInner       = BorderDown,
    }

    public class Theme
    {
        public string      Name { get; private set; }
        public Color[]     Colors;
        public BorderStyle Border; 
        public int         BorderSize;
        public string      Font;

        public Theme(string name, BorderStyle border_style, int border_sz, string font, params Color[] colors)
        {
            this.Name       = name;
            this.Border     = border_style;
            this.BorderSize = border_sz;
            this.Font       = font;
            this.Colors     = new Color[32];
            for (int i = 0; i < 32; i++) { if (i < colors.Length) { this.Colors[i] = colors[i]; } }
        }

        public Color GetColor(ColorIndex index) { return Colors[(int)index]; }

        public static Theme DefaultGeneric = new Theme("Default", BorderStyle.Fixed3D, 1, "Default",
                                                       new Color(0xFF, 0xD4, 0xD0, 0xC8), new Color(0xFF, 0xE4, 0xE0, 0xD8), new Color(0xFF, 0xC4, 0xC0, 0xB8), new Color(0xFF, 0xD4, 0xD0, 0xC8),
                                                       new Color(0xFF, 0x00, 0x00, 0x00), new Color(0xFF, 0x00, 0x00, 0x00), new Color(0xFF, 0x00, 0x00, 0x00), new Color(0xFF, 0x80, 0x80, 0x80),
                                                       new Color(0xFF, 0xFF, 0xFF, 0xFF), new Color(0xFF, 0x40, 0x40, 0x40), new Color(0xFF, 0x80, 0x80, 0x80), new Color(0xFF, 0xFF, 0xFF, 0xFF));
    }
}
