using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PurpleMoon.Core;
using PurpleMoon.Graphics;
using PurpleMoon.Multitasking;

namespace PurpleMoon.HAL
{
    public class DriverManager : Process
    {
        public List<Driver> Drivers { get; private set; }

        private uint _dev_id;

        public DriverManager() : base("devmgr")
        {

        }

        public override void Start()
        {
            base.Start();

            _dev_id = 0;
            Drivers = new List<Driver>();

            Detect();
            Debug.OK("Initialized driver manager");
        }

        public override void Stop()
        {
            base.Stop();
        }

        public override void Main()
        {
            foreach (Driver driver in Drivers)
            {
                driver.Update();
            }
        }

        private void Detect()
        {
            DetectVideo();

            Install(new PIT(), true);
            Install(new Keyboard(), true);
            Install(new Mouse(), true);
        }

        private void DetectVideo(bool force_vbe = false)
        {
            if (force_vbe && Cosmos.Core.VBE.IsAvailable()) { Install(new VBEAdapter()); return; }
            else
            {
                Cosmos.HAL.PCIDevice vmware_svga = Cosmos.HAL.PCI.GetDevice(Cosmos.HAL.VendorID.VMWare, Cosmos.HAL.DeviceID.SVGAIIAdapter);
                if (vmware_svga != null) { Install(new VMWareSVGA()); return; }
                else if (Cosmos.Core.VBE.IsAvailable()) { Install(new VBEAdapter()); return; }
            }
            Debug.Panic("Failed to locate graphical video device");
        }

        public void Install(Driver driver, bool start = true)
        {
            Drivers.Add(driver);
            Debug.Info("Installed driver - ID:%u Name:%s", driver.GetID(), driver.GetName());
            if (start) { driver.Start(); }
        }

        public Driver Fetch(string name)
        {
            foreach (Driver dev in Drivers) { if (dev.GetName() == name) { return dev; } }
            return null;
        }

        public uint GenerateID() { return _dev_id++; }
    }
}
