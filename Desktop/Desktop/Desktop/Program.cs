using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Management;
using System.Runtime.InteropServices;

namespace Desktop
{
    class Program
    {
        [DllImport("user32.dll")] //import user32.dll
        static extern IntPtr GetForegroundWindow(); //define GetForegroundWindow to method from imported dll
                                                    //returns a handle to the foreground window. The foreground window can be null in certain circumstances.
        [DllImport("user32.dll")] //import user32.dll
        static extern int GetWindowText(IntPtr hwnd, StringBuilder ss, int count); //define GetWindowText to method from imported dll
        public static string GetActiveWindowName()
        {
            //Create the variable
            const int nChar = 256;
            StringBuilder ss = new StringBuilder(nChar);

            //Run GetForeGroundWindows and get active window informations
            //assign them into handle pointer variable
            IntPtr handle = IntPtr.Zero;
            handle = GetForegroundWindow();

            if (GetWindowText(handle, ss, nChar) > 0) return ss.ToString();
            else return null;
        }
        public static void GetComPorts()
        {
            //SELECT ALL COM PORTS FROM Win32_PnPEntity
            using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE Caption like '%(COM%'"))
            {
                var portnames = SerialPort.GetPortNames(); //store all the port names
                var ports = searcher.Get().Cast<ManagementBaseObject>().ToList().Select(p => p["Caption"].ToString());

                var portList = portnames.Select(n => n + " - " + ports.FirstOrDefault(s => s.Contains(n))).ToList();

                foreach (string s in portList)
                {
                    Console.WriteLine(s);
                }
            }
        }

        static void Main(string[] args)
        {
            GetComPorts();
            //Console.WriteLine(GetActiveWindowName());
            Console.ReadLine();
        }
    }
}
