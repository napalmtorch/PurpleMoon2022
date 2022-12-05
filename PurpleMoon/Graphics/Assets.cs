using Cosmos.System.Graphics.Fonts;
using PurpleMoon.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurpleMoon.Graphics
{
    public static class Assets
    {
        private static List<string>       _font_ids;
        private static List<PCScreenFont> _fonts;

        private static List<string> _image_ids;
        private static List<Image>  _images;

        public static void Initialize()
        {
            _font_ids = new List<string>();
            _fonts    = new List<PCScreenFont>();

            _image_ids = new List<string>();
            _images    = new List<Image>();

            LoadFont("Default", new PCScreenFont("0:\\sys\\res\\fonts\\font16.psf", Point.Zero));
            LoadImage("BG", new Image("0:\\sys\\res\\wallpapers\\moon.bmp"));
            GetImage("BG").Resize(Renderer.GetSize().X, Renderer.GetSize().Y);
        }

        public static void LoadFont(string id, PCScreenFont font)
        {
            if (FontExists(id)) { Debug.Panic("Font with id '%s' already exists", id); return; }
            _font_ids.Add(id);
            _fonts.Add(font);
            Debug.Info("Registered font - Size:%dx%d Spacing:%dx%d ID:%s", font.GetWidth(false), font.GetHeight(false), font.Spacing.X, font.Spacing.Y, id);
        }

        public static void LoadImage(string id, Image img)
        {
            if (ImageExists(id)) { Debug.Panic("Image with id '%s' already exists", id); return; }
            _image_ids.Add(id);
            _images.Add(img);
            Debug.Info("Registered image - Size:%dx%d ID:%s", img.Size.X, img.Size.Y, id);
        }

        public static PCScreenFont GetFont(string id)
        {
            for (int i = 0; i < _font_ids.Count; i++)
            {
                if (_font_ids[i] == id) { return _fonts[i]; }
            }
            Debug.Panic("No font with id '%s' exists", id);
            return null;
        }

        public static Image GetImage(string id)
        {
            for (int i = 0; i < _image_ids.Count; i++)
            {
                if (_image_ids[i] == id) { return _images[i]; }
            }
            Debug.Panic("No image with id '%s' exists", id);
            return null;
        }

        public static bool FontExists(string id)
        {
            for (int i = 0; i < _font_ids.Count; i++) { if (_font_ids[i] == id) { return true; } }
            return false;
        }
        public static bool ImageExists(string id)
        {
            for (int i = 0; i < _image_ids.Count;i++) { if (_image_ids[i] == id) { return true; } }
            return false;
        }

        public static readonly Point DefaultCursorSize = new Point(12, 20);
        public static readonly Color[] DefaultCursor = new Color[12 * 20]
        {
            Color.Black, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent,
            Color.Black, Color.Black, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent,
            Color.Black, Color.White, Color.Black, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent,
            Color.Black, Color.White, Color.White, Color.Black, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent,
            Color.Black, Color.White, Color.White, Color.White, Color.Black, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent,
            Color.Black, Color.White, Color.White, Color.White, Color.White, Color.Black, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent,
            Color.Black, Color.White, Color.White, Color.White, Color.White, Color.White, Color.Black, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent,
            Color.Black, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.Black, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent,
            Color.Black, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.Black, Color.Transparent, Color.Transparent, Color.Transparent,
            Color.Black, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.Black, Color.Transparent, Color.Transparent,
            Color.Black, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.Black, Color.Transparent,
            Color.Black, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.Black,
            Color.Black, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.Black, Color.Black, Color.Black, Color.Black, Color.Black,
            Color.Black, Color.White, Color.White, Color.White, Color.Black, Color.White, Color.White, Color.Black, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent,
            Color.Black, Color.White, Color.White, Color.Black, Color.Transparent, Color.Black, Color.White, Color.White, Color.Black, Color.Transparent, Color.Transparent, Color.Transparent,
            Color.Black, Color.White, Color.Black, Color.Transparent, Color.Transparent, Color.Black, Color.White, Color.White, Color.Black, Color.Transparent, Color.Transparent, Color.Transparent,
            Color.Black, Color.Black, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Black, Color.White, Color.White, Color.Black, Color.Transparent, Color.Transparent,
            Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Black, Color.White, Color.White, Color.Black, Color.Transparent, Color.Transparent,
            Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Black, Color.Black, Color.Transparent, Color.Transparent, Color.Transparent,
            Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent,
        };
    }
}
