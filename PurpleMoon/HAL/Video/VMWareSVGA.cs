// Large portion of used in this class has been borrowed from nifanfa
// https://github.com/nifanfa
// Re-written to better suit my needs - thank you tho!

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PurpleMoon.Core;
using Cosmos.Core;
using Cosmos.HAL;
using PurpleMoon.Graphics;

namespace PurpleMoon.HAL
{
    public unsafe class VMWareSVGA : VideoDriver
    {
        public MemoryBlock VRAM { get; private set; }
        public MemoryBlock FIFO { get; private set; }

        private PCIDevice _pcidev;
        private IOPort    _port_index;
        private IOPort    _port_val;
        private IOPort    _port_bios;
        private IOPort    _port_irq;
        private uint      _bpp, _cap;
        private uint      _framesz;
        private uint      _frame_offset;


        public VMWareSVGA() : base("VMWareSVGAII")
        {
            _pcidev       = null;
            _port_index   = null;
            _port_val     = null;
            _port_bios    = null;
            _port_irq     = null;
            _bpp          = 0;
            _cap          = 0;
            _framesz      = 0;
            _frame_offset = 0;

            VRAM = null;
            FIFO = null;
        }

        public override void Start()
        {
            base.Start();
            _pcidev = (Cosmos.HAL.PCI.GetDevice(Cosmos.HAL.VendorID.VMWare, Cosmos.HAL.DeviceID.SVGAIIAdapter));
            _pcidev.EnableMemory(true);
            uint basePort = _pcidev.BaseAddressBar[0].BaseAddress;
            _port_index   = new IOPort((ushort)(basePort + (uint)IOPortOffset.Index));
            _port_val     = new IOPort((ushort)(basePort + (uint)IOPortOffset.Value));
            _port_bios    = new IOPort((ushort)(basePort + (uint)IOPortOffset.Bios));
            _port_irq     = new IOPort((ushort)(basePort + (uint)IOPortOffset.IRQ));

            WriteRegister(Register.ID, (uint)ID.V2);
            if (ReadRegister(Register.ID) != (uint)ID.V2) { return; }

            VRAM = new MemoryBlock(ReadRegister(Register.FrameBufferStart), ReadRegister(Register.VRamSize));

            _cap = ReadRegister(Register.Capabilities);
            InitializeFIFO();
            SetMode((uint)Renderer.DefaultResolution.X, (uint)Renderer.DefaultResolution.Y, 32);
        }

        public override void Stop()
        {
            base.Stop();
            WriteRegister(Register.Enable, 0);
        }

        private void InitializeFIFO()
        {
            FIFO = new MemoryBlock(ReadRegister(Register.MemStart), ReadRegister(Register.MemSize));
            FIFO[(uint)FIFOValue.Min] = (uint)Register.FifoNumRegisters * sizeof(uint);
            FIFO[(uint)FIFOValue.Max] = FIFO.Size;
            FIFO[(uint)FIFOValue.NextCmd] = FIFO[(uint)FIFOValue.Min];
            FIFO[(uint)FIFOValue.Stop] = FIFO[(uint)FIFOValue.Min];
            WriteRegister(Register.ConfigDone, 1);
        }

        public void SetMode(uint w, uint h, uint bpp = 32)
        {
            _sz  = new Point((int)w, (int)h);
            _bpp = (bpp / 8);
            WriteRegister(Register.Width, w);
            WriteRegister(Register.Height, h);
            WriteRegister(Register.BitsPerPixel, bpp);
            WriteRegister(Register.Enable, 1);
            InitializeFIFO();

            _framesz = ReadRegister(Register.FrameBufferSize);
            _frame_offset = ReadRegister(Register.FrameBufferOffset);
        }

        public void Update(uint x, uint y, uint w, uint h)
        {
            WriteToFIFO((uint)FIFOCommand.Update);
            WriteToFIFO(x);
            WriteToFIFO(y);
            WriteToFIFO(w);
            WriteToFIFO(h);
            WaitForFIFO();
        }

