using PurpleMoon.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurpleMoon.Multitasking
{
    public static class ProcessManager
    {
        public static List<Process> Processes { get; private set; }

        private static uint _id;
        private static int  _active;

        public static void Initialize()
        {
            _id = 0;
            Processes = new List<Process>();

            Debug.OK("Initialized process manager");
        }

        public static void Schedule()
        {
            if (_active < 0 || _active >= Processes.Count) { return; }

            Process now = Processes[_active];
            if (now.Running) { now.Main(); }
            if (now.Done) { Processes.RemoveAt(_active); }

            _active++;
            if (_active >= Processes.Count) { _active = 0; }
        }

        public static void Load(Process proc, bool start = false)
        {
            Processes.Add(proc);
            proc.Running = false;
            proc.Done    = false;
            Debug.Info("Loaded process - ID:%p Name:%s", proc.GetID(), proc.GetName());
            if (start) { proc.Start(); }
        }

        public static void Unload(Process proc)
        {
            Processes.Remove(proc);
        }

        public static uint GenerateID() { return _id++; }
    }
}
