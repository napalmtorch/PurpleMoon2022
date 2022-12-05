using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PurpleMoon.Core;

namespace PurpleMoon.Multitasking
{
    public abstract class Process
    {
        public bool Running;
        public bool Done;

        protected string _name;
        protected uint   _id;

        public Process(string name)
        {
            _name   = name;
            _id     = ProcessManager.GenerateID();
            Running = false;
            Done    = false;
            Debug.OK("Created process - ID:%p Name:%s", _id, _name);
        }

        public virtual void Start() { Running = true;  Done = false; }
        public virtual void Stop()  { Running = false; Done = true;  }

        public abstract void Main();

        public string GetName() { return _name; }
        public uint   GetID()   { return _id; }
    }
}
