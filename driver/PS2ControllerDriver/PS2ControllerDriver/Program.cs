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
    //class Button
    //{
    //    public string Name { get; set; }
    //    public VirtualKeyCode Key { get; set; }
    //    public bool Pressed { get; set; }
    //    public double Time { get; set; }
    //}

    class Program
    {
        static void Main(string[] args)
        {
            SerialPort serialPort = new SerialPort("COM3", 57600);
            serialPort.ReadTimeout = 0;
            serialPort.Open();
            Controller controller = new Controller();
            while (true)
            {
                try
                {
                    byte[] data = { 0, 0, 0, 0 };
                    serialPort.Read(data, 0, 4);
                    if(data[0] != 0 || data[1] != 0)
                    {
                        Console.WriteLine(Convert.ToString(data[0], 2).PadLeft(8, '0'));
                        Console.WriteLine(Convert.ToString(data[1], 2).PadLeft(8, '0'));
                        Console.WriteLine(Convert.ToString(data[2], 2).PadLeft(8, '0'));
                        Console.WriteLine(Convert.ToString(data[3], 2).PadLeft(8, '0'));
                        Console.WriteLine();
                    }
                    BitArray bitArray = new BitArray(new byte[] { data[0], data[1] });
                    int x = data[2], y = data[3];
                    if (x != 0 || y != 0)
                        controller.LeftSticker.MoveMouse(x, y);

                    for (int i = 0; i < controller.Buttons.Count; ++i)
                    {
                        var button = controller.Buttons[i];
                        if (bitArray[i] != button.Pressed)
                        {
                            controller.HandleButton(button);
                        }
                    }
                }
                catch (TimeoutException) { }
            }
        }
    }
}