        public override void Update()
        {
            base.Update();
            Update(0, 0, (uint)_sz.X, (uint)_sz.Y);
        }

        public override void Clear(uint color)
        {
            base.Clear(color);
            VRAM.Fill(0, _framesz, color);
        }

        public override void Clear(Color color)
        {
            base.Clear(color);
            VRAM.Fill(0, _framesz, color.Pack());
        }

        public void Copy(uint x, uint y, uint nx, uint ny, uint w, uint h)
        {
            WriteToFIFO((uint)FIFOCommand.RECT_COPY);
            WriteToFIFO(x);
            WriteToFIFO(y);
            WriteToFIFO(ny);
            WriteToFIFO(nx);
            WriteToFIFO(w);
            WriteToFIFO(h);
            WaitForFIFO();
        }

        public override void Swap(uint[] data)
        {
            base.Swap(data);
            fixed (uint* ptr = &data[0]) { Cosmos.Core.MemoryOperations.Copy((uint*)VRAM.Base, ptr, _sz.X * _sz.Y); }
            Update(0, 0, (uint)_sz.X, (uint)_sz.Y);
        }

        public override void DrawFilledRect(uint x, uint y, uint w, uint h, uint color)
        {
            if ((_cap & (uint)Capability.RectFill) != 0)
            {
                WriteToFIFO((uint)FIFOCommand.RECT_FILL);
                WriteToFIFO(color);
                WriteToFIFO(x);
                WriteToFIFO(y);
                WriteToFIFO(w);
                WriteToFIFO(h);
                WaitForFIFO();
            }
            else
            {
                if ((_cap & (uint)Capability.RectCopy) != 0)
                {
                    uint xTarget = (x + w);
                    uint yTarget = (y + h);
                    for (uint xTmp = x; xTmp < xTarget; xTmp++) { DrawPixel(xTmp, y, color); }

                    Update(x, y, w, 1);
                    for (uint yTmp = y + 1; yTmp < yTarget; yTmp++) { Copy(x, y, x, yTmp, w, 1); }
                }
                else
                {
                    uint xTarget = (x + h);
                    uint yTarget = (y + h);
                    for (uint xTmp = x; xTmp < xTarget; xTmp++) { for (uint yTmp = y; yTmp < yTarget; yTmp++) { DrawPixel(xTmp, yTmp, color); } }
                    Update(x, y, w, h);
                }
            }
        }

        public override void DrawPixel(uint x, uint y, uint color)
        {
            base.DrawPixel(x, y, color);
            if (x >= (uint)_sz.X || y >= (uint)_sz.Y) { return; }
            VRAM[(uint)((y * _sz.X + x) * _bpp)] = color;
        }

        public override void DrawPixel(uint x, uint y, Color color)
        {
            base.DrawPixel(x, y, color);
            if (x >= (uint)_sz.X || y >= (uint)_sz.Y) { return; }
            VRAM[(uint)((y * _sz.X + x) * _bpp)] = color.Pack();
        }

        public void WriteRegister(Register register, uint value)
        {
            _port_index.DWord = (uint)register;
            _port_val.DWord = value;
        }

        public uint ReadRegister(Register register)
        {
            _port_index.DWord = (uint)register;
            return _port_val.DWord;
        }

        private uint GetFIFO(FIFOValue cmd) { return FIFO[(uint)cmd]; }

        private uint SetFIFO(FIFOValue cmd, uint value) { return FIFO[(uint)cmd] = value; }

        private void WriteToFIFO(uint value)
        {
            if (((GetFIFO(FIFOValue.NextCmd) == GetFIFO(FIFOValue.Max) - 4) && GetFIFO(FIFOValue.Stop) == GetFIFO(FIFOValue.Min)) ||
                (GetFIFO(FIFOValue.NextCmd) + 4 == GetFIFO(FIFOValue.Stop)))
                WaitForFIFO();

            SetFIFO((FIFOValue)GetFIFO(FIFOValue.NextCmd), value);
            SetFIFO(FIFOValue.NextCmd, GetFIFO(FIFOValue.NextCmd) + 4);

            if (GetFIFO(FIFOValue.NextCmd) == GetFIFO(FIFOValue.Max))
                SetFIFO(FIFOValue.NextCmd, GetFIFO(FIFOValue.Min));
        }

