using System;
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
            //INSERT CODE FOR DOWNLOAD
            if (true) //check if upload was successful
            {
                MessageBox.Show("File has been successfully uploaded");
            }
            else
            {
                MessageBox.Show("Error: File could not be uploaded");
            }
        }

        private void downloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //INSERT CODE FOR DOWNLOAD
            if (true) //check if download was successful
            {
                MessageBox.Show("File has been successfully downloaded");
            }
            else
            {
                MessageBox.Show("Error: File could not be downloaded");
            }
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
