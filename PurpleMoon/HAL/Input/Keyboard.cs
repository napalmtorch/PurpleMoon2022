using PurpleMoon.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurpleMoon.HAL
{
    public class Keyboard : Driver
    {
        public bool[] Keymap { get; private set; }

        public Keyboard() : base("Keyboard")
        {

        }

        public override void Start()
        {
            Keymap = new bool[256];

            var kb_list = Cosmos.HAL.Global.GetKeyboardDevices().ToArray();
            if (kb_list.Length == 0) { Debug.Panic("No keyboard detected"); }
            kb_list[0].OnKeyPressed = OnKeyPressed;
            Debug.OK("PS/2 keyboard detected");
        }

        public override void Stop()
        {
            
        }

        public override void Update()
        {
            base.Update();
        }

        private void OnKeyPressed(byte scancode, bool released)
        {
            if (Keymap == null) { return; }
            Keymap[scancode] = !released;
        }

        public bool IsKeyDown(Key key) { return Keymap[(int)key]; }
        
        public bool IsKeyUp(Key key) { return !Keymap[(int)key]; }
    }
}
