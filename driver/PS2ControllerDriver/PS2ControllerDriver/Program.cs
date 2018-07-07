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
            SerialPort serialPort = new SerialPort()
            {
                BaudRate = 57600,
                PortName = "COM5",
                ReadTimeout = 0
            };
            serialPort.Open();
            Controller controller = new Controller();
            while (true)
            {
                try
                {
                    int leftStick = 0;
                    int rightStick = 1;
                    int buttons1 = 2;
                    int buttons2 = 3;
                    byte[] data = { 0, 0, 0, 0 };
                    serialPort.Read(data, 0, 4);
                    if(data[buttons1] != 0 || data[buttons2] != 0 )
                    {
                        Console.WriteLine(Convert.ToString(data[leftStick], 2).PadLeft(8, '0'));
                        Console.WriteLine(Convert.ToString(data[rightStick], 2).PadLeft(8, '0'));
                        Console.WriteLine(Convert.ToString(data[buttons1], 2).PadLeft(8, '0'));
                        Console.WriteLine(Convert.ToString(data[buttons2], 2).PadLeft(8, '0'));
                        Console.WriteLine();
                    }
                    int leftX = data[leftStick] >> 4;
                    int leftY = data[leftStick] & 15;
                    int rightX = data[rightStick] >> 4;
                    int rightY = data[rightStick] & 15;
                    controller.LeftSticker.Move(leftX, leftY);
                    controller.RightSticker.Move(rightX, rightY);

                    BitArray bitArray = new BitArray(new byte[] { data[buttons1], data[buttons2] });
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
