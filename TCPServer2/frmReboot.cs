using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace TCPServer2
{
    public partial class frmReboot : Form
    {
        public frmReboot()
        {
            InitializeComponent();
            label1.Text = "Are you sure you want to reboot unit " + Form1.selid + "?";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // send message TBD
            if (Form1.selid != "")
            {
                string data = "{VMC01," + Form1.selid + ",70,00,REBOOT}\r\n";

                if (Form1.foundsocket != null)
                {
                    //MessageBox.Show("foundsocket = " + Form1.foundsocket.RemoteEndPoint.ToString()); // TEST
                    AsynchronousSocketListener.Send(Form1.foundsocket, data);
                }
                else
                {
                    AsynchronousSocketListener.AddUnsentMessage(Form1.selid, "\r\n" + data);
                    //if (File.Exists("C:\\ProgramData\\TCPServer\\Unsent_Messages.txt"))
                    //{
                    //    StreamWriter unsent = new StreamWriter(new FileStream("C:\\ProgramData\\TCPServer\\Unsent_Messages.txt", FileMode.Append, FileAccess.Write));
                    //    //unsent.Write("\r\n" + missingunit.ToString() + ";" + data);
                    //    unsent.Write("\r\n" + data);
                    //    unsent.Close();
                    //}
                    //MessageBox.Show("foundsocket = null"); // TEST

                }
            }

            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        
    }
}
