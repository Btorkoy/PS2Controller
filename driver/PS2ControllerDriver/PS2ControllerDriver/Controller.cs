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
        public double Time { get; set; }

        public override string ToString() {
            var pr = Pressed ? "down" : "up";
            return $@"Button: {Name}, Key: {Key}, {pr} at {Time}"; }
    }

    class Controller
    {
        public List<Button> Buttons = new List<Button>();
        private InputSimulator inputSimulator = new WindowsInput.InputSimulator();

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
                    Time = 0.0
                });
            }
            Console.WriteLine("Virtual Controller intialized");
        }

        public void HandleButton(Button button)
        {
            var now = DateTime.Now.Ticks;
            button.Pressed = !button.Pressed;
            Console.WriteLine(button);
            if (button.Pressed)
                inputSimulator.Keyboard.KeyDown(button.Key);
            else
                inputSimulator.Keyboard.KeyUp(button.Key);
            button.Time = now;
        } 
    }
}
