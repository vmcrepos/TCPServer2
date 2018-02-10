

using System;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Data.SqlClient;
using System.Collections;
using System.Net.Mail;
using System.Net;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Net.Sockets;

namespace TCPServer2
{
    public partial class Form1 : Form
    {

        public static bool fwreq = false;
        public static string fwfile;
        static string connectionString = @"Server=P4469916\VLINKDB; Initial Catalog=VLink106466; User=vlink; Password='TestVlink'; Integrated Security=True";
        DataTable dt = new DataTable();
        DataTable dt2 = new DataTable();
        public static string selid = "";
        public static Socket foundsocket = null;





        //{
        //public static bool frmloaded = false;
        delegate void SetTextCallback(string text);
        //public string Result { get { return this.rtbOutgoing.Text; } set { this.rtbOutgoing.Text = value; } }
        public Form1()
        {
            InitializeComponent();
            //lblStatus.Text = " ";
            //AsynchronousSocketListener.StartListening();
            //lblStatus.Text = "Waiting for a connection...";
            //AsynchronousSocketListener.StartListening();
            //return;
            //FormLoad(null, null);
            // Handle the ApplicationExit event to know when the application is exiting.
            Application.ApplicationExit += new EventHandler(this.OnApplicationExit);
            

            btnStart_Click(null, null);
            
            //lstUnits.DrawMode = DrawMode.OwnerDrawFixed;
            //lstUnits.DrawItem += new DrawItemEventHandler(lstUnits_DrawItem);
            


        }
    

       



        private void btnStart_Click(object sender, EventArgs e)
        {
            //AsynchronousSocketListener l = new  AsynchronousSocketListener();
            AsynchronousSocketListener.Startup();
            AsynchronousSocketListener.Incoming += L_Incoming;
            AsynchronousSocketListener.Outgoing += AsynchronousSocketListener_Outgoing;
            lblStatus.Text = "Waiting for a TCP connection...";
            AsynchronousSocketListener.Connected += AsynchronousSocketListener_Connected;
            // FormLoad(null, null);
            btnStart.Enabled = false;

            CheckForNewUnits();
            timer4.Enabled = true;


            //string query = "SELECT SerialNumber FROM [VLink106466].[dbo].VLinkUnit";

            //try
            //{
            //    using (SqlConnection conn = new SqlConnection(connectionString))
            //    {
            //        using (SqlCommand comm = new SqlCommand(query, conn))
            //        {
            //            conn.Open();

            //            SqlDataAdapter da = new SqlDataAdapter();
            //            DataSet ds = new DataSet();


            //            // create dataset and datatable from returned data
            //            da.SelectCommand = comm;
            //            da.Fill(ds, "UnitTable");
            //            dt = ds.Tables["UnitTable"];
            //            BindingSource bSource2 = new BindingSource();
            //            dataGridView1.Visible = true;
            //            bSource2.DataSource = dt;

            //            dataGridView1.DataSource = bSource2;
                        
            //        }
            //    }

            //}

            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}

            //foreach (DataRow dr in dt.Rows)
            //{
            //    //lstUnits.Items.Add(dr[0]);
            //    lstUnits2.Items.Add(dr[0]);


            //}
        }

        //private void lstUnits_DrawItem(object sender, DrawItemEventArgs e)
        //{
            
        //    e.DrawBackground();
           
        //    if (e.Index % 2 == 0)
        //        e.Graphics.DrawString(lstUnits.Items[e.Index].ToString(), new Font("Arial", 10, FontStyle.Bold), Brushes.Black, e.Bounds);
        //    else
        //        e.Graphics.DrawString(lstUnits.Items[e.Index].ToString(), new Font("Arial", 10, FontStyle.Regular), Brushes.Black, e.Bounds);
        //    e.DrawFocusRectangle();
        //}







    

        private void AsynchronousSocketListener_Outgoing(string Data)
        {
            SetOutgoingText(Data);
            //throw new NotImplementedException();
        }

        private void AsynchronousSocketListener_Connected(string IPAddress)
        {
            if (this.lblStatus.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(AsynchronousSocketListener_Connected);
                this.Invoke(d, new object[] { IPAddress });
            }
            else
            {
                lblStatus.Text = "Connected to: " + IPAddress;
            }
            


            //throw new NotImplementedException();
        }

