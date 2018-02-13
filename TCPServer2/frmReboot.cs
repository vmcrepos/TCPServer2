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
            if (Form1.selid != "")
            {
                string data = "{VMC01," + Form1.selid + ",70,00,REBOOT}\r\n";

                if (Form1.foundsocket != null)
                {
                    
                    AsynchronousSocketListener.Send(Form1.foundsocket, data);
                }
                else  // if no socket is associated with the selected unit serial number, add message string to list of unsent messages for that unit
                {
                    AsynchronousSocketListener.AddUnsentMessage(Form1.selid, "\r\n" + data);
                
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
