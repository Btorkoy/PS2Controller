using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsInput;

namespace PS2ControllerDriver
{
    public class Config
    {
        public Dictionary<string, string> Mapping;
    }
    class Controller
    {
        public readonly Dictionary<string, Button> Buttons = new Dictionary<string, Button>();
        public const int delay = 1000000;

        public Controller()
        {
            var config = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "config.json")));
            foreach (var el in config)
            {
                var b = el.Key;
                Console.WriteLine(b);
                this.Buttons.Add(b, new Button
                {
                    Name = el.Key,
                    Key = el.Value,
                    Pressed = false,
                    Time = DateTime.Now.Ticks
                });
            }
        }

        public void HandleButton(Button button)
        {
            var now = DateTime.Now.Ticks;
           // if((now - button.Time) > delay)
        } 
        public void DownButton(Button button)
        {
            var now = DateTime.Now.Ticks;
           //if (!button.Pressed && (now - button.Time) > delay)
            {
                Console.WriteLine($"Button {button.Name} pressed once! Last: {button.Time}; Now: {now}");
                button.Down();
                button.Time = now;
            }
            //else if ((now - button.Time) > delay)
            //{
            //    Console.WriteLine($"Button {button.Name} hold! Last: {button.Time}; Now: {now}. {now - button.Time}");
            //    button.Down();
            //}
        }

        public void UpButton(Button button)
        {
            if (button.Pressed) button.Pressed = false;
        }

        public class Button
        {
            public string Name = "";
            public string Key = "";
            public bool Pressed = false;
            public long Time;

            public void Down() {
                this.Pressed = true;
                new InputSimulator().Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_6);
                SendKeys.SendWait(this.Key);
            }

            public void Up() => this.Pressed = false;

            public override string ToString() =>
                $"Button: {Name}, Key: {Key}, Pressed: {Pressed} at {Time}";
        }

    }
}
