using System;
using Rebex.Net;
using Rebex.IO;
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
            string currentDir = Directory.GetCurrentDirectory();
            string pythonPath = Path.GetFullPath(Path.Combine(currentDir, @"..\..\..\..\..\Test\venv\Scripts\python.exe"));
            string scriptPath = Path.GetFullPath(Path.Combine(currentDir, @"..\..\..\..\..\Test\testing.py"));
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = pythonPath;
            start.Arguments = string.Format("{0} {1}", scriptPath, args);
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

        private string getFilePath()
        {
            MessageBox.Show("Click the file.");
            Thread.Sleep(3000);
            string line = GetActiveWindowTitle();
            string[] values = line.Split(' ');
            string file = @values[0];
            MessageBox.Show(file);
            MessageBox.Show("Close the file.");
            string resultF = run_cmd(file);
            string result = resultF.Substring(0, resultF.Length - 2);
            string source = String.Format(@"{0}", result);

            return source;
        }

        private void uploadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string currentDir = Directory.GetCurrentDirectory();
            string designatedFolder = Path.GetFullPath(Path.Combine(currentDir, @"..\..\..\..\..\Test\httpdocs\"));

            string source = getFilePath();
            File.Delete(designatedFolder + Path.GetFileName(source));

            deleteSFTPContents();            

            File.Copy(source, designatedFolder + Path.GetFileName(source));
            try
            {
                Sftp client = new Sftp();
                client.Connect("hermes.serverict.nl");
                client.Login("hermes", "RCF&9xdr");
                client.Upload(designatedFolder + "*", "/httpdocs/", TraversalMode.MatchFilesShallow,
                TransferMethod.Copy, ActionOnExistingFiles.OverwriteAll);
                MessageBox.Show("Directory files uploaded successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            File.Delete(designatedFolder + Path.GetFileName(source));

        }
        
        private void downloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string currentDir = Directory.GetCurrentDirectory();
            string designatedFolder = Path.GetFullPath(Path.Combine(currentDir, @"..\..\..\..\..\Test\httpdocs"));
            try
            {
                Sftp client = new Sftp();
                client.Connect("hermes.serverict.nl");
                client.Login("hermes", "RCF&9xdr");
                client.Download("/httpdocs/*", designatedFolder, TraversalMode.Recursive);
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
            deleteSFTPContents();
            MessageBox.Show("SFTP folder contents cleared!");
        }

        public void deleteSFTPContents()
        {
            try
            {
                Sftp client = new Sftp();
                client.Connect("hermes.serverict.nl");
                client.Login("hermes", "RCF&9xdr");
                client.Delete("/httpdocs/*", TraversalMode.MatchFilesShallow);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
