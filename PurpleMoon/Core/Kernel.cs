using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Cosmos.System.FileSystem;
using PurpleMoon.Graphics;
using PurpleMoon.HAL;
using PurpleMoon.Multitasking;
using PurpleMoon.Services;

namespace PurpleMoon.Core
{
    public class Kernel : Cosmos.System.Kernel
    {
        // services
        public static DriverManager DriverMgr;
        public static WindowManager WindowMgr;

        public static int GCollectCount { get; private set; } = 0;

        private static float _gctime;

        protected override void BeforeRun()
        {
            Console.Clear();
            Debug.Info("Booting PurpleMoon...");

            FileSystem.Initialize();
            ProcessManager.Initialize();
            ProcessManager.Load(DriverMgr = new DriverManager(), true);

            Renderer.Initialize();
            Assets.Initialize();

            WindowMgr = new WindowManager();
            ProcessManager.Load(WindowMgr, true);
        }

        protected override void Run()
        {
            Debug.OK("Entered kernel main");

            while (true)
            {
                try
                {
                    ProcessManager.Schedule();

                    _gctime += WindowMgr.Delta;
                    if (_gctime >= 0.05f)
                    {
                        _gctime = 0;
                        GCollectCount += Cosmos.Core.Memory.Heap.Collect();
                    }
                }
                catch (Exception ex) { Debug.Panic(ex.Message); }
            }
        }
    }
}
