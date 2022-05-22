using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppPrototype
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitTimer();
        }
        private void InitTimer()
        {
            timer1 = new Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 2000;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //Console.WriteLine("Process id: " + GetActiveWindowPid());
            //Console.WriteLine("Process name: " + GetActiveWindowName());
            Console.WriteLine(GetActiveWindowFilepath());
        }

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

        public static int GetActiveWindowPid()
        {
            int processID = 0;
            uint threadID = GetWindowThreadProcessId(GetForegroundWindow(), out processID);
            //Process s 
            return processID;
        }

        public static string GetActiveWindowFilepath()
        {
            //create a new process object from the process id
            Process process = Process.GetProcessById(GetActiveWindowPid());
            //return the filepath of the main module of the process
            return process.MainModule.FileName;  
        }


        // *
        // * 
        // * Assigning definition to methods 
        // * 
        // *

        [DllImport("user32.dll")] //import user32.dll
        static extern IntPtr GetForegroundWindow(); //define GetForegroundWindow to method from imported dll
                                                    //returns a handle to the foreground window. The foreground window can be null in certain circumstances.
        [DllImport("user32.dll")] //import user32.dll
        static extern int GetWindowText(IntPtr hwnd, StringBuilder ss, int count); //define GetWindowText to method from imported dll

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);
    }
}
