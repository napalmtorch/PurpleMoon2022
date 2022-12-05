using PurpleMoon.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurpleMoon.HAL
{
    public class PIT : Driver
    {
        public const ushort DefaultFrequency = 1100;

        public int   Frequency         { get; private set; }
        public ulong Ticks             { get; private set; }
        public ulong TPS               { get; private set; }
        public long  Milliseconds      { get; private set; }
        public long  TotalMilliseconds { get; private set; }
        public float TotalSeconds      { get { return (float)TotalMilliseconds / 1000.0f; } }

        private Cosmos.Core.IOGroup.PIT _ports;
        private ulong                   _ticks, _timer;

        public PIT() : base("PIT")
        {
            Cosmos.Core.INTs.SetIrqHandler(0x00, null);
            Cosmos.HAL.Global.PIT = null;

            Frequency         = 0;
            Ticks             = 0;
            Milliseconds      = 0;
            TotalMilliseconds = 0;
            _ticks            = 0;
            _ports            = new Cosmos.Core.IOGroup.PIT();
        }

        public override void Start() { SetFrequency(DefaultFrequency); }

        public override void Stop()
        {
            Cosmos.Core.CPU.DisableInterrupts();
            Cosmos.Core.INTs.SetIrqHandler(0x00, null);
        }

        public override void Update()
        {
            base.Update();
        }

        private void OnTick(ref Cosmos.Core.INTs.IRQContext context)
        {
            Ticks++;
            _ticks++;
            _timer++;

            ulong factor = (ulong)Frequency / 1000L;
            if (_timer % factor == 0)
            {
                Milliseconds      += 1;
                TotalMilliseconds += 1;
            }

            if (Milliseconds >= 1000)
            {
                TPS          = _ticks;
                Milliseconds = 0;
                _ticks       = 0;
            }
        }

        public void SetFrequency(int freq_hz)
        {
            Cosmos.Core.CPU.DisableInterrupts();
            Cosmos.Core.INTs.SetIrqHandler(0x00, null);
            ushort f = (ushort)(1193180 / freq_hz);
            byte   h = (byte)((f >> 8));
            byte   l = (byte)((f & 0xFF));

            _ports.Command.Byte = 0x36;
            _ports.Data0.Byte   = l;
            _ports.Data0.Byte   = h;
            Cosmos.Core.INTs.SetIrqHandler(0x00, OnTick);
            Cosmos.Core.CPU.EnableInterrupts();
            Frequency = freq_hz;
        }
    }
}