        private void WaitForFIFO()
        {
            WriteRegister(Register.Sync, 1);
            while (ReadRegister(Register.Busy) != 0) { }
        }

        public enum Register : ushort
        {
            ID = 0,
            Enable = 1,
            Width = 2,
            Height = 3,
            MaxWidth = 4,
            MaxHeight = 5,
            Depth = 6,
            BitsPerPixel = 7,
            PseudoColor = 8,
            RedMask = 9,
            GreenMask = 10,
            BlueMask = 11,
            BytesPerLine = 12,
            FrameBufferStart = 13,
            FrameBufferOffset = 14,
            VRamSize = 15,
            FrameBufferSize = 16,
            Capabilities = 17,
            MemStart = 18,
            MemSize = 19,
            ConfigDone = 20,
            Sync = 21,
            Busy = 22,
            GuestID = 23,
            CursorID = 24,
            CursorX = 25,
            CursorY = 26,
            CursorOn = 27,
            HostBitsPerPixel = 28,
            ScratchSize = 29,
            MemRegs = 30,
            NumDisplays = 31,
            PitchLock = 32,
            FifoNumRegisters = 293
        }

        private enum ID : uint
        {
            Magic = 0x900000,
            V0 = Magic << 8,
            V1 = (Magic << 8) | 1,
            V2 = (Magic << 8) | 2,
            Invalid = 0xFFFFFFFF
        }

        public enum FIFOValue : uint
        {               
            Min = 0,
            Max = 4,
            NextCmd = 8,
            Stop = 12
        }

        private enum FIFOCommand
        {
            Update = 1,
            RECT_FILL = 2,
            RECT_COPY = 3,
            DEFINE_BITMAP = 4,
            DEFINE_BITMAP_SCANLINE = 5,
            DEFINE_PIXMAP = 6,
            DEFINE_PIXMAP_SCANLINE = 7,
            RECT_BITMAP_FILL = 8,
            RECT_PIXMAP_FILL = 9,
            RECT_BITMAP_COPY = 10,
            RECT_PIXMAP_COPY = 11,
            FREE_OBJECT = 12,
            RECT_ROP_FILL = 13,
            RECT_ROP_COPY = 14,
            RECT_ROP_BITMAP_FILL = 15,
            RECT_ROP_PIXMAP_FILL = 16,
            RECT_ROP_BITMAP_COPY = 17,
            RECT_ROP_PIXMAP_COPY = 18,
            DEFINE_CURSOR = 19,
            DISPLAY_CURSOR = 20,
            MOVE_CURSOR = 21,
            DEFINE_ALPHA_CURSOR = 22
        }

        private enum IOPortOffset : byte
        {
            Index = 0,
            Value = 1,
            Bios = 2,
            IRQ = 3
        }

        [Flags]
        private enum Capability
        {
            None = 0,
            RectFill = 1,
            RectCopy = 2,
            RectPatFill = 4,
            LecacyOffscreen = 8,
            RasterOp = 16,
            Cursor = 32,
            CursorByPass = 64,
            CursorByPass2 = 128,
            EigthBitEmulation = 256,
            AlphaCursor = 512,
            Glyph = 1024,
            GlyphClipping = 0x00000800,
            Offscreen1 = 0x00001000,
            AlphaBlend = 0x00002000,
            ThreeD = 0x00004000,
            ExtendedFifo = 0x00008000,
            MultiMon = 0x00010000,
            PitchLock = 0x00020000,
            IrqMask = 0x00040000,
            DisplayTopology = 0x00080000,
            Gmr = 0x00100000,
            Traces = 0x00200000,
            Gmr2 = 0x00400000,
            ScreenObject2 = 0x00800000
        }
    }
}
