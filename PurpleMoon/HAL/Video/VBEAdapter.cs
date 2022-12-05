using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cosmos.HAL;
using PurpleMoon.Core;
using PurpleMoon.Graphics;

namespace PurpleMoon.HAL
{
    public unsafe class VBEAdapter : VideoDriver
    {
        private uint                         _bpp;
        private uint*                        _base;

        public VBEAdapter() : base("VBEAdapter")
        {
            
        }

        public override void Start()
        {
            base.Start();
            SetMode((uint)Renderer.DefaultResolution.X, (uint)Renderer.DefaultResolution.Y, 32);
        }

        public override void Stop()
        {
            base.Stop();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Swap(uint[] data)
        {
            base.Swap(data);
            fixed (uint* ptr = &data[0]) { Cosmos.Core.MemoryOperations.Copy(_base, ptr, _sz.X * _sz.Y); }
        }

        public void SetMode(uint w, uint h, int bpp = 32)
        {
            _sz   = new Point(Cosmos.Core.VBE.getModeInfo().width, Cosmos.Core.VBE.getModeInfo().height);
            _bpp  = Cosmos.Core.VBE.getModeInfo().bpp;
            _base = (uint*)Cosmos.Core.VBE.getLfbOffset();
        }

        public override void Clear(uint color) { base.Clear(color); Cosmos.Core.MemoryOperations.Fill(_base, color, _sz.X * _sz.Y); }
      
        public override void Clear(Color color) { base.Clear(color); Cosmos.Core.MemoryOperations.Fill(_base, color.Pack(), _sz.X * _sz.Y); }

        public override void DrawPixel(uint x, uint y, uint color)
        {
            base.DrawPixel(x, y, color);
            if (x >= (uint)_sz.X || y >= (uint)_sz.Y) { return; }
            _base[y * _sz.X + x] = color;
        }

        public override void DrawPixel(uint x, uint y, Color color)
        {
            base.DrawPixel(x, y, color);
            if (x >= (uint)_sz.X || y >= (uint)_sz.Y) { return; }
            _base[y * _sz.X + x] = color.Pack();
        }

        public override void DrawFilledRect(uint x, uint y, uint w, uint h, uint color)
        {
            base.DrawFilledRect(x, y, w, h, color);
            for (uint i = 0; i < w * h; i++) { DrawPixel(x + (i % w), y + (i / w), color); }
        }
    }
}
