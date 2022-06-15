using InTheHand.Net.Sockets;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Connection
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string currentPort = null;

            while (currentPort == null)
            {
                Console.WriteLine("Looking for port");
                currentPort = GetBluetoothPort(getBluetoothAddress());
            }
            Console.WriteLine("Port found! " + currentPort);
            SerialPort serialPort = new SerialPort("COM12", 9600, Parity.None, 8);

            serialPort.DtrEnable = true;
            serialPort.RtsEnable = true;

            serialPort.Open();
            if (serialPort.IsOpen)
            {
                while (serialPort.IsOpen)
                {
                    while (serialPort.BytesToRead > 0)
                    {
                        Console.Write(Convert.ToChar(serialPort.ReadChar()));
                    }
                }
            }
            else { Console.WriteLine("not connecting"); }
        }

        private static string GetBluetoothPort(string deviceAddress)
        {
            const string Win32_SerialPort = "Win32_SerialPort";
            SelectQuery q = new SelectQuery(Win32_SerialPort);
            ManagementObjectSearcher s = new ManagementObjectSearcher(q);
            foreach (object cur in s.Get())
            {
                ManagementObject mo = (ManagementObject)cur;
                string pnpId = mo.GetPropertyValue("PNPDeviceID").ToString();
                if (pnpId.Contains(deviceAddress))
                {
                    object captionObject = mo.GetPropertyValue("Caption");
                    string caption = captionObject.ToString();
                    int index = caption.LastIndexOf("(COM");
                    if (index > 0)
                    {
                        string portString = caption.Substring(index);
                        string comPort = portString.
                            Replace("(", string.Empty).Replace(")", string.Empty);
                        return comPort;
                    }
                }
            }
            return null;
        }

        private static string getBluetoothAddress()
        {
            BluetoothClient client = new BluetoothClient();

            BluetoothDeviceInfo device = null;
            foreach (var dev in client.DiscoverDevices())
            {
                if (dev.DeviceName.Contains("H-C-2010-06-01"))
                {
                    device = dev;
                    break;
                }
            }

            return device.DeviceAddress.ToString();
        }
    }
}