        private void L_Incoming(object sender, string Data)
        {
            SetText(Data);

            //    rtbIncoming.AppendText(Data);
            //throw new NotImplementedException();
        }
        public void SetOutgoingText(string text)
        {
            if (this.rtbOutgoing.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetOutgoingText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.rtbOutgoing.AppendText(text);
                this.rtbOutgoing.ScrollToCaret();
            }

            if (rtbOutgoing.Lines.Length >= 1000)
                rtbOutgoing.Clear();
        }
        public void SetText(string text)
        {
            if (this.rtbIncoming.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.rtbIncoming.AppendText(text);
                this.rtbIncoming.ScrollToCaret();
            }

            if (rtbIncoming.Lines.Length >= 1000)
                rtbIncoming.Clear();
        }

       
        private void Form1_Load(object sender, EventArgs e)
        {

        }

       
        
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (AsynchronousSocketListener.clientSockets.Count != 0)
                AsynchronousSocketListener.CloseSocket();
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private void OnApplicationExit(object sender, EventArgs e)
        {
            if (AsynchronousSocketListener.clientSockets.Count != 0)
                AsynchronousSocketListener.CloseSocket();
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        public void btnSend_Click(object sender, EventArgs e)
        {
            // set flag indicating firmware update required
            if (btnSend.Text == "Enter FW Update Mode")
            {
                AsynchronousSocketListener.fwreq = true;
                btnSend.Text = "Exit FW Update Mode";
                lblUpdateFW.BackColor = Color.Yellow;
                lblUpdateFW.ForeColor = Color.Red;
                lblUpdateFW.Text = "FW Update Mode ENABLED";
            }
            else
            {
                AsynchronousSocketListener.fwreq = false;
                AsynchronousSocketListener.fwreq2 = false;
                btnSend.Text = "Enter FW Update Mode";
                lblUpdateFW.BackColor = DefaultBackColor;
                lblUpdateFW.ForeColor = DefaultForeColor;
                lblUpdateFW.Text = "FW Update Mode NOT ENABLED";

            }
            //AsynchronousSocketListener.FWSend();




            //// Show the open file dialog to select firmware file

            //openFileDialog1.ShowDialog();
            //fwfile = openFileDialog1.FileName;
            //m_split = rtbIncoming.Text.m_split(delimiter);
            //int limit = m_split.Length;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //AsynchronousSocketListener.checkact = true;
            //if (AsynchronousSocketListener.checkact)
            //if (!AsynchronousSocketListener.fwreq2)
            if (!AsynchronousSocketListener.fwstart)
                AsynchronousSocketListener.CheckActions();
                

            
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            AsynchronousSocketListener.SendTestText(rtbOutgoing.Text);
            
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (AsynchronousSocketListener.fwstart == false)
                AsynchronousSocketListener.SendCurrTime(); // send "set time" message periodically
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //for (int x = 0; x < lstUnits2.Items.Count; x++)
            for (int x = 0; x < dataGridView1.RowCount; x++)
            {
                //if (x % 2 == 0)
                //    lstUnits2.SetItemChecked(x, true);
                //if (AsynchronousSocketListener.sernumdict.ContainsValue(lstUnits2.Items[x].ToString()))
                //    lstUnits2.SetItemChecked(x, true);
                //else if (!AsynchronousSocketListener.sernumdict.ContainsValue(lstUnits2.Items[x].ToString()))
                //    lstUnits2.SetItemChecked(x, false);
                if (AsynchronousSocketListener.sernumdict.ContainsValue(dataGridView1.Rows[x].Cells[0].Value.ToString()))
                    dataGridView1.Rows[x].Cells[0].Style.Font = new Font(dataGridView1.DefaultCellStyle.Font, FontStyle.Bold | FontStyle.Italic);

                //else if (!AsynchronousSocketListener.sernumdict.ContainsValue(lstUnits2.Items[x].ToString()))
                //    lstUnits2.SetItemChecked(x, false);
               
            }
        }


        private void timer3_Tick(object sender, EventArgs e)
        {
            //for (int x = 0; x < lstUnits2.Items.Count; x++)
            //{
            //    if (AsynchronousSocketListener.sernumdict.ContainsValue(lstUnits2.Items[x].ToString()))

//    {
//        //MessageBox.Show("checked = true for " + x.ToString());
//        lstUnits2.SetItemChecked(x, true);

//    }

//    else
//    {
//        //MessageBox.Show("checked = false for " + x.ToString());
//        lstUnits2.SetItemChecked(x, false);
//    }
//}
            
            AsynchronousSocketListener.CheckConnection(); // check if disconnection has occurred

            for (int x = 0; x < dataGridView1.RowCount; x++)
            {
                //if (x % 2 == 0)
                //    lstUnits2.SetItemChecked(x, true);
                //if (AsynchronousSocketListener.sernumdict.ContainsValue(lstUnits2.Items[x].ToString()))
                //    lstUnits2.SetItemChecked(x, true);
                //else if (!AsynchronousSocketListener.sernumdict.ContainsValue(lstUnits2.Items[x].ToString()))
                //    lstUnits2.SetItemChecked(x, false);

                // if socket connected (serial number still included in dictionary), display serial number in datagridview list
                // using bold and italic font
                if (AsynchronousSocketListener.sernumdict.ContainsValue(dataGridView1.Rows[x].Cells[0].Value.ToString()))
                    dataGridView1.Rows[x].Cells[0].Style.Font = new Font(dataGridView1.DefaultCellStyle.Font, FontStyle.Bold | FontStyle.Italic);

                //else if (!AsynchronousSocketListener.sernumdict.ContainsValue(lstUnits2.Items[x].ToString()))
                //    lstUnits2.SetItemChecked(x, false);

                // if socket disconnected (serial number no longer included in dictionary), display serial number in datagridview
                // list using regular font
                else if (!AsynchronousSocketListener.sernumdict.ContainsValue(dataGridView1.Rows[x].Cells[0].Value.ToString()))
                    dataGridView1.Rows[x].Cells[0].Style.Font = new Font(dataGridView1.DefaultCellStyle.Font, FontStyle.Regular);

                
            }
            
        }

        // this function returns the socket associated with the serial number selected in the datagridview object
        private Socket getSocket()
        {
            string foundsernum = "";
            foundsocket = null;

            //try
            //{
            //    if (selid == "")
            //    {
            //        MessageBox.Show("Please select (click on) a unit from the serial number list");
            //        selid = dataGridView1.SelectedCells[0].Value.ToString();
            //        label4.Text = "Selected unit: " + selid;
            //    }
            //}

            //catch (Exception e)
            //{
            //    MessageBox.Show(e.ToString());
            //}
                

            //if (AsynchronousSocketListener.sernumdict.ContainsValue(dataGridView1.SelectedCells[0].Value.ToString()))
            if (AsynchronousSocketListener.sernumdict.ContainsValue(selid))
            {
                // create array of serial numbers
                Dictionary<Socket, string>.ValueCollection valueColl =
                    AsynchronousSocketListener.sernumdict.Values;
                string[] sernumarray = new string[AsynchronousSocketListener.sernumdict.Count];
                valueColl.CopyTo(sernumarray, 0);

                // create array of sockets
                Dictionary<Socket, string>.KeyCollection keyColl =
                            AsynchronousSocketListener.sernumdict.Keys;
                Socket[] sockarray = new Socket[AsynchronousSocketListener.sernumdict.Count];
                keyColl.CopyTo(sockarray, 0);



                for (int x = 0; x < sernumarray.Length; x++)
                {
                    // get socket associated with the selected serial number string
                    //if (sernumarray[x] == dataGridView1.SelectedCells[0].Value.ToString())
                    if (sernumarray[x] == selid)
                    {
                        foundsernum = sernumarray[x];
                        foundsocket = sockarray[x];
                    }
                }

                
            }

            return foundsocket;
        }

        private void btnSendTime_Click(object sender, EventArgs e) // send a "set time" message to the unit whose serial number is selected in the datagridview object
        {
            //string foundsernum = "";
            //Socket foundsocket;

            //if (AsynchronousSocketListener.sernumdict.ContainsValue(dataGridView1.SelectedCells[0].Value.ToString()))
            //{
            //    // create array of serial numbers
            //    Dictionary<Socket, string>.ValueCollection valueColl =
            //        AsynchronousSocketListener.sernumdict.Values;
            //    string[] sernumarray = new string[AsynchronousSocketListener.sernumdict.Count];
            //    valueColl.CopyTo(sernumarray, 0);

            //    // create array of sockets
            //    Dictionary<Socket, string>.KeyCollection keyColl =
            //                AsynchronousSocketListener.sernumdict.Keys;
            //    Socket[] sockarray = new Socket[AsynchronousSocketListener.sernumdict.Count];
            //    keyColl.CopyTo(sockarray, 0);

            //    for (int x = 0; x < sernumarray.Length; x++)
            //    {
            //        // get socket associated with the selected serial number string and send current time message
            //        if (sernumarray[x] == dataGridView1.SelectedCells[0].Value.ToString())
            //        {
            //            foundsernum = sernumarray[x];
            //            foundsocket = sockarray[x];
            if (selid != "")
            {
                foundsocket = getSocket();
                Int32 unixTimecurr = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds; // get current Unix time
                string hexTimecurr = unixTimecurr.ToString("X");
                string data = "{VMC01," + selid + ",59,00," + hexTimecurr.ToString() + "}\r\n";
                //AsynchronousSocketListener_Outgoing("foundsocket = " + foundsocket.ToString());
                if (foundsocket != null)
                {
                    //MessageBox.Show("foundsocket = " + foundsocket.RemoteEndPoint.ToString()); // TEST
                    AsynchronousSocketListener.Send(foundsocket, data);
                }
                else
                {
                    AsynchronousSocketListener.AddUnsentMessage(selid, "\r\n" + data);
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
            else
                MessageBox.Show("Please select (click on) a unit from the serial number list");

            //}


            //}
            //}
        }

        private void btnReqVer_Click(object sender, EventArgs e) // send a version request message to the unit whose serial number is selected in the datagridview object
        {
            if (selid != "")
            {
                foundsocket = getSocket();
                string data = "{VMC01," + selid + ",53,01}\r\n";
                
                if (foundsocket != null)
                {
                    //MessageBox.Show("foundsocket = " + foundsocket.RemoteEndPoint.ToString()); // TEST
                    AsynchronousSocketListener.Send(foundsocket, data);
                }
                else
                {
                    AsynchronousSocketListener.AddUnsentMessage(selid, "\r\n" + data);
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
            else
                MessageBox.Show("Please select (click on) a unit from the serial number list");
        }

        private void CheckForNewUnits()
        {

            string query = "SELECT SerialNumber FROM [VLink106466].[dbo].VLinkUnit";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        SqlDataAdapter da = new SqlDataAdapter();
                        DataSet ds = new DataSet();
                        DataSet ds2 = new DataSet();
                        DataRow dr = null;
                        DataRow dr2 = null;


                        
                        // create dataset and datatable from returned data
                        da.SelectCommand = comm;
                        da.Fill(ds, "UnitTable");
                        da.Fill(ds2, "UnitTable");
                        dt = ds.Tables["UnitTable"];
                        dt2 = ds2.Tables["UnitTable"];

                        for (int x = dt.Rows.Count - 1; x > -1; x--)

                        {
                            //// create datatable containing only connected units
                            
                            // create datatable containe all available units
                            if (!AsynchronousSocketListener.sernumdict.ContainsValue(dt.Rows[x]["SerialNumber"].ToString()))
                            {
                                dr = dt.Rows[x];
                                //dt.Rows.Remove(dr);

                            }

                        }

                        //for (int x = dt2.Rows.Count - 1; x > -1; x--)

                        //{
                        //    // create datatable containing only non-connected units

                        //    if (AsynchronousSocketListener.sernumdict.ContainsValue(dt2.Rows[x]["SerialNumber"].ToString()))
                        //    {
                        //        dr2 = dt2.Rows[x];
                        //        dt2.Rows.Remove(dr2);

                        //    }

                        //}

                        BindingSource bSource = new BindingSource();
                        dataGridView1.Visible = true;

                        //// create combined datatable with connected units followed by non-connected units and use this table as the
                        // data source for the datagridview object
                        //for (int x = 0; x < dt2.Rows.Count; x++)
                        //    dt.ImportRow(dt2.Rows[x]);
                        bSource.DataSource = dt;

                        dataGridView1.DataSource = bSource;
                        bSource.Sort = "SerialNumber";

                    }
                }

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            AsynchronousSocketListener.CheckConnection(); // check if disconnection has occurred

            for (int x = 0; x < dataGridView1.RowCount; x++)
            {
                //if (x % 2 == 0)
                //    lstUnits2.SetItemChecked(x, true);
                //if (AsynchronousSocketListener.sernumdict.ContainsValue(lstUnits2.Items[x].ToString()))
                //    lstUnits2.SetItemChecked(x, true);
                //else if (!AsynchronousSocketListener.sernumdict.ContainsValue(lstUnits2.Items[x].ToString()))
                //    lstUnits2.SetItemChecked(x, false);

                // if socket connected (serial number still included in dictionary), display serial number in datagridview list
                // using bold and italic font
                if (AsynchronousSocketListener.sernumdict.ContainsValue(dataGridView1.Rows[x].Cells[0].Value.ToString()))
                    dataGridView1.Rows[x].Cells[0].Style.Font = new Font(dataGridView1.DefaultCellStyle.Font, FontStyle.Bold | FontStyle.Italic);

                //else if (!AsynchronousSocketListener.sernumdict.ContainsValue(lstUnits2.Items[x].ToString()))
                //    lstUnits2.SetItemChecked(x, false);

                // if socket disconnected (serial number no longer included in dictionary), display serial number in datagridview
                // list using regular font
                else if (!AsynchronousSocketListener.sernumdict.ContainsValue(dataGridView1.Rows[x].Cells[0].Value.ToString()))
                    dataGridView1.Rows[x].Cells[0].Style.Font = new Font(dataGridView1.DefaultCellStyle.Font, FontStyle.Regular);

                
            }

           


        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            CheckForNewUnits();
            //int rowIndex = -1;
            //foreach (DataGridViewRow row in dataGridView1.Rows)
            //{
            //    if (row.Cells[1].Value.ToString().Equals(selid))
            //    {
            //        rowIndex = row.Index;
            //        break;
            //    }
            //}

            
        }

        private void btnChgPort_Click(object sender, EventArgs e)
        {
            if (selid != "")
            {
                foundsocket = getSocket();
                Form ChgPortForm = new frmChgPort(); // display form for changing incoming port
                ChgPortForm.ShowDialog();
            }
            else
                MessageBox.Show("Please select (click on) a unit from the serial number list");

        }

        private void btnReboot_Click(object sender, EventArgs e)
        {
            if (selid != "")
            {
                foundsocket = getSocket();
                Form RebootForm = new frmReboot(); // display form for confirming reboot
                RebootForm.ShowDialog();
            }
            else
                MessageBox.Show("Please select (click on) a unit from the serial number list");
           
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            timer4.Enabled = true; // re-enable checking for new units when user clicks on an item in the list of units

            if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                selid = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                //if (AsynchronousSocketListener.sernumdict.ContainsValue(selid))
                    label4.Text = "Selected unit: " + selid;
                //else
                //{
                //    MessageBox.Show("Please select (click on) a unit from the serial number list");
                //    selid = "";
                //}

            }
        }

        private void btnFWUpdate_Click(object sender, EventArgs e)
        {
            if (selid != "")
            {
                AsynchronousSocketListener.fwreq2 = true;
                AsynchronousSocketListener.pos = 0;
                Socket foundsocket = getSocket();
                string data = "{VMC01," + selid + ",64,FF,FWUpdate}" + "\r\n";
                if (foundsocket != null)
                {
                    //MessageBox.Show("foundsocket = " + foundsocket.RemoteEndPoint.ToString()); // TEST
                    AsynchronousSocketListener.Send(foundsocket, data);
                }
                else
                {
                    AsynchronousSocketListener.AddUnsentMessage(selid, "\r\n" + data);
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
            else
                MessageBox.Show("Please select (click on) a unit from the serial number list");
        }

        private void btnClrIncoming_Click(object sender, EventArgs e)
        {
            rtbIncoming.Clear();
        }

        private void btnClrOutgoing_Click(object sender, EventArgs e)
        {
            rtbOutgoing.Clear();
        }

        private void dataGridView1_Scroll(object sender, ScrollEventArgs e)
        {
            // disable checking for new units while user scrolls through list of units
            timer4.Enabled = false;
        }

        
        private void dataGridView1_MouseLeave(object sender, EventArgs e)
        {
            // re-enable checking for new units when user moves mouse pointer outside datagridview listing units
            timer4.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            for (int x = 0; x < AsynchronousSocketListener.clientSockets.Count; x++)
                //MessageBox.Show(AsynchronousSocketListener.clientSockets[x].RemoteEndPoint.ToString());
                richTextBox1.Text = richTextBox1.Text + AsynchronousSocketListener.clientSockets[x].RemoteEndPoint.ToString() + "\r\n";
        }
    }   
}


        

