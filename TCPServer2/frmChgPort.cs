using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;


namespace TCPServer2
{
    public partial class frmChgPort : Form
    {
        static string newaddress = "";
        static string newport = "";

        public frmChgPort()
        {
            InitializeComponent();
            this.Text = "Change Port/IP Address for Unit " + Form1.selid;
            textBox2.Text = GetLocalIPAddress();
            textBox1.Text = AsynchronousSocketListener.port;
            
        }


        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }

        private void button1_Click(object sender, EventArgs e)
        {


            int n;
            bool isNumeric = int.TryParse(textBox1.Text, out n);
            int c = 0;
            int count = 0;
            while (c < textBox2.Text.Length)
            {
                if (textBox2.Text[c] == '.')
                {
                    count++;

                }
                c++;
            }


            if (isNumeric == false)
            {
                MessageBox.Show("Please enter a value between 0 and 65535", "Invalid Entry");
                return;
            }
            if (Convert.ToInt32(textBox1.Text) < 0 || Convert.ToInt32(textBox1.Text) > 65535)
            {
                MessageBox.Show("Please enter a value between 0 and 65535", "Invalid Port");
                return;
            }
            



            else
            {
                newport = textBox1.Text;
                newaddress = textBox2.Text;

                
                if (Form1.selid != "")
                {
                    string data = "{VMC01," + Form1.selid + ",69,00," + newaddress + "," + newport + "}\r\n";
                    
                    //if (Form1.foundsocket != null)
                    //{
                       
                    //    AsynchronousSocketListener.Send(Form1.foundsocket, data);
                    //}
                    //else  // if no socket is associated with the selected unit serial number, add message string to list of unsent messages for that unit
                    //{
                    AsynchronousSocketListener.AddUnsentMessage(Form1.selid, "\r\n" + data);
                    AsynchronousSocketListener.SendCurrTime(Form1.selid);
                       

                    //}
                }
                

                this.Close();
            }
        }
    }
}
