using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace TCPServer
{
    public partial class Form2 : Form
    {
        delegate void SetTextCallback(string text);
        public static IPAddress ipa;
        public static string verreqstr;

        public Form2()
        {
            InitializeComponent();
            lblStatus.Text = "Waiting for a connection...";
            
            
        }

        // based on https://msdn.microsoft.com/en-us/library/fx6588te%28v=vs.110%29.aspx

        public void FormLoad(object sender, EventArgs e)
        {
            StartListening();
            return;
        }

        //public class AsynchronousSocketListener
        //{

            //public static IPAddress ipa;
            //public static string verreqstr;
            //public delegate void ConnectionMadeEventHandler(Object sender, EventArgs e);

            // Thread signal.
            public static ManualResetEvent allDone = new ManualResetEvent(false);

            //public AsynchronousSocketListener()
            //{
            //}

                        
            public void StartListening()
            {
                // Data buffer for incoming data.
                byte[] bytes = new Byte[1024];

                // Establish the local endpoint for the socket.

                //IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                //IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPAddress ipAddress = IPAddress.Any;
                //ipAddress = IPAddress.Any;
                IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 52199);
                //ipa = localEndPoint.Address;




                // Create a TCP/IP socket.
                Socket listener = new Socket(AddressFamily.InterNetwork,
                        SocketType.Stream, ProtocolType.Tcp);

                // Bind the socket to the local endpoint and listen for incoming connections.
                try
                {
                    listener.Bind(localEndPoint);
                    listener.Listen(100);



                    //Form1 frm = new Form1();
                    //lblStatus.Text = "Waiting for a connection...";


                    while (true)
                    {
                        // Set the event to nonsignaled state.
                        allDone.Reset();

                        // Start an asynchronous socket to listen for connections.

                        // if (!frmloaded)
                        // {
                        //    Form1 frm = new Form1();
                        //    frmloaded = true;
                        //    frm.lblStatus.Text = "Waiting for a connection...";
                        //}

                        listener.BeginAccept(
                            new AsyncCallback(AcceptCallback),
                            listener);


                        // Wait until a connection is made before continuing.
                        allDone.WaitOne();
                    }

                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());

                }



            }

            public void AcceptCallback(IAsyncResult ar)
            {


                // Signal the main thread to continue.
                allDone.Set();
                //Form1 frm = new Form1();
                //frm.lblStatus.Text = "Connection received!";


                // Get the socket that handles the client request.
                Socket listener = (Socket)ar.AsyncState;
                Socket handler = listener.EndAccept(ar);

                // Get IP address of client connected to socket
                //ipa = IPAddress.Parse(((IPEndPoint)handler.RemoteEndPoint).Address.ToString());

                // Create the state object.
                StateObject state = new StateObject();
                state.workSocket = handler;
                handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);


                // Send version request
                byte[] verreq = System.Text.Encoding.ASCII.GetBytes("{53,01}" + "\r\n");
                verreqstr = System.Text.Encoding.ASCII.GetString(verreq);
                Send(handler, verreqstr);
                //SendVerReq();
                
            
                
            }


            private void SendVerReq()
            {
                this.BeginInvoke(new SetTextCallback(SetText), new object[] { verreqstr });
            }
            Form1 frm = new Form1();
            //frm.CreateHandle();
            //frm.CreateControl();

            //RichTextBox rtbout = new RichTextBox();
            //rtbout.CreateControl();
            //rtbOutgoing.AppendText(verreqstr);
            //frm.BeginInvoke(new SetTextCallback(frm.SetText), new object[] { verreqstr });
            //frm.Show();
            //rtbout.BeginInvoke(new SetTextCallback(frm.SetText), new object[] { verreqstr });



            public static void ReadCallback(IAsyncResult ar)
            {
                String content = String.Empty;

                // Retrieve the state object and the handler socket
                // from the asynchronous state object.
                StateObject state = (StateObject)ar.AsyncState;
                Socket handler = state.workSocket;

                // Get IP address of client connected to socket
                ipa = IPAddress.Parse(((IPEndPoint)handler.RemoteEndPoint).Address.ToString());

                // Read data from the client socket. 
                int bytesRead = handler.EndReceive(ar);

                if (bytesRead > 0)
                {
                    // There  might be more data, so store the data received so far.
                    state.sb.Append(Encoding.ASCII.GetString(
                        state.buffer, 0, bytesRead));

                    // Check for end of data tag. If it is not there, read 
                    // more data.

                    content = ipa.ToString() + " " + state.sb.ToString();

                    if (content.IndexOf("}\r") > -1)
                    {
                        // All the data has been read from the 
                        // client. Display it on the console.
                        Form1 frm = new Form1();
                        //frm.rtbIncoming.AppendText(content);
                        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReadCallback), state);
                        //// Echo the data back to the client.
                        //Send(handler, content);
                        state.sb.Clear();



                    }
                    else
                    {
                        // Not all data received. Get more.
                        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReadCallback), state);
                    }
                }
            }

            public static void Send(Socket handler, String data)
            {
                // Convert the string data to byte data using ASCII encoding.
                byte[] byteData = Encoding.ASCII.GetBytes(data);

                // Begin sending the data to the remote device.
                handler.BeginSend(byteData, 0, byteData.Length, 0,
                    new AsyncCallback(SendCallback), handler);

                
            }

            private static void SendCallback(IAsyncResult ar)
            {
                try
                {
                    // Retrieve the socket from the state object.
                    Socket handler = (Socket)ar.AsyncState;

                    // Complete sending the data to the remote device.


                    //handler.Shutdown(SocketShutdown.Both);
                    //handler.Close();

                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }

        public void SetText(string text)
        {
            rtbOutgoing.AppendText(text);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            FormLoad(null,null);
        }
        //}
    }
}
