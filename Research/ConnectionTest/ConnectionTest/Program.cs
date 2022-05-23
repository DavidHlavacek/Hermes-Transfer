using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using System.Management;

namespace BluetoothClassicConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            BluetoothClient client = new BluetoothClient();

            BluetoothDeviceInfo device = null;
            foreach (var dev in client.DiscoverDevices())
            {
                if (dev.DeviceName.Contains("ESP32test"))
                {
                    device = dev;
                    break;
                }
            }
            Console.WriteLine(device.ClassOfDevice);
            Console.WriteLine(device.DeviceName);

            //string comPort = GetBluetoothPort(device.DeviceAddress.ToString());


            if (!device.Authenticated)
            {
                BluetoothSecurity.PairRequest(device.DeviceAddress,"1234");
            }
            device.Refresh();
            Console.WriteLine(device.Authenticated);

            client.Connect(device.DeviceAddress, BluetoothService.SerialPort);
            Console.WriteLine(device.Connected);
            
            var stream = client.GetStream();
            StreamWriter sw = new StreamWriter(stream, System.Text.Encoding.ASCII);
            sw.WriteLine("Hello world!\r\n\r\n");
            sw.Close();

            client.Close();
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
    }
}
