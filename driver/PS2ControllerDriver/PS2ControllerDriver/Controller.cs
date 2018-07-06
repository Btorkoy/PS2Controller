using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;

namespace PS2ControllerDriver
{
    class Button
    {
        public string Name { get; set; }
        public VirtualKeyCode Key { get; set; }
        public bool Pressed { get; set; }
        public DateTime Time { get; set; }

        public override string ToString() {
            var pr = Pressed ? "down" : "up";
            return $@"Button: {Name}, Key: {Key}, {pr} at {Time}"; }
    }

    class Stick
    {
        int range = 20;               // output range of X or Y movement
        int threshold;      // resting threshold
        int center;
        IMouseSimulator MouseSimulator = new InputSimulator().Mouse;

        public Stick()
        {
            this.threshold = range / 4;
            this.center = range / 2;
        }
        public void MoveMouse(int x, int y)
        {
            //x = (x * range) / 255;
            //y = (y * range) / 255;
            int distanceX = x - center;
            int distanceY = y - center;
            if (Math.Abs(distanceX) < threshold) distanceX = 0;
            if (Math.Abs(distanceY) < threshold) distanceY = 0;
            if (distanceX != 0 || distanceY != 0)
            {
                Console.WriteLine($"Stick moved x: {distanceX}, y: {distanceY}");
                this.MouseSimulator.MoveMouseBy(distanceX, distanceY);
            }
        }
    }

    class Controller
    {
        public List<Button> Buttons = new List<Button>();
        private InputSimulator inputSimulator = new WindowsInput.InputSimulator();
        public Stick LeftSticker = new Stick();

        public Controller()
        {
            var config = JsonConvert.DeserializeObject<Dictionary<string, VirtualKeyCode>>(File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "config.json")));
            for (int i = 0; i < config.Count; ++i)
            {
                this.Buttons.Add(new Button
                {
                    Name = config.ElementAt(i).Key,
                    Key = config.ElementAt(i).Value,
                    Pressed = false,
                    Time = DateTime.MinValue
                });
            }

            Console.WriteLine("Virtual Controller intialized");
        }

        public void HandleButton(Button button)
        {
            var now = DateTime.Now;
            button.Pressed = !button.Pressed;
            Console.WriteLine(button);
            switch (button.Key)
            {
                case VirtualKeyCode.LBUTTON:
                    if (button.Pressed) inputSimulator.Mouse.LeftButtonDown();
                    else inputSimulator.Mouse.LeftButtonUp();
                    break;
                case VirtualKeyCode.RBUTTON:
                    if (button.Pressed) inputSimulator.Mouse.RightButtonDown();
                    else inputSimulator.Mouse.RightButtonUp();
                    break;
                default:
                    if (button.Pressed) inputSimulator.Keyboard.KeyDown(button.Key);
                    else inputSimulator.Keyboard.KeyUp(button.Key);
                    break;
            }
            button.Time = now;
        }
    }
}
