﻿using System;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
                string source = @"C:\Users\arian\Desktop\testYAY.txt";
                string dest = pathH;

                UploadSFTPFile(host, uN, pwd, source, dest, port);
                MessageBox.Show("File has been Uploaded!");
            }
            catch(Exception ex)
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
