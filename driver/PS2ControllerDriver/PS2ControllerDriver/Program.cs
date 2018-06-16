using System;
using System.IO.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Newtonsoft.Json;
using System.IO;
using System.Windows.Forms;
using WindowsInput.Native;

namespace PS2ControllerDriver
{
    class Key
    {
        public VirtualKeyCode key;
        public bool pressed = false;
        public double time;
    }
    class Program
    {
        static void Main(string[] args)
        {
            SerialPort serialPort = new SerialPort("COM3", 57600);
            serialPort.ReadTimeout = 0;
            serialPort.Open();
            Controller controller = new Controller();
            Key[] keys = new Key[8];
            VirtualKeyCode[] b = {
                VirtualKeyCode.VK_0,
                VirtualKeyCode.VK_1,
                VirtualKeyCode.VK_2,
                VirtualKeyCode.VK_3,
                VirtualKeyCode.VK_4,
                VirtualKeyCode.VK_5,
                VirtualKeyCode.VK_6,
                VirtualKeyCode.VK_7
            };
            for(int i = 0; i < b.Length; ++i)
            {
                keys[i] = new Key
                {
                    key = b[i],
                    pressed = false,
                    time = 0.0
                };
            }
            var vK = new WindowsInput.InputSimulator().Keyboard;
            while (true)
            {
                try
                {
                    byte[] data = new byte[1] ;
                    serialPort.Read(data, 0, 1);
                    BitArray bitArray = new BitArray(data);
                    for (int i = 0; i < 8; ++i)
                    {
                        var key = keys[i];
                        var now = DateTime.Now.Ticks;
                        if (bitArray[i] != key.pressed)
                        {
                            key.pressed = bitArray[i];
                            Console.WriteLine($"{key.key}, {key.pressed}");
                            if (key.pressed)
                                vK.KeyDown(key.key);
                            else
                                vK.KeyUp(key.key);
                            key.time = now;
                        }
                    }
                    //for (int i = 0; i < controller.Buttons.Count; ++i)
                    //{
                    //    var button = controller.Buttons.ElementAt(i).Value;
                    //    var now = DateTime.Now.Ticks;
                    //    //if (bitArray[i] != button.Pressed)
                    //    //{
                    //    //    if (button.Pressed) button.Release();
                    //    //    else button.Press();
                    //    //    button.Time = now;
                    //    //}

                    //    if (bitArray[i] == button.Pressed)
                    //    {
                            
                    //            Console.WriteLine(Convert.ToString(data[0], 2).PadLeft(8, '0'));
                            
                    //        if (button.Pressed) SendKeys.SendWait(button.Key);
                    //        button.Pressed = !button.Pressed;
                    //        button.Time = now;
                    //    }
                    //    //    controller.DownButton(button);
                    //    //else controller.UpButton(button);
                    //}
                }
                catch (TimeoutException) { }
            }
        }

        public static void GetPressedKeysList(byte[] data, List<string> buttons)
        {
            
        }
    }

   

    
}
