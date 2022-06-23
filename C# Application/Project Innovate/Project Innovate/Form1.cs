using System;
using Renci.SshNet;
using Renci.SshNet.Sftp;
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
        string pathH = "//httpdocs";
        string host = "hermes.serverict.nl";
        string uN = "hermes";
        string pwd = "RCF&9xdr";
        int port = 22;
        public Form1()
        {
            InitializeComponent();
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
            string currentDir = System.IO.Directory.GetCurrentDirectory();
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



        private void uploadToolStripMenuItem_Click(object sender, EventArgs e)
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
            string dest = pathH;
            try
            {
                UploadSFTPFile(host, uN, pwd, source, dest, port);
                MessageBox.Show("File has been Uploaded!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }



            //INSERT CODE FOR UPLOAD
            /*if (true) //check if upload was successful
            {
                MessageBox.Show("File has been successfully uploaded");
            }
            else
            {
                MessageBox.Show("Error: File could not be uploaded");
            }
            */
        }

        private void UploadSFTPFile(string host, string username, string password, string sourcefile, string destination, int port)

        {

            using (SftpClient client = new SftpClient(host, port, username, password))

            {

                client.Connect();

                client.ChangeDirectory(destination);

                using (FileStream fs = new FileStream(sourcefile, FileMode.Open))

                {

                    client.BufferSize = 4 * 1024;

                    client.UploadFile(fs, Path.GetFileName(sourcefile));

                }

            }

        }

        /*private void downloadSFTPfile(string localPath, string remotePath)
        {
            SftpClient client = new SftpClient(host, port, uN, pwd);
            client.Connect();

            try
            {
                var s = File.Create(localPath);
                client.DownloadFile(remotePath, s);
                MessageBox.Show("Files have been downloaded!");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
            }
            finally
            {
                client.Disconnect();
            }
        }*/


        private void downloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SftpClient client = new SftpClient(host, port, uN, pwd);
            client.Connect();
            string sv = "//httpdocs/testYAY.txt";
            string local = @"D:\testSSD\test.txt";

            using(Stream stream = File.OpenWrite(local))
            {
                client.DownloadFile(sv, stream, x=>MessageBox.Show(x.ToString()));
            }

           /* try
            {
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(exception.Message.ToString());
            }
            finally
            {
                client.Disconnect();
            }*/


            //downloadSFTPfile(@"D:\testSSD", pathH);

            //INSERT CODE FOR DOWNLOAD
            /*if (true) //check if download was successful
            {
                MessageBox.Show("File has been successfully downloaded");
            }
            else
            {
                MessageBox.Show("Error: File could not be downloaded");
            }*/
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
