using System;
using Rebex.Net;
using Rebex.IO;
//using Renci.SshNet;
//using Renci.SshNet.Sftp;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Project_Innovate
{
    public partial class Form1 : Form
    {
        /*
        string pathH = "//httpdocs";
        string host = "hermes.serverict.nl";
        string uN = "hermes";
        string pwd = "RCF&9xdr";
        int port = 22;
        */
        public Form1()
        {
            InitializeComponent();
            Rebex.Licensing.Key = "==AJxMmIrQeJ87QJaW9ySkUdVVE3N+N9me5ell+rGMjs4Y==";
        }

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

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                Hide();
                notifyIcon1.Visible = true;
            }
        }

        private bool allowVisible;     // ContextMenu's Show command used
        private bool allowClose;       // ContextMenu's Exit command used

        protected override void SetVisibleCore(bool value)
        {
            if (!allowVisible)
            {
                value = false;
                if (!this.IsHandleCreated) CreateHandle();
            }
            base.SetVisibleCore(value);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!allowClose)
            {
                this.Hide();
                e.Cancel = true;
            }
            base.OnFormClosing(e);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            notifyIcon1.Text = "Hermes Transfer";
        }

        private void exitApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var about = new About();
            about.Show();
        }

        private void transferFileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }



        private void uploadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Sftp client = new Sftp();
                client.Connect("hermes.serverict.nl");
                client.Login("hermes", "RCF&9xdr");
                client.Upload(@"D:\testSSD\*", "/httpdocs", TraversalMode.MatchFilesShallow,
                TransferMethod.Copy, ActionOnExistingFiles.OverwriteAll);
                MessageBox.Show("Directory files uploaded successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }



            Thread.Sleep(2000);
            string line = GetActiveWindowTitle();
            string[] values = line.Split(' ');
            string file = @values[0];

            try
            {

                string source = run_cmd(file);
                //string dest = pathH;

                //UploadSFTPFile(host, uN, pwd, source, dest, port);
                //MessageBox.Show("File has been Uploaded!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }



        private void downloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Sftp client = new Sftp();
                client.Connect("hermes.serverict.nl");
                client.Login("hermes", "RCF&9xdr");
                client.Download("/httpdocs/", @"D:\testSFTPD", TraversalMode.Recursive);
                MessageBox.Show("Directory downloaded from SFTP with success!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //INSERT CODE FOR DOWNLOAD
            if (true) //check if delete was successful
            {
                MessageBox.Show("File has been successfully deleted");
            }
            else
            {
                MessageBox.Show("Error: File could not be deleted");
            }
        }
    }
}
