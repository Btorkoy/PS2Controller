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
    class Program
    {
        static void Main(string[] args)
        {
            SerialPort serialPort = new SerialPort("COM5", 57600);
            serialPort.ReadTimeout = 0;
            serialPort.Open();
            Controller controller = new Controller();
            while (true)
            {
                try
                {
                    int l_stickX = 0;
                    int l_stickY = 1;
                    int buttons1 = 2;
                    int buttons2 = 3;
                    byte[] data = { 0, 0, 0, 0 };
                    serialPort.Read(data, 0, 4);
                    if(data[buttons1] != 0 || data[buttons2] != 0 )
                    {
                        Console.WriteLine(Convert.ToString(data[buttons1], 2).PadLeft(8, '0'));
                        Console.WriteLine(Convert.ToString(data[buttons2], 2).PadLeft(8, '0'));
                        Console.WriteLine(Convert.ToString(data[l_stickX], 2).PadLeft(8, '0'));
                        Console.WriteLine(Convert.ToString(data[l_stickY], 2).PadLeft(8, '0'));
                        Console.WriteLine();
                    }
                    BitArray bitArray = new BitArray(new byte[] { data[buttons1], data[buttons2] });
                    int x = data[l_stickX], y = data[l_stickY];
                    //if (x != 0 || y != 0)
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
