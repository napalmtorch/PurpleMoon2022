using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PurpleMoon.Core;

namespace PurpleMoon.HAL
{
    public abstract class Driver
    {
        protected string _name;
        protected uint   _id;

        public Driver(string name)
        {
            _name = name;
            _id   = Kernel.DriverMgr.GenerateID();
        }

        public abstract void Start();
        public abstract void Stop();

        public virtual void Update() { }

        public string GetName() { return _name; }

        public uint GetID() { return _id; }
    }
}
