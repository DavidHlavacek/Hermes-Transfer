using SHDocVw;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using System.IO;
using System.Collections;
using System.Diagnostics;
using CliWrap;
using CliWrap.Buffered;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        private static string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;
        }
        private static string run_cmd(string args)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = @"C:\Users\Miroslav\AppData\Local\Programs\Python\Python310\python.exe";
            start.Arguments = string.Format("{0} {1}", @"D:\intellij\Test\testing.py", args);
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    return result;
                }
            }
        }



        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        public static void Test(string name)
        {
            string appPath = @"D:\intellij\Test\dist\testing\testing.exe";
            Process proc = new Process();
            ProcessStartInfo si = new ProcessStartInfo(appPath, name);
            si.WindowStyle = ProcessWindowStyle.Normal;
            si.WorkingDirectory = @"D:\intellij\Test\dist\testing";
            si.Verb = "runas";             // UAC elevation required.
            si.UseShellExecute = true;     // Required for UAC elevation.
            si.RedirectStandardOutput = true;
            proc.StartInfo = si;
            proc.Start();
            using (StreamReader reader = proc.StandardOutput)
            {
                string result2 = reader.ReadToEnd();
                Console.WriteLine(result2);
            }
            proc.WaitForExit();
        }
        
        static void Main(string[] args)
        {
            Thread.Sleep(2000);
            string line = GetActiveWindowTitle();
            string[] values = line.Split(' ');
            string file = @values[0];
            //string path = run_cmd(file);
            Console.WriteLine(file);
            Test(file);

        }
    }
}