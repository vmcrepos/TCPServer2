﻿#define GREGG

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using System.Collections;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Net.Mail;
using System.Linq;
using System.Data.SqlTypes;


namespace TCPServer2
{
    // based on https://msdn.microsoft.com/en-us/library/fx6588te%28v=vs.110%29.aspx

    public class AsynchronousSocketListener
    {
        public delegate void dta(object sender, string Data);
        public static event dta Incoming;

        public delegate void dtaOut(string Data);
        public static event dtaOut Outgoing;

        public static IPAddress ipa;
        public static string verreqstr;
        public delegate void conn(string IPAddress);
        public static event conn Connected;
        public static Boolean verrqstd = false;
        //public static string connectionString = @"Server=P4469916\VLINKDB; Initial Catalog=VLink106466; User=vlink; Password='TestVlink'; Integrated Security=True";
        public static string connectionString = @"Server=P6230003\VLINKDB; Initial Catalog=VLink106466; User=VLink106466; Password='GreenCo2017'; Integrated Security=False";
        //public static string connectionString = @"Server=P4469916\VLINKDB; Initial Catalog=VLink_GreenCo; User=vlink; Password='TestVlink'; Integrated Security=True";
        //public static string connectionString = @"Server=VMCIS\SQLEXPRESS; Initial Catalog=VLink; User=VLink; Password='TestVlink'; Integrated Security=True";
        //public static string connectionString = @"Server=207.198.117.37\VMCIS\SQLEXPRESS; Initial Catalog=VLink; User=VLink; Password='TestVlink'; Integrated Security=True";
        public static Int32 unitid;
        //public static Dictionary<Socket, Int32> units = new Dictionary<Socket, int>();
        public static Dictionary<Int32, Socket> units2 = new Dictionary<int, Socket>();
        public static Dictionary<int, int> intsensorvals = new Dictionary<int, int>();
        public static Dictionary<int, string> stringsensorvals = new Dictionary<int, string>();
        public static byte[] fwbytes2;
        public static bool fwreq = false;
        public static bool fwreq2 = false;
        public static bool checkact = false;
        public static bool settimer = false;
        static DataTable dt = new DataTable();
        static DataTable dt2 = new DataTable();
        static DataTable dtalarm = new DataTable();
        static Socket handler;
        public static Dictionary<int, int> pendingactions = new Dictionary<int, int>();
        public static bool actisoreq = false;
        public static ArrayList actisoreqlist = new ArrayList();
        public static ArrayList actsetoutlist = new ArrayList();
        //public static ArrayList actsetaout = new ArrayList();
        public static bool actsetout = false;
        public static int actionidreq;
        public static int actionidseta;
        public static int actionidsetd;
        public static Dictionary<string, int> ackaction = new Dictionary<string, int>();
        static DataSet ds = new DataSet();
        static DataSet ds2 = new DataSet();
        //static IPAddress foundip;
        static Socket foundsocket;
        static Socket foundsocket2;
        public static List<Socket> clientSockets = new List<Socket>();
        static int remoteport;
        public static Dictionary<int, string> actisoreqdict = new Dictionary<int, string>();
        public static Dictionary<int, string> actsetoutdict = new Dictionary<int, string>();
        //static Socket handler2;
        //public static ArrayList alarmlist = new ArrayList();
        //public static ArrayList alarmlist2 = new ArrayList();
        public static Dictionary<int, int> alarmsensor = new Dictionary<int, int>();
        public static Dictionary<int, double> alarmlow = new Dictionary<int, double>();
        public static Dictionary<int, double> alarmhigh = new Dictionary<int, double>();
        public static Dictionary<int, int> almidsensid = new Dictionary<int, int>();
        public static Dictionary<int, int> alarmunit = new Dictionary<int, int>();
        public static int retalarmid;
        static DataTable dt3 = new DataTable();
        static DataTable dt4 = new DataTable();
        static DataTable dt5 = new DataTable();
        static DataTable dt6 = new DataTable();
        public static ArrayList almlowlimits = new ArrayList();
        public static ArrayList almhighlimits = new ArrayList();
        public static ArrayList actalarmid = new ArrayList();
        public static int[] actalmsensarray;
        public static ArrayList almsensors = new ArrayList();
        public static ArrayList almids = new ArrayList();
        //public static Dictionary<IPAddress, bool> disclogmod = new Dictionary<IPAddress, bool>();
        public static Dictionary<Socket, bool> disclogmod = new Dictionary<Socket, bool>();
        //public static Dictionary<IPAddress, bool> outlogmod = new Dictionary<IPAddress, bool>();
        public static Dictionary<Socket, bool> outlogmod = new Dictionary<Socket, bool>();
        //public static Dictionary<IPAddress, bool> inlogmod = new Dictionary<IPAddress, bool>();
        public static Dictionary<Socket, bool> outlogmod2 = new Dictionary<Socket, bool>();
        public static Dictionary<Socket, bool> inlogmod = new Dictionary<Socket, bool>();
        public static Dictionary<Socket, bool> inlogmod2 = new Dictionary<Socket, bool>();
        //public static Dictionary<IPAddress, string> curraction = new Dictionary<IPAddress, string>();
        public static Dictionary<Socket, string> curraction = new Dictionary<Socket, string>();
        public static Dictionary<string, string> curraction2 = new Dictionary<string, string>();
        //public static Dictionary<Socket, string> sernumdict = new Dictionary<Socket, string>();
        public static Dictionary<string, Socket> sernumdict2 = new Dictionary<string, Socket>();
        public static Dictionary<int, bool> modesetdict = new Dictionary<int, bool>();
        public static Dictionary<int, bool> intervalsetdict = new Dictionary<int, bool>();
        public static string sernum = String.Empty;
        static int actiontoupdate = 0;
        static bool fwackwait = false;
        static bool fwackrec = false;
        static double diffInSeconds = 0;
        //static int count = 0;
        //static int pos = 1;
        public static int pos = 0;
        static DateTime start;
        static DateTime current;
        static string content2 = String.Empty;
        static string OpMode = String.Empty;
        static string Interval = String.Empty;
        static bool modeset = false;
        static bool timeset = false;
        static bool interset = false;
        static bool initreq = false;
        public static bool fwstart = false;
        public static string port = "";
        static string fwnamestr = "";
        static bool fwfirst = false;
        //static string retbuff;
        static int nakcount;
        static Dictionary<int, int> setoutsentcount = new Dictionary<int, int>();
        static Dictionary<int, int> outcyclecount = new Dictionary<int, int>();
        static Dictionary<int, bool> outcyclereset = new Dictionary<int, bool>();
        static ArrayList unsentarr = new ArrayList();
        public static ArrayList sernumlist = new ArrayList();
        static bool packproc = true;
        private static IAsyncResult currAsyncResult;
        static bool actionschkd = true;
        static bool logfinished = true;
        //static StateObject state = new StateObject();
        static bool msgsent = false;




        // Thread signal.
        public static ManualResetEvent allDone = new ManualResetEvent(false);
        static Thread t;
        //Form1 fm;
        //AsynchronousSocketListener ls;

        public static void Startup()
        {
            Outgoing += AsynchronousSocketListener_Outgoing;
            Incoming += AsynchronousSocketListener_Incoming;
            Connected += AsynchronousSocketListener_Connected;
            t = new Thread((System.Threading.ThreadStart)ThreadCallback);
            t.Start();
        }

        private static void AsynchronousSocketListener_Outgoing(string Data)
        {
            // throw new NotImplementedException();



        }

        private static void AsynchronousSocketListener_Connected(string IPAddress)
        {
            //throw new NotImplementedException();


        }

        public static void ThreadCallback()
        {

            TCPServer2.AsynchronousSocketListener.StartListening();
        }
        public AsynchronousSocketListener()
        {
            //ls = this;

            //fm = frmMain;


            new Thread(delegate () { TCPServer2.AsynchronousSocketListener.StartListening(); }).Start();

        }

        private static void AsynchronousSocketListener_Incoming(object sender, string Data)
        {
            //   throw new NotImplementedException();
        }

        public static void StartListening()
        {
            // Data buffer for incoming data.
            byte[] bytes = new Byte[1024];

            // Establish the local endpoint for the socket.

            //IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            //IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPAddress ipAddress = IPAddress.Any;
            //ipAddress = IPAddress.Any;
#if DON
                IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 52100);
#endif
#if GREGG
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 52199);
#endif
#if TEST
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 52000);
#endif
            //ipa = localEndPoint.Address;
            port = ((IPEndPoint)localEndPoint).Port.ToString();





            // Create a TCP/IP socket.
            Socket listener = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(2000);



                //Form1 frm = new Form1();
                //lblStatus.Text = "Waiting for a connection...";


                while (true)
                {
                    // Set the event to nonsignaled state.
                    allDone.Reset();

                    // Start an asynchronous socket to listen for connections.

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

        public static void AcceptCallback(IAsyncResult ar)
        {

           
            // Signal the main thread to continue.
            allDone.Set();

            
            // Get the socket that handles the client request.
            Socket listener = (Socket)ar.AsyncState;
            //Socket handler = listener.EndAccept(ar);
            handler = listener.EndAccept(ar);
            handler.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);
            //if (clientSockets.Contains(handler))
            //    clientSockets.Remove(handler);

            //clientSockets.Add(handler); // list of connected sockets
            //Thread.Sleep(1000);

            
            // Get IP address of client connected to socket
            IPAddress ipa = IPAddress.Parse(((IPEndPoint)handler.RemoteEndPoint).Address.ToString());
            Connected(ipa.ToString());
            

            // Create the state object.
            StateObject state = new StateObject();
            state.workSocket = handler;
            //ar = currAsyncResult;
            //currAsyncResult = ar;
            //currAsyncResult = handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
            //    new AsyncCallback(ReadCallback), state);
            //if (ar == currAsyncResult)
            //    MessageBox.Show("Yes, ar = currAsyncResult");
            //else if (ar != currAsyncResult)
            //    MessageBox.Show("No, ar != currAsyncResult");
            //if (currAsyncResult == ar)
            //    MessageBox.Show("Yes, currAsyncResult = ar");
            //else if (currAsyncResult != ar && currAsyncResult != null)
            //    MessageBox.Show("No, currAsyncResult != ar");
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);

        }



        public static void CheckActions(string sn)
        {
            unitid = GetUnitIDFromSN(sn);

            if (unitid != 0)

            {
                // move new actions from "ActionConfig" table to "Actions" table

                string query = "EXEC proc_checkforaction @unitid";

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        using (SqlCommand comm = new SqlCommand(query, conn))
                        {


                            conn.Open();
                            comm.Parameters.AddWithValue("@unitid", unitid);

                            try
                            {
                                Int32 response = Convert.ToInt32(comm.ExecuteScalar());

                            }
                            catch (Exception e)
                            {
                                MessageBox.Show(e.ToString());
                            }

                        }
                    }


                }

                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }



                // get action items where "Pending Time" and "Completed Time" are null

                string query2 = "EXEC proc_getactions @username, @unitid, @status, @start, @end";

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        using (SqlCommand comm = new SqlCommand(query2, conn))
                        {

                            conn.Open();
                            comm.Parameters.AddWithValue("@username", DBNull.Value);
                            comm.Parameters.AddWithValue("@unitid", unitid);
                            comm.Parameters.AddWithValue("@status", 0);
                            comm.Parameters.AddWithValue("@start", DBNull.Value);
                            comm.Parameters.AddWithValue("@end", DBNull.Value);






                            // create dataset and datatable from returned data
                            SqlDataAdapter da = new SqlDataAdapter();
                            dt.Clear();
                            dt2.Clear();
                            da.SelectCommand = comm;
                            da.Fill(ds, "ActionTable");
                            dt = ds.Tables["ActionTable"];
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }

                // get action items where "Completed Time" is null

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        using (SqlCommand comm2 = new SqlCommand(query2, conn))
                        {

                            conn.Open();
                            comm2.Parameters.AddWithValue("@username", DBNull.Value);
                            comm2.Parameters.AddWithValue("@unitid", unitid);
                            comm2.Parameters.AddWithValue("@status", 1);
                            comm2.Parameters.AddWithValue("@start", DBNull.Value);
                            comm2.Parameters.AddWithValue("@end", DBNull.Value);

                            // create dataset and datatable from returned data
                            SqlDataAdapter da2 = new SqlDataAdapter();

                            da2.SelectCommand = comm2;
                            da2.Fill(ds2, "ActionTable2");
                            dt2 = ds2.Tables["ActionTable2"];
                            dt.Merge(dt2); // merge data tables
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }

                 
               
                    foreach (DataRow dr in dt.Rows)
                   
                    {
                        // scan data table for sensor data request actions

                        if (Convert.ToInt32(dr["ActionType"]) == 1 && (dr["PendingTime"] == DBNull.Value ||
                             dr["PendingTime"].ToString() == "1/1/1970 12:00:00 AM" || dr["CompleteTime"] == DBNull.Value))

                        {
                            actionidreq = Convert.ToInt32(dr["id"]); // action id
                            string packetid = (actionidreq % 255).ToString("X"); // create packet id from action id (packet id must be a single hex byte)
                            if (packetid.Length == 1)
                                packetid = "0" + packetid;
                            byte[] isoreq = System.Text.Encoding.ASCII.GetBytes("{VMC01," + sn + ",66," + packetid + ",00}\r\n"); // create request message string using packet id
                            string isoreqstr = System.Text.Encoding.ASCII.GetString(isoreq);

                            if (dr["PendingTime"] == DBNull.Value)
                            {
                                AddUnsentMessage(sn, "\r\n" + isoreqstr); // add data request message to list of unsent messages
                                SendCurrTime(sn);   // send current time message to ping unit and see if it is available
                            }

                            if (!pendingactions.ContainsKey(unitid))
                                pendingactions.Add(unitid, actionidreq);    // add action id to dictionary of pending actions for the current unit

                            actisoreqlist.Add(actionidreq); // add action id to list of active request action ids
                            if (!ackaction.ContainsKey(packetid))
                                ackaction.Add(packetid, actionidreq); // add dictionary item linking packet id with action id



                            UpdateAction(actionidreq, "actisoreq"); // update action database table for appropriate action id
                        }

                        // scan data table for "set digital output" actions
                        if (Convert.ToInt32(dr["ActionType"]) == 10 && dr["CompleteTime"] == DBNull.Value)
                        {
                            string setting = dr["Setting"].ToString(); // setting string for output
                            actionidsetd = Convert.ToInt32(dr["id"]); // action id
                            string packetid = (actionidsetd % 255).ToString("X"); // create packet id from action id (packet id must be a single hex byte)
                            if (packetid.Length == 1)
                                packetid = "0" + packetid;
                            byte[] setdoutput = System.Text.Encoding.ASCII.GetBytes("{VMC01," + sernum + ",65," + packetid + "," + setting + ",00,00}\r\n"); // create "set output" message string using packet id and setting string
                            string setdoutputstr = System.Text.Encoding.ASCII.GetString(setdoutput);
                            bool match = false;
                            bool resetcycle = false;
                            int cyclecount;

                            if (outcyclereset.ContainsKey(actionidsetd) && outcyclecount.ContainsKey(actionidsetd))
                            {
                                outcyclereset.TryGetValue(actionidsetd, out resetcycle);
                                outcyclecount.TryGetValue(actionidsetd, out cyclecount);
                                if (resetcycle)
                                    outcyclecount[actionidsetd] = 1;
                                else
                                    outcyclecount[actionidsetd] = cyclecount + 1;
                            }
                            if (!outcyclecount.ContainsKey(actionidsetd))
                                outcyclecount.Add(actionidsetd, 1);
                            if (!outcyclereset.ContainsKey(actionidsetd))
                                outcyclereset.Add(actionidsetd, false);

                            if (!setoutsentcount.ContainsKey(actionidsetd))
                            {
                                setoutsentcount.Add(actionidsetd, 0);
                            }
                            int outcount = 0;
                            if (setoutsentcount.ContainsKey(actionidsetd))
                                setoutsentcount.TryGetValue(actionidsetd, out outcount);
                            int cyclecount2 = 0;
                            if (outcyclecount.ContainsKey(actionidsetd))
                                outcyclecount.TryGetValue(actionidsetd, out cyclecount2);
                            if (outcyclereset.ContainsKey(actionidsetd))
                                outcyclereset[actionidsetd] = false;

                            //if (outcount < 10 && cyclecount2 % 6 == 0)
                            //{
                            //Send(handler2, setdoutputstr);
                            if (dr["PendingTime"] == DBNull.Value)
                            {
                                AddUnsentMessage(sn, "\r\n" + setdoutputstr);   // add set output message to list of unsent messages
                                SendCurrTime(sn);   // send current time message to ping unit and see if it is available
                            }
                            //     setoutsentcount[actionidsetd] = outcount + 1;
                            // }
                            // else if (outcount == 10)
                            // {
                            //     setoutsentcount[actionidsetd] = 0;
                            //     string data = "{VMC01," + sernum + ",70,00,REBOOT}\r\n";
                            //     //Send(handler2, data);
                            //     AddUnsentMessage(sn, "\r\n" + data);
                            //     SendCurrTime(sn);

                            // }
                            //match = false;
                            //}

                            //if (!outlogmod2.ContainsKey(handler2))
                            //    outlogmod2.Add(handler2, false);
                            //else
                            //{
                            //    outlogmod2.Remove(handler2);
                            //    outlogmod2.Add(handler2, false);
                            //}

                            //if (!curraction.ContainsKey(handler2))
                            //    curraction.Add(handler2, "actsetout");
                            //else
                            //{
                            //    curraction.Remove(handler2);
                            //    curraction.Add(handler2, "actsetout");
                            //}

                            //handler2 = null;
                            //if (!curraction.ContainsKey(handler))
                            //    curraction.Add(handler, "actsetout");
                            //else
                            //{
                            //    curraction.Remove(handler);
                            //    curraction.Add(handler, "actsetout");
                            //}

                            if (!curraction2.ContainsKey(sn))
                                curraction2.Add(sn, "actsetout");   // add action to dictionary of current actions
                            else
                            {
                                curraction2.Remove(sn);
                                curraction2.Add(sn, "actsetout");
                            }
                            //actsetout = true; // boolean indicating that a "set output" action is active
                            actsetoutlist.Add(actionidsetd); // add action id to list of active "set output" action ids
                            if (!ackaction.ContainsKey(packetid))
                                ackaction.Add(packetid, actionidsetd); // add dictionary item linking packet id with action id
                            UpdateAction(actionidsetd, "actsetout"); // update "Pending Time" in database action table

                        }

                        // scan data table for "set analog output" actions

                        if (Convert.ToInt32(dr["ActionType"]) == 11 && dr["CompleteTime"] == DBNull.Value)
                        {
                            string setting = dr["Setting"].ToString(); // setting string for output
                            actionidseta = Convert.ToInt32(dr["id"]); // action id
                            string packetid = (actionidseta % 255).ToString("X"); // create packet id from action id (packet id must be a single hex byte)
                            if (packetid.Length == 1)
                                packetid = "0" + packetid;
                            byte[] setaoutput = System.Text.Encoding.ASCII.GetBytes("{VMC01," + sernum + ",65," + packetid + "," + setting + ",00,00}\r\n"); // create "set output" message string using packet id and setting string
                            string setaoutputstr = System.Text.Encoding.ASCII.GetString(setaoutput);

                            bool resetcycle = false;
                            int cyclecount;

                            if (outcyclereset.ContainsKey(actionidseta) && outcyclecount.ContainsKey(actionidseta))
                            {
                                outcyclereset.TryGetValue(actionidseta, out resetcycle);
                                outcyclecount.TryGetValue(actionidseta, out cyclecount);
                                if (resetcycle)
                                    outcyclecount[actionidseta] = 1;
                                else
                                    outcyclecount[actionidseta] = cyclecount + 1;
                            }
                            if (!outcyclecount.ContainsKey(actionidseta))
                                outcyclecount.Add(actionidseta, 1);
                            if (!outcyclereset.ContainsKey(actionidseta))
                                outcyclereset.Add(actionidseta, false);

                            //Socket handler2 = new Socket(AddressFamily.InterNetwork,
                            //    SocketType.Stream, ProtocolType.Tcp); // socket to send message
                            //                                          //int connectid = Convert.ToInt32(dr["UnitID"]);

                            //// create array of unit IDs
                            //Dictionary<Socket, int>.ValueCollection valueColl =
                            //    units.Values;
                            //int[] unitarray = new int[units.Count];
                            //valueColl.CopyTo(unitarray, 0);

                            //// create array of sockets
                            //Dictionary<Socket, int>.KeyCollection keyColl =
                            //            units.Keys;
                            //Socket[] addarray = new Socket[units.Count];
                            //keyColl.CopyTo(addarray, 0);

                            //for (int x = 0; x < unitarray.Length; x++)
                            //{
                            //    // get socket associated with the current unit id
                            //    if (unitarray[x] == unitid)
                            //        foundsocket = addarray[x];

                            //}

                            //for (int y = 0; y < clientSockets.Count; y++)
                            //{
                            //    if (foundsocket.RemoteEndPoint.ToString() == clientSockets[y].RemoteEndPoint.ToString())
                            //    {
                            //        // socket associated with IP address of unit id
                            //        handler2 = clientSockets[y];

                            //    }

                            //}

                            // send request message using socket
                            //if (SocketExtensions.IsConnected(handler2))
                            //{
                            //    //Send(handler2, setaoutputstr);

                            if (!setoutsentcount.ContainsKey(actionidseta))
                            {
                                setoutsentcount.Add(actionidseta, 0);
                            }
                            int outcount = 0;
                            if (setoutsentcount.ContainsKey(actionidseta))
                                setoutsentcount.TryGetValue(actionidseta, out outcount);
                            int cyclecount2 = 0;
                            if (outcyclecount.ContainsKey(actionidseta))
                                outcyclecount.TryGetValue(actionidseta, out cyclecount2);
                            if (outcyclereset.ContainsKey(actionidseta))
                                outcyclereset[actionidseta] = false;

                            //    if (outcount < 10 && cyclecount2 % 6 == 0)
                            //    {
                            //        Send(handler2, setaoutputstr);
                            //        setoutsentcount[actionidseta] = outcount + 1;
                            //    }

                            if (dr["PendingTime"] == DBNull.Value)
                            {
                                AddUnsentMessage(sn, "\r\n" + setaoutputstr); // add set output message to list of unsent messages
                                SendCurrTime(sn);   // send current time message to ping unit and see if it is available
                            }
                            //    else if (outcount == 10)
                            //    {
                            //        setoutsentcount[actionidseta] = 0;
                            //        string data = "{VMC01," + sernum + ",70,00,REBOOT}\r\n";
                            //        Send(handler2, data);

                            //    }
                            //}

                            //if (!outlogmod2.ContainsKey(handler2))
                            //    outlogmod2.Add(handler2, false);
                            //else
                            //{
                            //    outlogmod2.Remove(handler2);
                            //    outlogmod2.Add(handler2, false);
                            //}

                            //if (!curraction.ContainsKey(handler2))
                            //    curraction.Add(handler2, "actsetout");
                            //else
                            //{
                            //    curraction.Remove(handler2);
                            //    curraction.Add(handler2, "actsetout");
                            //}
                            //handler2 = null;
                            //actisoreq = true;
                            //if (!curraction.ContainsKey(handler))
                            //    curraction.Add(handler, "actsetout");
                            //else
                            //{
                            //    curraction.Remove(handler);
                            //    curraction.Add(handler, "actsetout");
                            //}

                            if (!curraction2.ContainsKey(sn))
                                curraction2.Add(sn, "actsetout");   // add action to dictionary of current actions
                            else
                            {
                                curraction2.Remove(sn);
                                curraction2.Add(sn, "actsetout");
                            }
                            //actsetout = true; // boolean indicating that a "set output" action is active
                            actsetoutlist.Add(actionidseta); // add action id to list of active "set output" action ids
                            if (!ackaction.ContainsKey(packetid))
                                ackaction.Add(packetid, actionidseta); // add dictionary item linking packet id with action id
                            UpdateAction(actionidseta, "actsetout"); // update "Pending Time" in database action table



                        }
                    }



                }

            actionschkd = true;

                
                
            

        }

        // this method updates the "Pending Time" data for an item in the database action table

        public static void UpdateAction(int actid, string type)
        {
            string query = "EXEC proc_updateaction @id, @pending, @complete";
            DateTime pending = DateTime.Now;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {

                        conn.Open();
                        comm.Parameters.AddWithValue("@id", actid);
                        comm.Parameters.AddWithValue("@pending", pending);
                        comm.Parameters.AddWithValue("@complete", DBNull.Value);

                        try
                        {
                            Int32 response = Convert.ToInt32(comm.ExecuteScalar());

                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.ToString());
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            



        }

        // this method updates the "Completed Time" data for an item in the database action table

        public static void UpdateAction2(int actid, string type)
        {
            string query = "EXEC proc_updateaction @id, @pending, @complete";

            DateTime complete = DateTime.Now;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {

                        conn.Open();
                        comm.Parameters.AddWithValue("@id", actid);
                        comm.Parameters.AddWithValue("@pending", DBNull.Value);
                        comm.Parameters.AddWithValue("@complete", complete);

                        try
                        {
                            Int32 response = Convert.ToInt32(comm.ExecuteScalar());
                            //MessageBox.Show("response = " + response.ToString());
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.ToString());
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

            if (actsetoutlist.Contains(actid))
            {
                actsetoutlist.Remove(actid); // remove action id for a completed "Set Output" action item from the list\

            }

            if (actsetoutlist.Count == 0)
                actsetout = false; // if list is empty, set boolean to false (no active "Set Output" action items)

            if (actisoreqlist.Contains(actid))
            {
                actisoreqlist.Remove(actid); // remove action id for a completed "Sensor Data Request" action item from the list

            }

            if (actisoreqlist.Count == 0)
                actisoreq = false; // if list is empty, set boolean to false (no active "Sensor Data Request" action items)

            


        }
        
        public static void FWSend()
        {
            // Firmware update requested
            if (fwreq)
            {
                Thread.Sleep(1000);
                byte[] fwreqarr = System.Text.Encoding.ASCII.GetBytes("{VMC01," + sernum + ",64,FF,FWUpdate}" + "\r\n");
                string fwreqstr = System.Text.Encoding.ASCII.GetString(fwreqarr);
                
                fwreq2 = true;
                if (SocketExtensions.IsConnected(handler))
                    Send(handler, fwreqstr);
            }
            //http://codetechnic.blogspot.com/2009/02/sending-large-files-over-tcpip.html
        }

        public static void FWSendFile() // get firmware file name from ini file
        {

            
            try
            {
                //http://stackoverflow.com/questions/12087145/file-transfer-using-sockets-c-received-file-doesnt-contain-full-data

                // open firmware file for reading
                
                if (File.Exists("C:\\ProgramData\\TCPServer\\TCPServer.ini")) // check if config file exists
                {

                    using (FileStream fs = File.OpenRead("C:\\ProgramData\\TCPServer\\TCPServer.ini")) // open config file
                    {
                        
                        var file = new System.IO.StreamReader(fs, System.Text.Encoding.UTF8);
                        fwnamestr = file.ReadLine(); // get name of config file
#if TEST
                        //fwnamestr = "D:\\testfile.txt";
                        fwnamestr = "D:\\" + fwnamestr;
#else
                        fwnamestr = "D:\\" + fwnamestr;
#endif

                        file.Dispose();
                        
                    }
                }

                else
                    MessageBox.Show("Configuration file not found");
            }

            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

            FWSendFile2(); // call function to send firmware file


        }

        public static void FWSendFile2() // send firmware file
        {


            if (File.Exists(fwnamestr))
            {
                using (FileStream fs2 = File.OpenRead(fwnamestr)) // open firmware file
                                                                  //using (FileStream fs2 = File.OpenRead("D:\\FLXtest1.bin"))

                {
                    

                    //https://stackoverflow.com/questions/3967541/how-to-split-large-files-efficiently

                    //break up file into 512-byte chunks

                    byte[] buffer = new byte[514]; //set the size of firmware file chunk (includes checksum and footer bytes)
                    byte[] header = new byte[1];
                    header[0] = Convert.ToByte("AA", 16); // firmware header byte




                    
                    start = DateTime.Now; // record time when chunk is sent
                    diffInSeconds = -1;
                    
                    fs2.Position = pos; // increment start position for next chunk to be sent
                    while (fs2.Position < fs2.Length)
                    {
                        // read next 512 bytes
                        int chunkBytesRead = 0;
                        while (chunkBytesRead < 512)

                        {
                            int bytesRead = fs2.Read(buffer,
                                                       chunkBytesRead,
                                                       512 - chunkBytesRead);

                            if (bytesRead == 0)
                            {
                                fwstart = false;
                                break;
                            }
                            chunkBytesRead += bytesRead;
                        }

                        
                        if (fwstart && fwackrec) // acknowledgement received; send next chunk
                        {

                            // calculate checksum (https://stackoverflow.com/questions/12942904/calculate-twos-complement-checksum-of-hexadecimal-string)

                            int chkSum = buffer.Aggregate(0, (s, b) => s += b) & 0xff; // sum all data bytes 
                            chkSum = (0x100 - chkSum) & 0xff; // twos complement calculation
                            string chkSumhex = chkSum.ToString("X"); // convert decimal result to hex string
                            byte chkSumByte = Convert.ToByte(chkSumhex, 16); // convert hex string to  checksum byte

                            buffer[512] = Convert.ToByte("55", 16); // add footer byte
                            buffer[513] = chkSumByte; // add checksum byte

                            handler.Send(header); // send header byte
                            handler.Send(buffer, 514, SocketFlags.None); // send chunk + footer and checksum bytes
                            
                            fwackwait = true;
                            fwackrec = false;
                            
                            Thread.Sleep(1000);
                           
                        }
                        else if (!fwackrec && !fwackwait && fwstart) // acknowledgement not received; terminate transfer
                                                                     
                        {
                            fwstart = false;
                            fs2.Dispose();
                            break;
                        }

                       
                    }


                    
                }
            }

            else
                MessageBox.Show("Firmware update file not found");


        }

        public static void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;
            content2 = String.Empty;
            String content3 = String.Empty;
            ArrayList indata = new ArrayList();
            ArrayList indata2 = new ArrayList();
            SocketError errorCode = SocketError.Success;
            int bytesRead = 0;
            //StateObject state = (StateObject)ar.AsyncState;
            msgsent = false;

            try
            {


                // Retrieve the state object and the handler socket
                // from the asynchronous state object.

                //currAsyncResult = ar;
                //ar = currAsyncResult;
                StateObject state = (StateObject)ar.AsyncState;
                //state = (StateObject)ar.AsyncState;
                //Socket handler = state.workSocket;
                handler = state.workSocket;


                // Get IP address of client connected to socket
                ipa = IPAddress.Parse(((IPEndPoint)handler.RemoteEndPoint).Address.ToString());

                // Read data from the client socket. 
                // http://stackoverflow.com/questions/2582036/an-existing-connection-was-forcibly-closed-by-the-remote-host


                try
                {
                    //if (ar == currAsyncResult)
                    //{
                    //    //MessageBox.Show("ar = currAsyncResult");
                    //    //SocketError errorCode;
                        bytesRead = handler.EndReceive(ar, out errorCode);
                    //    //MessageBox.Show("bytesRead = " + bytesRead.ToString());
                    //}
                    //else
                    //{
                    //    //ignore;
                    //}
                }

                catch (Exception ex)
                {
                    //Log exception. Don't throw exception. Most probably BeginReceive failed.
                    Outgoing(ex.ToString());
                }
                //}
                //else
                //{
                // ignore;
                //    MessageBox.Show("Socket change required");
                //}
                //}

                //catch (Exception ex)
                //{
                //    //Log exception. Don't throw exception. Most probably BeginReceive failed.
                //    Outgoing(ex.ToString());
                //}




                if (errorCode != SocketError.Success)
                {
                    bytesRead = 0;
                }

                if (bytesRead > 0)
                {

                    // There might be more data, so store the data received so far.
                    state.sb.Append(Encoding.ASCII.GetString(
                        state.buffer, 0, bytesRead));

                    // Check for end of data tag. If it is not there, read 
                    // more data.

                    content2 = "";
                    content2 = state.sb.ToString();

                    if (content2.IndexOf("}") > -1)
                    {
                        // All the data has been read from the 
                        // client

                        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                            new AsyncCallback(ReadCallback), state);
                        state.sb.Clear();

                        content2 = content2.Substring(0, content2.IndexOf("}") + 1);


                        //if (content2.Contains("VMC01,") && content2.EndsWith("}")) // received VLink message
                        if (content2.Contains("VMC01,")) // received VLink message

                       
                        {
                            string content2a = "";
                            content2a = content2.TrimEnd('}');
                            indata.Clear();
                            indata.AddRange(content2a.Split(',')); // split response at delimiters and store elements in arraylist
                            sernum = indata[1].ToString(); // extract serial number from message



                            Int32 response = 0;
                            Int32 response2 = 0;


                            if (!sernumlist.Contains(sernum)) // unit serial number not found in list of units
                            {
                                //MessageBox.Show("serial number  = " + sernum + " Did not recognize serial number"); // test
                                Outgoing("Did not recognize serial number -- disconnecting");
                                try
                                {

                                    sernum = "";
                                    handler.Shutdown(SocketShutdown.Both); // disconnect

                                }
                                catch (ObjectDisposedException e)
                                {
                                    MessageBox.Show("1621 " + e.ToString());
                                }

                            }
                            else // unit serial number found in list
                            {
                                response = GetUnitIDFromSN(sernum);

                                try
                                {
                                    units2.Remove(response);
                                    units2.Add(response, handler); // add entry for unit ID to units2 dictionary

                                    sernumdict2.Remove(sernum);
                                    sernumdict2.Add(sernum, handler); // add entry for unit in the dictionary of actively connected units (IP address and unit id)

                                    if (!modesetdict.ContainsKey(response))
                                        modesetdict.Add(response, false); // add entry to dictionary indicating that mode command has not been sent to this unit
                                    if (!intervalsetdict.ContainsKey(response))
                                        intervalsetdict.Add(response, false); // add entry to dictionary indicating that interval command has not been sent to this unit
                                }

                                catch (Exception e)
                                {
                                    MessageBox.Show("1682 " + e.ToString());
                                }

                                content = sernum + "   " + handler.RemoteEndPoint.ToString() + "   " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + "   " + content2 + "\r\n";
                                //Incoming(null, content);

                                //string infilename = "C:\\Users\\gayakawa\\desktop\\TCPServer Log\\" + sernum + " incoming.log";

                                //try
                                //{
                                //    StreamWriter incominglog = new StreamWriter(new FileStream(infilename, FileMode.Append, FileAccess.Write));
                                //    incominglog.Write("\r\n" + DateTime.Now + " " + " raw incoming " + content2);
                                //    incominglog.Close();
                                //}

                                //catch (Exception e)
                                //{
                                //    Incoming(null, "===============\r\nIncoming log not updated with the following: " + DateTime.Now + " " + " raw incoming " + content2 + "===============\r\n");
                                //}

                                if (logfinished)
                                {
                                    logfinished = false;
                                    LogData(content, content2);
                                }


                                //}



                                if (response2 != response)

                                {

                                    unitid = response;


                                    modesetdict.TryGetValue(unitid, out modeset); // check if operation mode has previously been checked for this unit
                                    if (!modeset) // operation mode has not previously been checked 
                                    {
                                        // query VLinkUnit table to determine operation mode associated with unit ID
                                        string query2 = "SELECT [Mode] FROM [VLink106466].[dbo].[VLinkUnit] WHERE ([UnitID] = " + response.ToString() + ")";


                                        using (SqlConnection conn3 = new SqlConnection(connectionString))
                                        {
                                            using (SqlCommand comm3 = new SqlCommand(query2, conn3))
                                            {
                                                conn3.Open();


                                                try
                                                {
                                                    OpMode = comm3.ExecuteScalar().ToString();
                                                    if (OpMode == "0000")
                                                    {
                                                        modesetdict.Remove(unitid);
                                                        modesetdict.Add(unitid, true);

                                                    }
                                                }
                                                catch (Exception e2)
                                                {
                                                    MessageBox.Show("1748 " + e2.ToString());
                                                }
                                            }
                                        }
                                    }

                                    intervalsetdict.TryGetValue(unitid, out interset);  // check if interval value has previously been checked for this unit 
                                    if (!interset)  // interval value has not previously been checked
                                    {
                                        // query VLinkUnit table to determine data transmission interval associated with unit ID
                                        string query3 = "SELECT [Interval] FROM [VLink106466].[dbo].[VLinkUnit] WHERE ([UnitID] = " + response.ToString() + ")";


                                        using (SqlConnection conn4 = new SqlConnection(connectionString))
                                        {
                                            using (SqlCommand comm4 = new SqlCommand(query3, conn4))
                                            {
                                                conn4.Open();


                                                try
                                                {
                                                    Interval = comm4.ExecuteScalar().ToString();
                                                    if (Interval == "0000")
                                                    {
                                                        intervalsetdict.Remove(unitid);
                                                        intervalsetdict.Add(unitid, true);

                                                     }

                                                }
                                                catch (Exception e2)
                                                {
                                                    MessageBox.Show("1781 " + e2.ToString());
                                                }
                                            }
                                        }


                                    }
                                }



                                if (!msgsent)
                                {
                                    msgsent = true;
                                    CheckUnsentMessages(sernum);    // check for unsent messages for this unit
                                    
                                }

                                if (content2.Contains("VMC01," + sernum + ",09,")) // received date and time request
                                {
                                    SendCurrTime(sernum); // send set time message


                                }

                                if (content2.Contains("{VMC01," + sernum + ",0A") && fwreq2 == false && fwackwait == false) // received acknowledgement message for action message
                                {
                                    bool match2 = false;
                                    string actupdate;

                                    // get action type from dictionary associating type of action with unit IP address
                                    // (only one type of action [setting an output] will now generate an acknowledgement -- sensor
                                    // data requests no longer do)
                                    curraction2.TryGetValue(sernum, out actupdate);

                                    if (actupdate == "actsetout") // acknowledgement of set output action
                                    {

                                        int unitid2 = 0;
                                        // get unit ID from database table
                                        unitid2 = GetUnitIDFromSN(sernum);


                                        string packid = content2.Split(',')[3]; // get packet id from acknowledgement message



                                        if (ackaction.ContainsKey(packid)) // check if packet id of acknowledgement matches a previously sent "set output" message
                                        {
                                            int actionid = 0;

                                            ackaction.TryGetValue(packid, out actionid); // get action id from dictionary relating packet id with action id
                                            string query = "SELECT id, unitid FROM [VLink106466].[dbo].VLinkActions";


                                            try
                                            {
                                                using (SqlConnection conn = new SqlConnection(connectionString))
                                                {
                                                    using (SqlCommand comm = new SqlCommand(query, conn))
                                                    {
                                                        conn.Open();

                                                        SqlDataAdapter da = new SqlDataAdapter();
                                                        DataSet ds = new DataSet();



                                                        // create dataset and datatable from returned data
                                                        dt5.Clear();
                                                        da.SelectCommand = comm;
                                                        da.Fill(ds, "ActionTable");
                                                        dt5 = ds.Tables["ActionTable"];

                                                    }
                                                }

                                            }

                                            catch (Exception e)
                                            {
                                                MessageBox.Show("1861 " + e.ToString());
                                            }


                                            for (int d = 0; d < dt5.Rows.Count; d++)
                                            {
                                                // find action id and unit id that match action
                                                if (Convert.ToInt32(dt5.Rows[d]["id"]) == actionid && Convert.ToInt32(dt5.Rows[d]["unitid"]) == unitid2)
                                                {
                                                    {
                                                        match2 = true;
                                                        break;
                                                    }
                                                }



                                            }
                                            if (content2.Contains("00}") && match2) // positive acknowledgement
                                            {
                                                // update dictionary object to indicate that the appropriate log file has not been updated 
                                                try
                                                {
                                                    if (!outlogmod.ContainsKey(handler))
                                                        outlogmod.Add(handler, false);
                                                    else
                                                    {
                                                        outlogmod.Remove(handler);
                                                        outlogmod.Add(handler, false);
                                                    }

                                                    ackaction.TryGetValue(content2.Split(',')[3], out actiontoupdate); // find action id corresponding to packet id of acknowledgement message
                                                    UpdateAction2(actiontoupdate, "actsetout"); // update action database table for appropriate action id
                                                    match2 = false;
                                                }

                                                catch (Exception e)
                                                {
                                                    MessageBox.Show("1910 " + e.ToString());
                                                }
                                            }



                                        }



                                    }
                                }

                                string content2A = content2.TrimEnd('\r', '\n');
                                string sensorstart = "VMC01," + sernum + ",01";

                                if (content2A.Contains(sensorstart) && packproc) // received sensor data packet; previous packet has completed processing
                                {
                                    SendCurrTime(sernum); // send current time to unit to acknowledge receipt of sensor data packet
                                    packproc = false;
                                    ProcessDataPacket(content2A); // process sensor data packet

                                }


                                //if (OpMode != "0000" && timeset) // send message if operation mode is not set to default
                                if (OpMode != "0000" && sernum != "" & !modeset) // send message if operation mode is not set to default and mode command has not been previously sent
                                {
                                    try
                                    {
                                        string opmodeset = "{VMC01," + sernum + ",67,FE," + OpMode + "}\r\n";
                                        Thread.Sleep(1000);
                                        Send(handler, opmodeset);
                                        //modeset = true;
                                        modesetdict.Remove(unitid);
                                        modesetdict.Add(unitid, true); // set dictionary value to indicate that mode command has been sent
                                    }

                                    catch (Exception e)
                                    {
                                        MessageBox.Show("2251 " + e.ToString());
                                    }

                                }

                                modesetdict.TryGetValue(unitid, out modeset);

                                //if (Interval != "0000" && timeset && !modeset) // send message if transmission interval is not set to default
                                if (Interval != "0000" && !modeset) // send message if transmission interval is not set to default

                                {
                                    try
                                    {
                                        string intervalset = "{VMC01," + sernum + ",71,FC," + Interval + "}\r\n";
                                        Thread.Sleep(1000);
                                        Send(handler, intervalset);
                                        intervalsetdict.Remove(unitid);
                                        intervalsetdict.Add(unitid, true); // set dictionary value to indicate that interval command has been sent
                                    }

                                    catch (Exception e)
                                    {
                                        MessageBox.Show("2273 " + e.ToString());
                                    }


                                }

                                if (actionschkd)
                                {
                                    actionschkd = false;
                                    CheckActions(sernum); // check action table to see if configured actions exist for this unit

                                }



                                if (content2.Contains("{VMC01," + sernum + ",0A,FF") && fwreq2) // received initial acknowledgement for firmware update request
                                {
                                    fwreq2 = false;
                                    fwstart = true;
                                    fwackrec = true;
                                    nakcount = 0;
                                    Thread.Sleep(5000);
                                    FWSendFile(); // begin sending firmware file
                                }


                                if (content2.Contains("{VMC01," + sernum + ",0A,FE")) // received initial acknowledgement for mode set command
                                {
                                    //modeset = false;

                                    //if (Interval != "0000" && timeset) // send message if transmission interval is not set to default
                                    if (Interval != "0000") // send message if transmission interval is not set to default
                                    {
                                        try
                                        {
                                            string intervalset = "{VMC01," + sernum + ",71,FC," + Interval + "}\r\n";
                                            Thread.Sleep(1000);
                                            Send(handler, intervalset);
                                            //interset = true;
                                            intervalsetdict.Remove(unitid);
                                            intervalsetdict.Add(unitid, true); // set dictionary value to indicate that interval command has been sent
                                        }

                                        catch (Exception e)
                                        {
                                            MessageBox.Show("2426 " + e.ToString());
                                        }
                                    }


                                }



                                //if (content2.Contains("{VMC01," + sernum + ",11") && fwreq2 == false && fwackwait == true) // received acknowledgement upon receipt of firmware chunk
                                if (content2.Contains(sernum + ",11") && fwreq2 == false && fwackwait == true) // received acknowledgement upon receipt of firmware chunk
                                {

                                    current = DateTime.Now;
                                    diffInSeconds = (current - start).TotalSeconds; // check elapsed time between initial sending of firmware data and receipt of acknowledgement
                                    if (content2.Contains("00}") && diffInSeconds >= 0 && diffInSeconds < 20) // positive acknowledgement received within 20 seconds
                                    {
                                        nakcount = 0;
                                        // add log entry indicating firmware update acknowledgement has been received

                                        StreamWriter fwuplog = new StreamWriter(new FileStream("C:\\Users\\gayakawa\\desktop\\TCPServer Log\\" + sernum + ".log", FileMode.Append, FileAccess.Write));
                                        fwuplog.Write("\r\n" + DateTime.Now + " " + handler.RemoteEndPoint.ToString() + " (Serial Number " + sernum + ") firmware update acknowledgement successfully received");
                                        fwuplog.Close();

                                        Outgoing(sernum + "   " + ipa + " " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + "  Sent " + (pos + 512).ToString() + " bytes\r\n");
                                        fwackrec = true;
                                        fwackwait = false;
                                        content2 = "";
                                        diffInSeconds = -1;
                                        pos += 512;
                                        FWSendFile2(); // send next firmware chunk
                                    }

                                    else if (content2.Contains("00}") && diffInSeconds >= 20) // positive acknowledgement not received within 20 seconds
                                        fwstart = false;

                                    else if ((!content2.Contains("00}")))
                                    {
                                        nakcount++;
                                        Outgoing("nakcount = " + nakcount.ToString() + "\r\n");
                                        if (nakcount < 5)
                                        {
                                            fwackrec = true;
                                            FWSendFile2();
                                        }
                                        else if (nakcount == 5)
                                            fwstart = false;
                                    }


                                }


                                string diagstart = "{VMC01," + sernum + ",06";
                                if (content2.Contains(diagstart)) // received diagnostic data packet
                                {
                                    DateTime packettime = DateTime.Now;
                                    content2 = content2.Replace("}", ""); // remove "}"
                                    content2 = content2.Replace("\r\n", ""); // remove CRLF 
                                    String indatastr = String.Empty;
                                    ArrayList sensorid = new ArrayList();
                                    ArrayList sensorval = new ArrayList();
                                    String sensoridstr = String.Empty;
                                    String sensorvalstr = String.Empty;
                                    String sensoridintstr = String.Empty;
                                    String sensorvalintstr = String.Empty;
                                    ArrayList sensoridint = new ArrayList();
                                    ArrayList sensorvalint = new ArrayList();

                                    indata.Clear();
                                    indata.AddRange(content2.Split(',')); // split response at delimiters and store elements in arraylist
                                    sensorvalint.Clear();
                                    intsensorvals.Clear();
                                    stringsensorvals.Clear();
                                    for (int i = 0; i < indata.Count; i++)
                                    {
                                        if (indata[i].ToString().Contains("=")) // get data items in format "sensor id = value"
                                            indata2.Add(indata[i]);             // and store elements in arraylist
                                    }
                                    for (int i = 0; i < indata2.Count; i++)
                                    {
                                        indatastr = indatastr + " " + indata2[i].ToString(); //test
                                        sensorid.Add(indata2[i].ToString().Substring(0, indata2[i].ToString().IndexOf("="))); // add sensor id to arraylist
                                        sensorval.Add(indata2[i].ToString().Substring(indata2[i].ToString().IndexOf("=") + 1)); // add sensor value to arraylist
                                        sensoridstr = sensoridstr + " " + sensorid[i].ToString(); // test
                                        if (IsHex(sensorid[i].ToString())) // value string is hex
                                            sensoridint.Add(Convert.ToInt32(sensorid[i].ToString(), 16)); // convert to integer and add to arraylist
                                        else
                                            sensoridint.Add(sensorid[i].ToString()); // add unconverted string to arraylist

                                        sensoridintstr = sensoridintstr + " " + sensoridint[i].ToString(); // test
                                        sensorvalstr = sensorvalstr + " " + sensorval[i].ToString(); // test

                                        if (IsHex(sensorval[i].ToString())) // string is hex 
                                        {
                                            try
                                            {
                                                sensorvalint.Add(Convert.ToInt32(sensorval[i].ToString(), 16)); // convert to integer and add to arraylist
                                                intsensorvals.Add(Convert.ToInt32(sensoridint[i]), Convert.ToInt32(sensorvalint[i])); // add sensor id and integer value to dictionary of integer sensor values
                                            }

                                            catch (Exception e)
                                            {
                                                MessageBox.Show("2841 " + e.ToString());
                                            }
                                            //units.TryGetValue(handler, out unitid);
                                            unitid = GetUnitIDFromSN(sernum);
                                            string query = "EXEC proc_storedatapacket @unitid, @sensor_id, @value1, @value2, @value3, @packetdate";

                                            using (SqlConnection conn = new SqlConnection(connectionString))
                                            {
                                                using (SqlCommand comm = new SqlCommand(query, conn))
                                                {
                                                    conn.Open();
                                                    comm.Parameters.AddWithValue("@unitid", unitid);
                                                    comm.Parameters.AddWithValue("@sensor_id", sensoridint[i]);
                                                    comm.Parameters.AddWithValue("@value1", sensorvalint[i]);
                                                    comm.Parameters.AddWithValue("@value2", DBNull.Value);
                                                    comm.Parameters.AddWithValue("@value3", DBNull.Value);
                                                    comm.Parameters.AddWithValue("@packetdate", packettime);

                                                    try
                                                    {
                                                        Int32 response6 = Convert.ToInt32(comm.ExecuteScalar());
                                                        //MessageBox.Show(response.ToString());
                                                    }
                                                    catch (Exception e)
                                                    {
                                                        MessageBox.Show("2803 " + e.ToString());
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            try
                                            {
                                                sensorvalint.Add(sensorval[i].ToString()); // add unconverted string to arraylist
                                                stringsensorvals.Add(Convert.ToInt32(sensoridint[i]), sensorvalint[i].ToString()); // add sensor id and string value to dictionary of string sensor values
                                            }

                                            catch (Exception e)
                                            {
                                                MessageBox.Show("2881 " + e.ToString());
                                            }
                                            unitid = GetUnitIDFromSN(sernum);
                                            string query = "EXEC proc_storedatapacket @unitid, @sensor_id, @value1, @value2, @value3, @packetdate";

                                            using (SqlConnection conn = new SqlConnection(connectionString))
                                            {
                                                using (SqlCommand comm = new SqlCommand(query, conn))
                                                {
                                                    conn.Open();
                                                    comm.Parameters.AddWithValue("@unitid", unitid);
                                                    comm.Parameters.AddWithValue("@sensor_id", sensoridint[i]);
                                                    comm.Parameters.AddWithValue("@value1", DBNull.Value);
                                                    comm.Parameters.AddWithValue("@value2", DBNull.Value);
                                                    comm.Parameters.AddWithValue("@value3", sensorvalint[i]);
                                                    comm.Parameters.AddWithValue("@packetdate", packettime);

                                                    try
                                                    {
                                                        Int32 response6 = Convert.ToInt32(comm.ExecuteScalar());
                                                        //MessageBox.Show(response.ToString());
                                                    }
                                                    catch (Exception e)
                                                    {
                                                        MessageBox.Show("2836 " + e.ToString());
                                                    }
                                                }
                                            }
                                        }

                                        sensorvalintstr = sensorvalintstr + " " + sensorvalint[i].ToString();   // test


                                    }


                                }

                            }

                        }
                    }
                    else
                    {
                        // Not all data received. Get more.
                        //if (ar == currAsyncResult)
                        //{
                        //    MessageBox.Show("ar is STILL = currAsyncResult");
                            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                            new AsyncCallback(ReadCallback), state);
                        //}

                    }
                }
            }


            //}
            //else
            //{
            //    // ignore;
            //    //MessageBox.Show("Socket change required");
            //}
            //}

            //catch (Exception ex)
            //{
            //    //Log exception. Don't throw exception. Most probably BeginReceive failed.
            //    Outgoing(ex.ToString());
            //}





            //}



            catch (Exception e)
            {
                MessageBox.Show("2870 " + e.ToString());

            }


             
           

            
        

        }

        public static void LogData (string data1, string data2)
        {
            Incoming(null, data1);

            string infilename = "C:\\Users\\gayakawa\\desktop\\TCPServer Log\\" + sernum + " incoming.log";

            try
            {
                StreamWriter incominglog = new StreamWriter(new FileStream(infilename, FileMode.Append, FileAccess.Write));
                incominglog.Write("\r\n" + DateTime.Now + " " + " raw incoming " + data2);
                incominglog.Close();
            }

            catch (Exception e)
            {
                Incoming(null, "===============\r\nIncoming log not updated with the following: " + DateTime.Now + " " + " raw incoming " + data2 + "===============\r\n");
            }

            logfinished = true;

        }
        public static void ProcessDataPacket(string packet)
        {
            ArrayList indata = new ArrayList();
            ArrayList indata2 = new ArrayList();
            string sensorstart = "VMC01," + sernum + ",01";
            
            if (packet.Contains(sensorstart) && packet.Contains("}")) // received sensor data packet with proper structure
            {
               
                string packetid = packet.Split(',')[3]; // split packet at delimiters and get packet id
                DateTime packettime = DateTime.Now;
                packet = packet.Replace("}", ""); // remove "}"
                String indatastr = String.Empty;
                ArrayList sensorid = new ArrayList();
                ArrayList sensorval = new ArrayList();
                String sensoridstr = String.Empty;
                String sensorvalstr = String.Empty;
                String sensoridintstr = String.Empty;
                String sensorvalintstr = String.Empty;
                ArrayList sensoridint = new ArrayList();
                ArrayList sensorvalint = new ArrayList();

                indata.Clear();
                indata.AddRange(packet.Split(',')); // split response at delimiters and store elements in arraylist
                indata2.Clear();
                sensorvalint.Clear();
                intsensorvals.Clear();
                stringsensorvals.Clear();
                sensorid.Clear();
                sensorval.Clear();
                sensoridint.Clear();

                for (int i = 0; i < indata.Count; i++)
                {
                    if (indata[i].ToString().Contains("=")) // get data items in format "sensor id = value"
                        indata2.Add(indata[i]);             // and store elements in arraylist
                }

                for (int i = 0; i < indata2.Count; i++)
                {
                    indatastr = indatastr + " " + indata2[i].ToString(); //test
                    sensorid.Add(indata2[i].ToString().Substring(0, indata2[i].ToString().IndexOf("="))); // add sensor id to arraylist
                    sensorval.Add(indata2[i].ToString().Substring(indata2[i].ToString().IndexOf("=") + 1)); // add sensor value to arraylist
                    sensoridstr = sensoridstr + " " + sensorid[i].ToString(); // test
                    if (IsHex(sensorid[i].ToString())) // id string is hex
                        sensoridint.Add(Convert.ToInt32(sensorid[i].ToString(), 16)); // convert to integer and add to arraylist
                    else
                        sensoridint.Add(sensorid[i].ToString()); // add unconverted string to arraylist

                    sensoridintstr = sensoridintstr + " " + sensoridint[i].ToString(); // test
                    sensorvalstr = sensorvalstr + " " + sensorval[i].ToString(); // test


                    //int packetunixtime = 0;

                    // hex data from sensors (not user input from Isotote touchscreen device) 
                    if (!(indata2[i].ToString().StartsWith("35=") || indata2[i].ToString().StartsWith("36=") || indata2[i].ToString().StartsWith("37=")
                        || indata2[i].ToString().StartsWith("38=") || indata2[i].ToString().StartsWith("39=") || indata2[i].ToString().StartsWith("4f=")
                        || indata2[i].ToString().StartsWith("50=") || indata2[i].ToString().StartsWith("3d=") || indata2[i].ToString().StartsWith("3e=")
                        || indata2[i].ToString().StartsWith("3f=") || indata2[i].ToString().StartsWith("40=") || indata2[i].ToString().StartsWith("41=")
                        || indata2[i].ToString().StartsWith("42=") || indata2[i].ToString().StartsWith("43=") || indata2[i].ToString().StartsWith("44=") || indata2[i].ToString().StartsWith("45=")
                        || indata2[i].ToString().StartsWith("46=") || indata2[i].ToString().StartsWith("47=") || indata2[i].ToString().StartsWith("48=")
                        || indata2[i].ToString().StartsWith("49=") || indata2[i].ToString().StartsWith("4a=") || indata2[i].ToString().StartsWith("4b=") || indata2[i].ToString().StartsWith("4c=")
                        || indata2[i].ToString().StartsWith("4d=") || indata2[i].ToString().StartsWith("4e=") || indata2[i].ToString().StartsWith("51=")
                        || indata2[i].ToString().StartsWith("52=") || indata2[i].ToString().StartsWith("53=") || indata2[i].ToString().StartsWith("54=") || indata2[i].ToString().StartsWith("55=")
                        || indata2[i].ToString().StartsWith("56=") || indata2[i].ToString().StartsWith("57=")))
                    //if (IsHex(sensorval[i].ToString())) // string is hex

                    {
                        if (indata2[i].ToString().StartsWith("3B=")) //temperature sensor is float that must be converted to 16-bit integer. Final
                                                                     // valuc multiplied by 10 before sending to database
                        {
                            try
                            {
                                string hexString = sensorval[i].ToString();
                                uint num = uint.Parse(hexString, System.Globalization.NumberStyles.AllowHexSpecifier); // parse received string as hex value

                                byte[] bytes = BitConverter.GetBytes(num); // create byte array
                                //if (BitConverter.IsLittleEndian)
                                //{
                                //    bytes = bytes.Reverse().ToArray();
                                //}
                                float myFloat = BitConverter.ToSingle(bytes, 0); // convert byte array to float
                                short roundx = (short)Math.Round(myFloat, 0); // convert float to 16-bit integer (rounding to nearest integer)
                                short finalx = (Int16)(roundx * 10); //multiply value by 10
                                sensorvalint.Add(finalx); // convert to integer and add to arraylist
                                intsensorvals.Add(Convert.ToInt32(sensoridint[i]), Convert.ToInt32(sensorvalint[i])); // add sensor id and integer value to dictionary of integer sensor values
                                                                                                                      //packetunixtime = Convert.ToInt32(sensorvalint[0]); // convert packet time hex data to decimal integer
                                                                                                                      //MessageBox.Show("packet unix time = " + packetunixtime.ToString());
                            }

                            catch (Exception e)
                            {
                                MessageBox.Show("1980 " + sensoridintstr + "\r\n" + e.ToString());
                            }

                            unitid = GetUnitIDFromSN(sernum); // get unit id of currently connected unit

                            string query = "EXEC proc_storedatapacket @unitid, @sensor_id, @value1, @value2, @value3, @packetdate";

                            using (SqlConnection conn = new SqlConnection(connectionString))
                            {
                                // call stored procedure to update data packets database table (hex data converted to integer)
                                using (SqlCommand comm = new SqlCommand(query, conn))
                                {
                                    conn.Open();
                                    comm.Parameters.AddWithValue("@unitid", unitid);
                                    comm.Parameters.AddWithValue("@sensor_id", sensoridint[i]);
                                    comm.Parameters.AddWithValue("@value1", sensorvalint[i]);
                                    comm.Parameters.AddWithValue("@value2", DBNull.Value);
                                    comm.Parameters.AddWithValue("@value3", DBNull.Value);
                                    comm.Parameters.AddWithValue("@packetdate", packettime);

                                    //if (packetunixtime < 1514764800) // if data packet timestamp year < 2018
                                    //{
                                    //    packettime = DateTime.Now.ToUniversalTime();
                                    //    comm.Parameters.AddWithValue("@packetdate", packettime); // use current time (converted to UTC) as packet timestamp sent to database

                                    //}
                                    //else // if data packet timestamp year >= 2018
                                    //{
                                    //    packettime = UnixTimeStampToDateTime(packetunixtime);
                                    //    packettime = packettime.ToUniversalTime(); // use data packet time data (converted to UTC) as packet timestamp sent to database
                                    //    comm.Parameters.AddWithValue("@packetdate", packettime);

                                    //}



                                    try
                                    {
                                        Int32 response3 = Convert.ToInt32(comm.ExecuteScalar());
                                        //MessageBox.Show(response.ToString());

                                    }
                                    catch (Exception e)
                                    {
                                        MessageBox.Show("1984 " + e.ToString());
                                    }
                                }
                            }
                        }

                        else
                        {
                            try
                            {
                                sensorvalint.Add(Convert.ToInt32(sensorval[i].ToString(), 16)); // convert to integer and add to arraylist
                                intsensorvals.Add(Convert.ToInt32(sensoridint[i]), Convert.ToInt32(sensorvalint[i])); // add sensor id and integer value to dictionary of integer sensor values
                                //packetunixtime = Convert.ToInt32(sensorvalint[0]); // convert packet time hex data to decimal integer
                                //MessageBox.Show("packet unix time = " + packetunixtime.ToString());
                            }

                            catch (Exception e)
                            {
                                MessageBox.Show("1980 " + sensoridintstr + "\r\n" + e.ToString());
                            }

                            unitid = GetUnitIDFromSN(sernum); // get unit id of currently connected unit

                            string query = "EXEC proc_storedatapacket @unitid, @sensor_id, @value1, @value2, @value3, @packetdate";

                            using (SqlConnection conn = new SqlConnection(connectionString))
                            {
                                // call stored procedure to update data packets database table (hex data converted to integer)
                                using (SqlCommand comm = new SqlCommand(query, conn))
                                {
                                    conn.Open();
                                    comm.Parameters.AddWithValue("@unitid", unitid);
                                    comm.Parameters.AddWithValue("@sensor_id", sensoridint[i]);
                                    comm.Parameters.AddWithValue("@value1", sensorvalint[i]);
                                    comm.Parameters.AddWithValue("@value2", DBNull.Value);
                                    comm.Parameters.AddWithValue("@value3", DBNull.Value);
                                    comm.Parameters.AddWithValue("@packetdate", packettime);

                                    //if (packetunixtime < 1514764800) // if data packet timestamp year < 2018
                                    //{
                                    //    packettime = DateTime.Now.ToUniversalTime();
                                    //    comm.Parameters.AddWithValue("@packetdate", packettime); // use current time (converted to UTC) as packet timestamp sent to database

                                    //}
                                    //else // if data packet timestamp year >= 2018
                                    //{
                                    //    packettime = UnixTimeStampToDateTime(packetunixtime);
                                    //    packettime = packettime.ToUniversalTime(); // use data packet time data (converted to UTC) as packet timestamp sent to database
                                    //    comm.Parameters.AddWithValue("@packetdate", packettime);

                                    //}



                                    try
                                    {
                                        Int32 response3 = Convert.ToInt32(comm.ExecuteScalar());
                                        //MessageBox.Show(response.ToString());

                                    }
                                    catch (Exception e)
                                    {
                                        MessageBox.Show("1984 " + e.ToString());
                                    }
                                }
                            }


                        }
                    }

                    // string data from Isotote touchscreen device user input and fuel efficiency test (latitude and longitude)
                    else if (indata2[i].ToString().StartsWith("35=") || indata2[i].ToString().StartsWith("36=") || indata2[i].ToString().StartsWith("37=")
                        || indata2[i].ToString().StartsWith("38=") || indata2[i].ToString().StartsWith("39=") || indata2[i].ToString().StartsWith("4f=")
                        || indata2[i].ToString().StartsWith("50=") || indata2[i].ToString().StartsWith("3d=") || indata2[i].ToString().StartsWith("3e=")
                        || indata2[i].ToString().StartsWith("3f=") || indata2[i].ToString().StartsWith("40=") || indata2[i].ToString().StartsWith("41=")
                        || indata2[i].ToString().StartsWith("42=") || indata2[i].ToString().StartsWith("43=") || indata2[i].ToString().StartsWith("44=") || indata2[i].ToString().StartsWith("45=")
                        || indata2[i].ToString().StartsWith("46=") || indata2[i].ToString().StartsWith("47=") || indata2[i].ToString().StartsWith("48=")
                        || indata2[i].ToString().StartsWith("49=") || indata2[i].ToString().StartsWith("4a=") || indata2[i].ToString().StartsWith("4b=") || indata2[i].ToString().StartsWith("4c=")
                        || indata2[i].ToString().StartsWith("4d=") || indata2[i].ToString().StartsWith("4e=") || indata2[i].ToString().StartsWith("51=")
                        || indata2[i].ToString().StartsWith("52=") || indata2[i].ToString().StartsWith("53=") || indata2[i].ToString().StartsWith("54=") || indata2[i].ToString().StartsWith("55=")
                        || indata2[i].ToString().StartsWith("56=") || indata2[i].ToString().StartsWith("57="))
                    {
                        try
                        {
                            sensorvalint.Add(sensorval[i].ToString()); // add unconverted string to arraylist
                            stringsensorvals.Add(Convert.ToInt32(sensoridint[i]), sensorvalint[i].ToString()); // add sensor id and string value to dictionary of string sensor values
                        }

                        catch (Exception e)
                        {
                            MessageBox.Show("2018 " + e.ToString());
                        }

                        unitid = GetUnitIDFromSN(sernum); // get unit id of currently connected unit
                                                          //units.TryGetValue(handler, out unitid); // get unit id of currently connected unit
                        string query = "EXEC proc_storedatapacket @unitid, @sensor_id, @value1, @value2, @value3, @packetdate";

                        using (SqlConnection conn = new SqlConnection(connectionString))
                        {
                            // call stored procedure to update data packets database table (string data)
                            using (SqlCommand comm = new SqlCommand(query, conn))
                            {
                                conn.Open();
                                comm.Parameters.AddWithValue("@unitid", unitid);
                                comm.Parameters.AddWithValue("@sensor_id", sensoridint[i]);
                                comm.Parameters.AddWithValue("@value1", DBNull.Value);
                                comm.Parameters.AddWithValue("@value2", DBNull.Value);
                                comm.Parameters.AddWithValue("@value3", sensorvalint[i]);
                                comm.Parameters.AddWithValue("@packetdate", packettime);

                                //if (packetunixtime < 1514764800) // if data packet timestamp year < 2018
                                //{
                                //    packettime = DateTime.Now.ToUniversalTime();
                                //    comm.Parameters.AddWithValue("@packetdate", packettime); // use current time (converted to UTC) as packet timestamp sent to database
                                //    }

                                //else // if data packet timestamp year >= 2018
                                //{
                                //    packettime = UnixTimeStampToDateTime(packetunixtime);
                                //    packettime = packettime.ToUniversalTime(); // use data packet time data (converted to UTC) as packet timestamp sent to database 
                                //        comm.Parameters.AddWithValue("@packetdate", packettime);

                                //}

                                try
                                {
                                    Int32 response4 = Convert.ToInt32(comm.ExecuteScalar());



                                }
                                catch (Exception e)
                                {
                                    MessageBox.Show("2023 " + e.ToString());
                                }
                            }
                        }


                    }

                sensorvalintstr = sensorvalintstr + " " + sensorvalint[i].ToString();   // test

                
            }
            



            string actionidreqstr = "";
            string comptimestr = "";
            string query2 = "SELECT [id] FROM [VLink106466].[dbo].[VLinkActions] WHERE ([UnitID] = " + unitid.ToString() + ")" +
                " AND ([ActionType] = '1')";

            // check action table to see if a specific data request action item exists for this unit ID

            using (SqlConnection conn3 = new SqlConnection(connectionString))
            {
                using (SqlCommand comm3 = new SqlCommand(query2, conn3))
                {
                    conn3.Open();


                    try
                    {
                        if (comm3.ExecuteScalar() != null)
                            actionidreqstr = comm3.ExecuteScalar().ToString();
                    }


                    catch (Exception e2)
                    {
                        MessageBox.Show("2084 " + e2.ToString());
                    }
                }
            }

            if (actionidreqstr != "")   // if action item found, check if action has previously been completed
            {
                
                string query3 = "SELECT [CompleteTime] FROM [VLink106466].[dbo].[VLinkActions] WHERE ([id] = " + actionidreqstr + ")";

                using (SqlConnection conn4 = new SqlConnection(connectionString))
                {
                    using (SqlCommand comm4 = new SqlCommand(query3, conn4))
                    {
                        conn4.Open();


                        try
                        {
                            comptimestr = comm4.ExecuteScalar().ToString();
                        }


                        catch (Exception e2)
                        {
                            MessageBox.Show("2109 " + e2.ToString());
                        }
                    }
                }


                

                
                if (comptimestr == "")  // if action not yet completed
                {
                    actionidreq = Convert.ToInt32(actionidreqstr);
                    UpdateAction2(actionidreq, "actisoreq"); // update action table entry for action id
                }
            }
                
                unitid = GetUnitIDFromSN(sernum);
                //CheckAlarms(unitid);
                
            }

            
            // if a packet of returned sensor data does not have the proper structure, reset the "pending time"
            // in the database action table to trigger new requests for sensor data

            
            else if (packet.StartsWith(sensorstart) && !packet.Contains("}"))
            {

                string query = "EXEC proc_updateaction @id, @pending, @complete";

                DateTime pending = DateTime.Parse("1-1-1970 00:00:00"); // reset pending time

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        using (SqlCommand comm = new SqlCommand(query, conn))
                        {

                            conn.Open();
                            comm.Parameters.AddWithValue("@id", actiontoupdate);
                            comm.Parameters.AddWithValue("@pending", pending);
                            comm.Parameters.AddWithValue("@complete", DBNull.Value);

                            try
                            {
                                Int32 response5 = Convert.ToInt32(comm.ExecuteScalar());
                                
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show(e.ToString());
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("2204 " + e.ToString());
                }
            }

            packproc = true; // set flag indicating that packet processing is complete
            



        }
// *****************CONTINUE CLEANUP FROM HERE **********************************
        private static void CheckUnsentMessages(string sn)
        {
            
            string filename = "C:\\ProgramData\\TCPServer\\Unsent_Messages_" + sn;
            //if (File.Exists("C:\\ProgramData\\TCPServer\\Unsent_Messages.txt"))
            if (File.Exists(filename + ".txt"))
            {
                //StreamReader unsentcheck = new StreamReader(new FileStream("C:\\ProgramData\\TCPServer\\Unsent_Messages.txt", FileMode.Open, FileAccess.Read));
                StreamReader unsentcheck = new StreamReader(new FileStream(filename + ".txt", FileMode.Open, FileAccess.Read));
                string line = unsentcheck.ReadLine();
                //string snunsent = String.Empty;
                //while (line != null && line.Length > 0)
                unsentarr.Clear();
                while (line != null)
                {
                    if (line.Contains(","))
                    {
                        //snunsent = line.Split(',')[1];
                        //if (snunsent == sernum)
                        //{
                        unsentarr.Add(line);
                        //}
                    }
                    line = unsentcheck.ReadLine();

                }
                unsentcheck.Close();
                unsentcheck.Dispose();

                File.Delete(filename + ".txt");




                for (int x = 0; x < unsentarr.Count; x++)
                    {
                        if (unsentarr[x].ToString().Contains("Send Current Time,"))
                            SendCurrTime(sn);
                        else
                        {
                            if (unsentarr[x].ToString().Contains("64,FF"))
                            {
                                fwreq2 = true;
                                pos = 0;
                            }

                            Socket handler2 = null;
                            sernumdict2.TryGetValue(sn, out handler2);
                            Send(handler2, unsentarr[x].ToString() + "\r\n");
                        }
                        
                        Thread.Sleep(10000);
                        
                    }
                    
                

                
                //File.Delete(filename + ".txt");


            }
        }
    
        public static void CheckAlarms(int Unit)
        {
            string emailfinal;
            MailMessage mailObj;
            string SerialNumber = String.Empty;
            dtalarm.Clear();

            if (Unit != 0)
            {
                Thread.Sleep(500);
                //MessageBox.Show("Unit = " + Unit.ToString());

                // get alarm configuration for this unit
                string query3 = "EXEC proc_getalarmconfiguration @username, @unitid";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand comm = new SqlCommand(query3, conn))
                    {
                        try
                        {
                            comm.Parameters.AddWithValue("@username", "gayakawa");
                            comm.Parameters.AddWithValue("@unitid", Unit);
                            SqlDataAdapter da = new SqlDataAdapter();
                            DataSet ds = new DataSet();
                            //dt = new DataTable();

                            // create dataset and datatable from returned configuration data
                            da.SelectCommand = comm;
                            da.Fill(ds, "AlarmTable");
                            dtalarm.Clear();
                            alarmsensor.Clear();
                            alarmlow.Clear();
                            alarmhigh.Clear();
                            almlowlimits.Clear();
                            almhighlimits.Clear();
                            almids.Clear();
                            almsensors.Clear();
                            dtalarm = ds.Tables["AlarmTable"];

                            // create arraylist containing unit ID followed by list of alarm IDs
                            //alarmlist.Add(Unit);
                            //foreach (DataRow dr in dtalarm.Rows)
                            //{
                            //    alarmlist.Add(Convert.ToInt32(dr["id"]));
                            //}

                            // create second arraylist containing only alarm IDs
                            //alarmlist2 = alarmlist;
                            //alarmlist2.RemoveAt(0);

                            // add entry to dictionary that relates alarm ID to sensor ID
                            foreach (DataRow dr in dtalarm.Rows)
                            {
                                if (!alarmsensor.ContainsKey(Convert.ToInt32(dr["id"])))
                                    alarmsensor.Add(Convert.ToInt32(dr["id"]), Convert.ToInt32(dr["SensorID"]));

                            }

                            // add entry to dictionary that relates alarm ID to low limit value
                            foreach (DataRow dr in dtalarm.Rows)
                            {
                                if (!alarmlow.ContainsKey(Convert.ToInt32(dr["id"])))
                                {
                                    if (dr["LowLimit"] != DBNull.Value)
                                        alarmlow.Add(Convert.ToInt32(dr["id"]), Convert.ToDouble(dr["LowLimit"]));
                                    else
                                        alarmlow.Add(Convert.ToInt32(dr["id"]), -1);
                                }
                            }

                            // add entry to dictionary that relates alarm ID to high limit value
                            foreach (DataRow dr in dtalarm.Rows)
                            {
                                if (!alarmhigh.ContainsKey(Convert.ToInt32(dr["id"])))
                                {
                                    if (dr["HighLimit"] != DBNull.Value)
                                        alarmhigh.Add(Convert.ToInt32(dr["id"]), Convert.ToDouble(dr["HighLimit"]));
                                    else
                                        alarmhigh.Add(Convert.ToInt32(dr["id"]), -1);
                                }
                            }
                        }



                        catch (Exception e2)
                        {
                            MessageBox.Show("problem at 2718\r\n" + e2.ToString());
                        }

                        // create array of sensor IDs associated with alarms
                        Dictionary<int, int>.ValueCollection valueColl =
                            alarmsensor.Values;
                        int[] sensorarray = new int[alarmsensor.Count];
                        valueColl.CopyTo(sensorarray, 0);

                        // create array of available sensor IDs
                        Dictionary<int, int>.KeyCollection keyColl2 =
                            intsensorvals.Keys;
                        int[] sensorarray2 = new int[intsensorvals.Count];
                        keyColl2.CopyTo(sensorarray2, 0);

                        // create array of alarm IDs
                        Dictionary<int, int>.KeyCollection keyColl =
                                    alarmsensor.Keys;
                        int[] alarmidarray = new int[alarmsensor.Count];
                        keyColl.CopyTo(alarmidarray, 0);

                        // create array of alarm low limits
                        Dictionary<int, double>.ValueCollection valueColl2 =
                                    alarmlow.Values;
                        double[] alarmlowarray = new double[alarmlow.Count];
                        valueColl2.CopyTo(alarmlowarray, 0);

                        // create array of alarm high limits
                        Dictionary<int, double>.ValueCollection valueColl3 =
                                    alarmhigh.Values;
                        double[] alarmhigharray = new double[alarmhigh.Count];
                        valueColl3.CopyTo(alarmhigharray, 0);


                        // check if any previously set alarms should no longer be active

                        // get table of unacknowledged alarms

                        string query5 = "EXEC proc_getalarms @username, @unitid, @new, @start, @end";
                        try
                        {
                            using (SqlConnection conn2 = new SqlConnection(connectionString))
                            {
                                using (SqlCommand comm2 = new SqlCommand(query5, conn2))
                                {
                                    conn2.Open();
                                    comm2.Parameters.AddWithValue("@username", "gayakawa");
                                    comm2.Parameters.AddWithValue("@unitid", Unit);
                                    comm2.Parameters.AddWithValue("@new", 1);
                                    comm2.Parameters.AddWithValue("@start", DBNull.Value);
                                    comm2.Parameters.AddWithValue("@end", DBNull.Value);

                                    SqlDataAdapter da = new SqlDataAdapter();
                                    DataSet ds = new DataSet();
                                    //DataTable dt = new DataTable();

                                    // create dataset and datatable from returned configuration data
                                    da.SelectCommand = comm2;
                                    da.Fill(ds, "ActiveAlarmTable");
                                    dt4 = ds.Tables["ActiveAlarmTable"];
                                    //MessageBox.Show("Created dt4 OK"); //TEST


                                }
                            }
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.ToString());
                        }

                        // This query will create a dataset that includes the unique alarm id from the Alarms table, the alarm id from the AlarmConfig
                        // table, the "EndDate" from the Alarms table, and the unit id, sensor id, low limit and high limit data from the AlarmConfig
                        // table. The dataset will join the Alarms and AlarmConfig tables where the unique sensor id data matches in both tables

                        string query2A = "SELECT[VLink106466].[dbo].[VLinkAlarms].[id], [VLink106466].[dbo].[VLinkAlarms].[ConfigID], " +
                                            "[VLink106466].[dbo].[VLinkAlarms].[EndDate], [VLink106466].[dbo].[VLinkAlarmConfig].[UnitID], [SensorID], [LowLimit], [HighLimit] " +
                                            "FROM [VLink106466].[dbo].[VLinkAlarmConfig] INNER JOIN [VLink106466].[dbo].[VLinkAlarms] " +
                                            "ON ([VLink106466].[dbo].[VLinkAlarms].[ConfigID] = [VLink106466].[dbo].[VLinkAlarmConfig].[id]) ";
                        try
                        {
                            using (SqlConnection conn2 = new SqlConnection(connectionString))
                            {
                                using (SqlCommand comm2 = new SqlCommand(query2A, conn2))
                                {
                                    conn2.Open();

                                    SqlDataAdapter da = new SqlDataAdapter();
                                    DataSet ds = new DataSet();
                                    //DataTable dt = new DataTable();

                                    // create dataset and datatable from returned data
                                    da.SelectCommand = comm2;
                                    da.Fill(ds, "ActiveAlarmTable2");
                                    dt3 = ds.Tables["ActiveAlarmTable2"];
                                    //MessageBox.Show("Created dt3 OK"); //TEST
                                }
                            }

                        }

                        catch (Exception e)
                        {

                            MessageBox.Show("problem at 2822\r\n" + e.ToString());
                        }




                        foreach (DataRow dr in dt3.Rows)
                        {
                            if (Convert.ToInt32(dr["UnitID"]) == Unit && dr["EndDate"] == DBNull.Value) // active alarms
                            {
                                // add entry to dictionary that relates unique alarm ID to sensor ID for active alarms
                                if (!almidsensid.ContainsKey(Convert.ToInt32(dr["id"])))
                                    almidsensid.Add(Convert.ToInt32(dr["id"]), Convert.ToInt32(dr["SensorID"]));
                            }

                            // create array of sensor IDs associated with alarms that have been set
                            Dictionary<int, int>.ValueCollection valueColl4 =
                                almidsensid.Values;
                            //int[] actalmsensarray = new int[almidsensid.Count];
                            actalmsensarray = new int[almidsensid.Count];
                            valueColl4.CopyTo(actalmsensarray, 0);

                            try
                            {
                                if (Convert.ToInt32(dr["UnitID"]) == Unit && dr["EndDate"] == DBNull.Value) // active alarm
                                {
                                    if (dr["LowLimit"] == DBNull.Value) // if "low limit" set to null for this alarm, add max 4-byte integer value for the "low limit" value
                                                                        // to the low limit array
                                        almlowlimits.Add(Int32.MaxValue);
                                    else
                                        almlowlimits.Add(Convert.ToInt32(dr["LowLimit"])); // add "low limit" value to low limit array
                                    //MessageBox.Show("Low Limit = " + almlowlimits[almlowlimits.Count - 1].ToString()); //TEST
                                    if (dr["HighLimit"] == DBNull.Value) // if "high limit" set to null for this alarm, add -1 for the "high limit" value
                                                                         // to the high limit array
                                        almhighlimits.Add(-1);
                                    else
                                        almhighlimits.Add(Convert.ToInt32(dr["HighLimit"]));  // add "high limit" value to high limit array
                                    //MessageBox.Show("High Limit = " + almhighlimits[almhighlimits.Count - 1].ToString()); //TEST
                                    almsensors.Add(Convert.ToInt32(dr["SensorID"])); // add sensor id to sensor id array
                                    //MessageBox.Show("Sensor ID = " + almsensors[almsensors.Count - 1].ToString()); //TEST
                                    almids.Add(Convert.ToInt32(dr["id"])); // add unique alarm id to id array
                                    //MessageBox.Show("Alarm ID = " + almids[almids.Count - 1].ToString()); //TEST

                                }
                            }
                            catch (Exception e)
                            {

                                MessageBox.Show("problem at 2870\r\n" + e.ToString());
                            }


                        }

                        bool lowok = false;
                        bool highok = false;
                        // create array of sensor IDs associated with alarms that have been set
                        //Dictionary<int, int>.ValueCollection valueColl4 =
                        //    almidsensid.Values;
                        //int[] actalmsensarray = new int[almidsensid.Count];
                        //valueColl4.CopyTo(actalmsensarray, 0);
                        int checksensval = 0;
                        for (int f = 0; f < actalmsensarray.Length; f++)
                            for (int g = 0; g < almsensors.Count; g++)
                            {
                                if (Convert.ToInt32(actalmsensarray[f]) == Convert.ToInt32(almsensors[g])) // sensors with active alarms
                                {
                                    intsensorvals.TryGetValue(Convert.ToInt32(actalmsensarray[f]), out checksensval); // get current sensor value
                                    if (checksensval > Convert.ToInt32(almlowlimits[g]) || Convert.ToInt32(almlowlimits[g]) == Int32.MaxValue)
                                        lowok = true; // if sensor value is greater than alarm low limit or low limit set to null
                                    else if (checksensval < Convert.ToInt32(almlowlimits[g]))
                                        lowok = false;
                                    //MessageBox.Show("lowok = " + lowok.ToString() + " for " + actalmsensarray[f].ToString());
                                    if (checksensval < Convert.ToInt32(almhighlimits[g]) || Convert.ToInt32(almhighlimits[g]) == -1)
                                        highok = true; // if sensor value is less than alarm high limit or high limit set to null
                                    else if (checksensval > Convert.ToInt32(almhighlimits[g]))
                                        highok = false;
                                    //MessageBox.Show("highok = " + highok.ToString() + " for " + actalmsensarray[f].ToString() + "\r\n"
                                    //    + " value = " + checksensval.ToString() + " high limit = " + almhighlimits[g].ToString());

                                    if (lowok && highok) // if sensor value is within alarm limits, end alarm

                                    {

                                        string query = "EXEC proc_endalarm @unitid, @id, @date";

                                        using (SqlConnection conn3 = new SqlConnection(connectionString))
                                        {
                                            using (SqlCommand comm3 = new SqlCommand(query, conn3))
                                            {
                                                conn3.Open();
                                                comm3.Parameters.AddWithValue("@unitid", Unit);
                                                comm3.Parameters.AddWithValue("@id", almids[g]);
                                                comm3.Parameters.AddWithValue("@date", DateTime.Now);


                                                try
                                                {
                                                    Int32 response = Convert.ToInt32(comm3.ExecuteScalar());
                                                    almidsensid.Remove(Convert.ToInt32(almids[g]));
                                                    //MessageBox.Show(response.ToString());
                                                }
                                                catch (Exception e)
                                                {
                                                    MessageBox.Show(e.ToString());
                                                }
                                            }
                                        }
                                    }

                                    //else
                                    //{
                                    //    MessageBox.Show("Alarm still active. Unit " + Unit.ToString() + "alarm id " + almids[g].ToString() + " \r\n" +
                                    //        "sensor id " + actalmsensarray[f].ToString() + "sensor value " + checksensval.ToString() + " \r\n" +
                                    //        "low limit = " + almlowlimits[f].ToString() + "high limit " + almhighlimits[f].ToString());
                                    //}

                                    //lowok = false;
                                    //highok = false;


                                }
                            }





                        // check if any alarms need to be set
                        for (int b = 0; b < sensorarray.Length; b++)
                            for (int a = 0; a < sensorarray2.Length; a++)
                            {
                                if (sensorarray2[a] == sensorarray[b])  // if a sensor has an associated alarm
                                {
                                    //MessageBox.Show("Found " + sensorarray2[a].ToString() + " and " + sensorarray[b].ToString()); //TEST
                                    //MessageBox.Show("Low Limit = " + alarmlowarray[b].ToString() + " and High Limit = " + alarmhigharray[b].ToString()); //TEST
                                    int sensval;
                                    intsensorvals.TryGetValue(sensorarray2[a], out sensval); // get current sensor value
                                    string query4 = "EXEC proc_setalarm @unitid, @alarmid, @date, @value, @text";
                                    if (sensval < alarmlowarray[b] && alarmlowarray[b] != -1) // if current sensor value is less than low limit for alarm, set alarm
                                    {
                                        // if a currently active alarm does not exist for this sensor
                                        //https://stackoverflow.com/questions/13257458/check-if-a-value-is-in-an-array-c

                                        if (!Array.Exists(actalmsensarray, element => element == sensorarray[b]))
                                        {
                                            //MessageBox.Show("Value Low");


                                            using (SqlConnection conn2 = new SqlConnection(connectionString))
                                            {
                                                using (SqlCommand comm2 = new SqlCommand(query4, conn2))
                                                {
                                                    conn2.Open();
                                                    comm2.Parameters.AddWithValue("@unitid", Unit);
                                                    comm2.Parameters.AddWithValue("@alarmid", alarmidarray[b]);
                                                    comm2.Parameters.AddWithValue("@date", DateTime.Now);
                                                    comm2.Parameters.AddWithValue("@value", sensval);
                                                    comm2.Parameters.AddWithValue("@text", "Sensor " + sensorarray2[a].ToString() + " below limit");


                                                    try
                                                    {
                                                        Int32 response = Convert.ToInt32(comm2.ExecuteScalar());
                                                        retalarmid = response; // returned alarm ID
                                                                               //MessageBox.Show("returned alarm id = " + retalarmid.ToString());

                                                        // add entry to dictionary that relates RETURNED alarm ID to sensor ID
                                                        if (!almidsensid.ContainsKey(retalarmid))
                                                            almidsensid.Add(retalarmid, sensorarray2[a]);

                                                    }
                                                    catch (Exception e)
                                                    {
                                                        MessageBox.Show(e.ToString());
                                                    }
                                                }
                                            }
                                        }


                                        // send e-mail messages

                                        // get serial number associated with unit id (to be used in e-mail message)

                                        string query2 = "SELECT [SerialNumber] FROM [VLink106466].[dbo].[VLinkUnit] WHERE ([UnitID] = " + Unit.ToString() + ")";


                                        using (SqlConnection conn3 = new SqlConnection(connectionString))
                                        {
                                            using (SqlCommand comm3 = new SqlCommand(query2, conn3))
                                            {
                                                conn3.Open();


                                                try
                                                {
                                                    SerialNumber = comm3.ExecuteScalar().ToString();

                                                }
                                                catch (Exception e2)
                                                {
                                                    MessageBox.Show(e2.ToString());
                                                }
                                            }
                                        }
                                        // get "OnAction" entry for alarm from database
                                        foreach (DataRow dr in dtalarm.Rows)
                                        {
                                            //if (Convert.ToInt32(dr["AlarmID"]) == alarmidarray[b])
                                            if (Convert.ToInt32(dr["id"]) == alarmidarray[b])
                                            {
                                                string onactstring = dr["OnAction"].ToString();
                                                //string emailtext = dr["Text"].ToString();
                                                ArrayList emailarr = new ArrayList();
                                                string emaildelimiter = string.Empty;
                                                if (onactstring.Contains("EMAIL"))
                                                {
                                                    emaildelimiter = "EMAIL:";
                                                    // get count of email addresses (http://stackoverflow.com/questions/541954/how-would-you-count-occurrences-of-a-string-within-a-string)
                                                    int i = (onactstring.Length - onactstring.Replace(emaildelimiter, "").Length) / emaildelimiter.Length;

                                                    // replace "EMAIL:" string with "/" to allow splitting of string into individual e-mail addresses
                                                    string onactstring2 = onactstring.Replace("EMAIL:", "/");

                                                    // remove "/" delimiter if found at beginning of string (this messes up splitting of string)
                                                    if (onactstring2.StartsWith("/"))
                                                        onactstring2 = onactstring2.Remove(0, 1);


                                                    for (int j = 0; j < i; j++)
                                                    {
                                                        // split string and store individual e-mail addresses in arraylist
                                                        emailarr.Add(onactstring2.Split('/')[j]);

                                                        // remove extraneous CR/LF characters from e-mail addresses
                                                        emailfinal = emailarr[j].ToString().Replace('\r', ' ');
                                                        emailfinal = emailfinal.Replace('\n', ' ');
                                                        emailfinal = emailfinal.Replace(" ", "");



                                                        //create and send e-mail message(s)
                                                        mailObj = new MailMessage(
                                                        "gayakawa@vmcnet.com", emailfinal, "Low Limit Alarm for Sensor " + sensorarray[b].ToString()
                                                        + " of Unit Serial Number " + SerialNumber, "The most recent reading for Sensor " + sensorarray[b].ToString() +
                                                        " is below the low alarm limit for Unit Serial Number " + SerialNumber);

                                                        SmtpClient SMTPServer = new SmtpClient("smtp.bizmail.yahoo.com", 587);
                                                        SMTPServer.EnableSsl = true;
                                                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                                                        SMTPServer.Credentials = new System.Net.NetworkCredential("gayakawa@vmcnet.com", "300cmV5584");

                                                        SMTPServer.Send(mailObj);
                                                    }
                                                }
                                            }

                                        }
                                    }

                                    if (sensval > alarmhigharray[b] && alarmhigharray[b] != -1)    // if current sensor value is greater than high limit for alarm, set alarm
                                    {
                                        // if a currently active alarm does not exist for this sensor
                                        //https://stackoverflow.com/questions/13257458/check-if-a-value-is-in-an-array-c

                                        if (!Array.Exists(actalmsensarray, element => element == sensorarray[b]))

                                        {
                                            //MessageBox.Show("Value High");


                                            using (SqlConnection conn2 = new SqlConnection(connectionString))
                                            {
                                                using (SqlCommand comm2 = new SqlCommand(query4, conn2))
                                                {
                                                    conn2.Open();
                                                    comm2.Parameters.AddWithValue("@unitid", Unit);
                                                    comm2.Parameters.AddWithValue("@alarmid", alarmidarray[b]);
                                                    comm2.Parameters.AddWithValue("@date", DateTime.Now);
                                                    comm2.Parameters.AddWithValue("@value", sensval);
                                                    comm2.Parameters.AddWithValue("@text", "Sensor " + sensorarray2[a].ToString() + " above limit");


                                                    try
                                                    {
                                                        Int32 response = Convert.ToInt32(comm2.ExecuteScalar());
                                                        retalarmid = response; // returned alarm ID
                                                                               //MessageBox.Show("returned alarm id = " + retalarmid.ToString());

                                                        // add entry to dictionary that relates RETURNED alarm ID to sensor ID
                                                        if (!almidsensid.ContainsKey(retalarmid))
                                                            almidsensid.Add(retalarmid, sensorarray2[a]);


                                                    }
                                                    catch (Exception e)
                                                    {

                                                        MessageBox.Show("problem at 3120\r\n" + e.ToString());
                                                    }
                                                }
                                            }
                                        }

                                        // send e-mail messages

                                        // get serial number associated with unit id (to be used in e-mail message)

                                        string query2 = "SELECT [SerialNumber] FROM [VLink106466].[dbo].[VLinkUnit] WHERE ([UnitID] = " + Unit.ToString() + ")";


                                        using (SqlConnection conn3 = new SqlConnection(connectionString))
                                        {
                                            using (SqlCommand comm3 = new SqlCommand(query2, conn3))
                                            {
                                                conn3.Open();


                                                try
                                                {
                                                    SerialNumber = comm3.ExecuteScalar().ToString();

                                                }
                                                catch (Exception e2)
                                                {
                                                    MessageBox.Show(e2.ToString());
                                                }
                                            }
                                        }
                                        // get "OnAction" entry for alarm from database
                                        foreach (DataRow dr in dtalarm.Rows)
                                        {
                                            //if (Convert.ToInt32(dr["AlarmID"]) == alarmidarray[b])
                                            if (Convert.ToInt32(dr["id"]) == alarmidarray[b])
                                            {
                                                string onactstring = dr["OnAction"].ToString();
                                                //string emailtext = dr["Text"].ToString();
                                                ArrayList emailarr = new ArrayList();
                                                string emaildelimiter = string.Empty;
                                                if (onactstring.Contains("EMAIL"))
                                                {
                                                    emaildelimiter = "EMAIL:";
                                                    // get count of email addresses (http://stackoverflow.com/questions/541954/how-would-you-count-occurrences-of-a-string-within-a-string)
                                                    int i = (onactstring.Length - onactstring.Replace(emaildelimiter, "").Length) / emaildelimiter.Length;

                                                    // replace "EMAIL:" string with "/" to allow splitting of string into individual e-mail addresses
                                                    string onactstring2 = onactstring.Replace("EMAIL:", "/");

                                                    // remove "/" delimiter if found at beginning of string (this messes up splitting of string)
                                                    if (onactstring2.StartsWith("/"))
                                                        onactstring2 = onactstring2.Remove(0, 1);


                                                    for (int j = 0; j < i; j++)
                                                    {
                                                        // split string and store individual e-mail addresses in arraylist
                                                        emailarr.Add(onactstring2.Split('/')[j]);

                                                        // remove extraneous CR/LF characters from e-mail addresses
                                                        emailfinal = emailarr[j].ToString().Replace('\r', ' ');
                                                        emailfinal = emailfinal.Replace('\n', ' ');
                                                        emailfinal = emailfinal.Replace(" ", "");



                                                        //create and send e-mail message(s)
                                                        mailObj = new MailMessage(
                                                        "gayakawa@vmcnet.com", emailfinal, "High Limit Alarm for Sensor " + sensorarray[b].ToString()
                                                        + " of Unit Serial Number " + SerialNumber, "The most recent reading for Sensor " + sensorarray[b].ToString() +
                                                        " is above the high alarm limit for Unit Serial Number " + SerialNumber);

                                                        SmtpClient SMTPServer = new SmtpClient("smtp.bizmail.yahoo.com", 587);
                                                        SMTPServer.EnableSsl = true;
                                                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                                                        SMTPServer.Credentials = new System.Net.NetworkCredential("gayakawa@vmcnet.com", "300cmV5584");

                                                        SMTPServer.Send(mailObj);
                                                    }
                                                }
                                            }

                                        }


                                    }
                                }

                            }




                    }
                }
            }
        }





        // check if string is a hex value
        public static bool IsHex(IEnumerable<char> chars)
        {
            bool isHex;
            foreach (var c in chars)
            {
                isHex = ((c >= '0' && c <= '9') ||
                         (c >= 'a' && c <= 'f') ||
                         (c >= 'A' && c <= 'F'));

                if (!isHex)
                    return false;
            }
            return true;
        }

        public static void Send(Socket handler, String data)
        {

            // Convert the string data to byte data using ASCII encoding.
            byte[] byteData = Encoding.ASCII.GetBytes(data);
            //int missingunit;
            //if (missingunit == 0)
            //    units.TryGetValue(handler, out missingunit);

            // Begin sending the data to the remote device.

            try
            {
                if (SocketExtensions.IsConnected(handler))
                {
                handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
                //Outgoing(ipa + " " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + " " + data);
                //Outgoing(handler.RemoteEndPoint.ToString() + "   " + DateTime.Now.ToShortDateString() + " " +
                //    DateTime.Now.ToShortTimeString() + "   " + data);
                string outsernum = "";
                ArrayList senddata = new ArrayList();
                senddata.Clear();
                senddata.AddRange(data.Split(',')); // split message data string at delimiters and store elements in arraylist
                sernum = senddata[1].ToString(); // extract serial number from message
                outsernum = senddata[1].ToString(); // extract serial number from message
                                                    //sernumdict.TryGetValue(handler, out outsernum);
                Outgoing(outsernum + "   " + handler.RemoteEndPoint.ToString() + "   " + DateTime.Now.ToShortDateString() + " " +
                    DateTime.Now.ToShortTimeString() + "   " + data);
//#if TEST
                string outfilename = "C:\\Users\\gayakawa\\desktop\\TCPServer Log\\" + sernum + " outgoing.log";
                //FileInfo outfile = new FileInfo(outfilename);
                //StreamWriter outgoinglog = new StreamWriter(new FileStream(outfilename, FileMode.Append, FileAccess.Write));
                //if (!IsFileLocked(outfile))
                //{
                //    Outgoing("false");
                try
                {
                    StreamWriter outgoinglog = new StreamWriter(new FileStream(outfilename, FileMode.Append, FileAccess.Write));
                    outgoinglog.Write(DateTime.Now + " " + " raw outgoing " + data);
                    outgoinglog.Close();
                }

                catch (Exception e)
                {
                    Outgoing("===============\r\nOutgoing log not updated with the following: " + DateTime.Now + " " + " raw outgoing " + data + "===============\r\n");
                }
                //}
                //else
                //    Outgoing("true");
//#else

                //StreamWriter outgoinglog = new StreamWriter(new FileStream("C:\\Users\\gayakawa\\desktop\\TCPServer Log\\" + "outgoing.log", FileMode.Append, FileAccess.Write));
                //outgoinglog.Write("\r\n" + DateTime.Now + " " + handler.RemoteEndPoint.ToString() + " (Serial Number " + outsernum + ")  " + "raw outgoing " + data);
                //outgoinglog.Close();
//#endif

                }
            }
            catch (Exception e)
            {
                //MessageBox.Show("got to 3176");
                AddUnsentMessage(sernum, "\r\n" + data);

                
                //if (File.Exists("C:\\ProgramData\\TCPServer\\Unsent_Messages.txt"))
                //{
                //    StreamWriter unsent = new StreamWriter(new FileStream("C:\\ProgramData\\TCPServer\\Unsent_Messages.txt", FileMode.Append, FileAccess.Write));
                //    //unsent.Write("\r\n" + missingunit.ToString() + ";" + data);
                //    unsent.Write("\r\n" + data);
                //    unsent.Close();
                //}
                //MessageBox.Show(e.ToString());
            }

            //if (!SocketExtensions.IsConnected(handler))
            //{
            //    //int missingunit;
            //    //units.TryGetValue(handler, out missingunit);
            //    //MessageBox.Show("got to 3183");
            //    AddUnsentMessage(sernum, "\r\n" + data);
            //    //if (File.Exists("C:\\ProgramData\\TCPServer\\Unsent_Messages.txt"))
            //    //{
            //    //    StreamWriter unsent = new StreamWriter(new FileStream("C:\\ProgramData\\TCPServer\\Unsent_Messages.txt", FileMode.Append, FileAccess.Write));
            //    //    unsent.Write("\r\n" + data);
            //    //    unsent.Close();
            //    //}

            //    handler.Shutdown(SocketShutdown.Both);
            //    handler.Disconnect(true);
            //    clientSockets.Remove(handler);
            //    units.Remove(handler);
            //    sernumdict.Remove(handler);
            //    //MessageBox.Show("Lost connection");
            //}
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

        public static void SendTestText(string data)
        {
            string data2 = data + "\r\n";
            Send(handler, data2);

        }

        public static void CloseSocket()
        {
            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
        }

        // send "set time" message
        public static void SendCurrTime(string sn)
        {
            //// reset dictionary value for the connected unit to indicate that log is ready to be modified (this is used for a disconnection log entry below)
            //if (clientSockets.Count != 0) // insure that at least one socket has previously been opened
            //{
            //    if (!disclogmod.ContainsKey(handler))
            //        disclogmod.Add(handler, false);
            //    else
            //    {
            //        disclogmod.Remove(handler);
            //        disclogmod.Add(handler, false);
            //    }
            //}

            //string sernum2 = "";
            //if (clientSockets.Count != 0) // insure that at least one socket has previously been opened
            //{
            //    //for (int a = 0; a < clientSockets.Count; a++)
            //    for (int a = (clientSockets.Count - 1); a > -1; a--) // loop through sockets in reverse since sockets may be removed from clientSockets list during processing
            //    //foreach (var s in clientSockets)
            //    {
            //        handler = clientSockets[a];
            //        if ((SocketExtensions.IsConnected(handler))) // check if socket is connected
            //        {
            //            try
            //            {
            //                // reset dictionary value for the connected unit to indicate that log is ready to be modified (this is used for a disconnection log entry below)
            //                if (!disclogmod.ContainsKey(handler))
            //                    disclogmod.Add(handler, false);
            //                else
            //                {
            //                    disclogmod.Remove(handler);
            //                    disclogmod.Add(handler, false);
            //                }

            //                if (sernumdict.ContainsKey(handler)) //&& sernum2 != ""))                                                                                                         
            //                {
            //                    sernumdict.TryGetValue(handler, out sernum2); // get serial number of connected unit
            //                }
            if (sernumdict2.ContainsKey(sn))
            {
                sernumdict2.TryGetValue(sn, out handler); // get socket associated with serial number of connected unit
            }
            Int32 unixTimecurr = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds; // get current Unix time
            string hexTimecurr = unixTimecurr.ToString("X");
            string data = "{VMC01," + sn + ",59,00," + hexTimecurr.ToString() + "}\r\n";
            Send(handler, data);


            //                //if (sernumdict.ContainsKey(handler)) //&& sernum2 != ""))                                                                                                         
            //                //{
            //                //    sernumdict.TryGetValue(handler, out sernum2); // get serial number of connected unit

            //                //    {
            //                // add log file entry indicating that this unit is still connected

            //                StreamWriter connlog = new StreamWriter(new FileStream("C:\\Users\\gayakawa\\desktop\\TCPServer Log\\" + sernum2 + ".log", FileMode.Append, FileAccess.Write));
            //                connlog.Write("\r\n" + DateTime.Now + " " + handler.RemoteEndPoint.ToString() + " (Serial Number " + sernum2 + ") connected");
            //                connlog.Close();
            //                //    }
            //                //}


            //            }
            //            catch (Exception e)
            //            {
            //                MessageBox.Show(e.ToString());
            //            }


            //        }

            //    else if ((!SocketExtensions.IsConnected(handler))) // check if socket is connected
            //    {
            //        // unit has apparently been disconnected
            //        clientSockets.Remove(handler);
            //        bool logmod;
            //        disclogmod.TryGetValue(handler, out logmod); // check if log file entry has been made for this event

            //        if (logmod == false) // if log file entry has not yet been made
            //        {
            //            if (sernumdict.ContainsKey(handler)) //&& (sernum2 != ""))
            //            {
            //                sernumdict.TryGetValue(handler, out sernum2); // get serial number of disconnected unit
            //                // add log file entry indicating that this unit has been disconnected
            //                //StreamWriter disclog = new StreamWriter(new FileStream("C:\\Users\\gayakawa\\desktop\\TCPServer Log\\disconnect.log", FileMode.Append, FileAccess.Write));
            //                StreamWriter disclog = new StreamWriter(new FileStream("C:\\Users\\gayakawa\\desktop\\TCPServer Log\\" + sernum2 + ".log", FileMode.Append, FileAccess.Write));
            //                disclog.Write("\r\n" + DateTime.Now + " " + handler.RemoteEndPoint.ToString() + " (Serial Number " + sernum2 + ") disconnection detected");
            //                //disclog.Write("\r\n" + DateTime.Now + " " + IPAddress.Parse(((IPEndPoint)handler.RemoteEndPoint).Address.ToString()) + " disconnected");
            //                disclog.Close();
            //            }
            //        }

            //        units.Remove(handler);
            //        sernumdict.Remove(handler);
            //        //MessageBox.Show("NUMBER OF SOCKETS = " + clientSockets.Count.ToString());
            //        // set dictionary value indicating that a log entry has been made for this unit's log file

            //        if (!disclogmod.ContainsKey(handler))
            //            disclogmod.Add(handler, true);
            //        else
            //        {
            //            disclogmod.Remove(handler);
            //            disclogmod.Add(handler, true);
            //        }





            //    }
            //}

        }







        //}

        public static void CheckConnection()
        {
            string sernum2 = "";
            if (clientSockets.Count != 0) // insure that at least one socket has previously been opened
            {
                for (int c = (clientSockets.Count - 1); c > -1; c--) // loop through sockets in reverse since sockets may be removed from clientSockets list during processing
                {
                    Socket handler2 = clientSockets[c];


                    if (!SocketExtensions.IsConnected(handler2)) // if socket is disconnected
                    {
                        //clientSockets.Remove(handler2);

                        //bool logmod;
                        //disclogmod.TryGetValue(handler2, out logmod); // check if log file entry has been made for this event

                        //if (logmod == false) // if log file entry has not yet been made
                        //{
                        //    if (sernumdict.ContainsKey(handler2)) //&& (sernum2 != ""))
                        //    {
                        //        sernumdict.TryGetValue(handler2, out sernum2); // get serial number of disconnected unit
                        //                                                       // add log file entry indicating that this unit has been disconnected
                        //                                                       //StreamWriter disclog = new StreamWriter(new FileStream("C:\\Users\\gayakawa\\desktop\\TCPServer Log\\disconnect.log", FileMode.Append, FileAccess.Write));
                        //        StreamWriter disclog = new StreamWriter(new FileStream("C:\\Users\\gayakawa\\desktop\\TCPServer Log\\" + sernum2 + ".log", FileMode.Append, FileAccess.Write));
                        //        disclog.Write("\r\n" + DateTime.Now + " " + handler2.RemoteEndPoint.ToString() + " (Serial Number " + sernum2 + ") disconnection detected");
                        //        //disclog.Write("\r\n" + DateTime.Now + " " + IPAddress.Parse(((IPEndPoint)handler.RemoteEndPoint).Address.ToString()) + " disconnected");
                        //        disclog.Close();
                        //    }
                        //}



                        // set dictionary value indicating that a log entry has been made for this unit's log file

                        if (!disclogmod.ContainsKey(handler2))
                            disclogmod.Add(handler2, true);
                        else
                        {
                            disclogmod.Remove(handler2);
                            disclogmod.Add(handler2, true);
                        }

                        //if (sernumdict.ContainsKey(handler2))
                        //    sernumdict.Remove(handler2);
                        //if (units.ContainsKey(handler2))
                        //    units.Remove(handler2);

                    }
                }

            }
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public static void AddUnsentMessage(string sn, string data)
        {
            string filename = "C:\\ProgramData\\TCPServer\\Unsent_Messages_" + sn;
            if (!File.Exists(filename + ".txt"))
            {
                StreamWriter unsent = new StreamWriter(new FileStream(filename + ".txt", FileMode.Create, FileAccess.Write));
                unsent.Write("Send Current Time,\r\n" + data);
                unsent.Close();
            }

            else if (File.Exists(filename + ".txt"))
            {
                StreamWriter unsent2 = new StreamWriter(new FileStream(filename + ".txt", FileMode.Append, FileAccess.Write));
                unsent2.Write(data);
                unsent2.Close();
            }

            string outfilename = "C:\\Users\\gayakawa\\desktop\\TCPServer Log\\" + sn + " outgoing.log";

            try
            {
                StreamWriter outgoinglog = new StreamWriter(new FileStream(outfilename, FileMode.Append, FileAccess.Write));
                outgoinglog.Write("*** MESSAGE BELOW WAS NOT SENT SUCCESSFULLY ***\r\n");
                outgoinglog.Close();
            }

            catch (Exception e)
            {
                Outgoing("===============\r\nOutgoing log not updated with the following: " + "*** MESSAGE BELOW WAS NOT SENT SUCCESSFULLY ***" + "\r\n===============\r\n");
            }

                //StreamWriter outgoinglog = new StreamWriter(new FileStream("C:\\Users\\gayakawa\\desktop\\TCPServer Log\\" + sn + " outgoing.log", FileMode.Append, FileAccess.Write));
                //outgoinglog.Write("*** MESSAGE BELOW WAS NOT SENT SUCCESSFULLY ***\r\n");
                //
                //outgoinglog.Close();

        }

        public static string GetSNFromUnitID(int id)
        {
            string SerialNumber = "";
            // get serial number associated with unit id

            string query2 = "SELECT [SerialNumber] FROM [VLink106466].[dbo].[VLinkUnit] WHERE ([UnitID] = " + id.ToString() + ")";


            using (SqlConnection conn3 = new SqlConnection(connectionString))
            {
                using (SqlCommand comm3 = new SqlCommand(query2, conn3))
                {
                    conn3.Open();


                    try
                    {
                        SerialNumber = comm3.ExecuteScalar().ToString();
                    }


                    catch (Exception e2)
                    {
                        MessageBox.Show(e2.ToString());
                    }
                }
            }

            return SerialNumber;
        }


        public static int GetUnitIDFromSN(string sn)
        {
            int unit = 0;
            string unitstr = "";
            // get unit id associated with serial number

            string query2 = "SELECT [UnitID] FROM [VLink106466].[dbo].[VLinkUnit] WHERE ([SerialNumber] = " + "'" + sn.ToString() + "')";
            

            using (SqlConnection conn3 = new SqlConnection(connectionString))
            {
                using (SqlCommand comm3 = new SqlCommand(query2, conn3))
                {
                    conn3.Open();


                    try
                    {
                        unitstr = comm3.ExecuteScalar().ToString();
                        
                    }


                    catch (Exception e2)
                    {
                        MessageBox.Show(e2.ToString());
                    }                                                                                                     
                }
            }

            unit = Convert.ToInt32(unitstr);
            return unit;
        }

        public static int GetUnitIDFromActionID(int action)
        {
            int unit = 0;
            string unitstr = "";
            // get serial number associated with unit id

            string query2 = "SELECT [UnitID] FROM [VLink106466].[dbo].[VLinkActions] WHERE ([id] = " + action.ToString() + ")";


            using (SqlConnection conn3 = new SqlConnection(connectionString))
            {
                using (SqlCommand comm3 = new SqlCommand(query2, conn3))
                {
                    conn3.Open();


                    try
                    {
                        unitstr = comm3.ExecuteScalar().ToString();
                    }


                    catch (Exception e2)
                    {
                        MessageBox.Show(e2.ToString());
                    }
                }
            }

            unit = Convert.ToInt32(unitstr);
            return unit;





        }

        public static void CheckAndSetMode(string sn)
        {
            int modesetint = 0;
            string modestr = "";
            int intervalsetint = 0;
            string intervalstr = "";

            string query = "SELECT [ModeSet] FROM [VLink106466].[dbo].[VLinkUnit] WHERE ([SerialNumber] = '" + sn + "')";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    conn.Open();


                    try
                    {
                        modesetint = Convert.ToInt16(comm.ExecuteScalar());

                    }


                    catch (Exception e2)
                    {
                        MessageBox.Show(e2.ToString());
                    }
                }
            }

            string query2 = "SELECT [Mode] FROM [VLink106466].[dbo].[VLinkUnit] WHERE ([SerialNumber] = '" + sn + "')";

            using (SqlConnection conn2 = new SqlConnection(connectionString))
            {
                using (SqlCommand comm2 = new SqlCommand(query2, conn2))
                {
                    conn2.Open();


                    try
                    {
                        modestr = comm2.ExecuteScalar().ToString();

                    }


                    catch (Exception e2)
                    {
                        MessageBox.Show(e2.ToString());
                    }
                }
            }

            if (modesetint == 0)
            {
                try
                {
                    string opmodeset = "{VMC01," + sernum + ",67,FE," + modestr + "}\r\n";
                    Thread.Sleep(1000);
                    Send(handler, opmodeset);

                    string update = "UPDATE [VLink106466].[dbo].[VLinkUnit] SET ModeSet = 1 WHERE ([SerialNumber] = '" + sn + "')";

                    using (SqlConnection conn5 = new SqlConnection(connectionString))
                    {
                        using (SqlCommand comm5 = new SqlCommand(update, conn5))
                        {
                            conn5.Open();


                            try
                            {
                                comm5.ExecuteNonQuery();

                            }


                            catch (Exception e2)
                            {
                                MessageBox.Show(e2.ToString());
                            }
                        }
                    }
                                        
                }

                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }


            }

            string query3 = "SELECT [IntervalSet] FROM [VLink106466].[dbo].[VLinkUnit] WHERE ([SerialNumber] = '" + sn + "')";

            using (SqlConnection conn3 = new SqlConnection(connectionString))
            {
                using (SqlCommand comm3 = new SqlCommand(query3, conn3))
                {
                    conn3.Open();


                    try
                    {
                        intervalsetint = Convert.ToInt16(comm3.ExecuteScalar());

                    }


                    catch (Exception e2)
                    {
                        MessageBox.Show(e2.ToString());
                    }
                }
            }

            string query4 = "SELECT [Interval] FROM [VLink106466].[dbo].[VLinkUnit] WHERE ([SerialNumber] = '" + sn + "')";

            using (SqlConnection conn4 = new SqlConnection(connectionString))
            {
                using (SqlCommand comm4 = new SqlCommand(query4, conn4))
                {
                    conn4.Open();


                    try
                    {
                        intervalstr = comm4.ExecuteScalar().ToString();

                    }


                    catch (Exception e2)
                    {
                        MessageBox.Show(e2.ToString());
                    }
                }
            }


            if (intervalsetint == 0)
            {
                try
                {
                    string intervalset = "{VMC01," + sernum + ",71,FC," + intervalstr + "}\r\n";
                    Thread.Sleep(1000);
                    Send(handler, intervalset);

                    string update2 = "UPDATE [VLink106466].[dbo].[VLinkUnit] SET IntervalSet = 1 WHERE ([SerialNumber] = '" + sn + "')";

                    using (SqlConnection conn6= new SqlConnection(connectionString))
                    {
                        using (SqlCommand comm6 = new SqlCommand(update2, conn6))
                        {
                            conn6.Open();


                            try
                            {
                                comm6.ExecuteNonQuery();

                            }


                            catch (Exception e2)
                            {
                                MessageBox.Show(e2.ToString());
                            }
                        }
                    }

                }

                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }


            }


        }


        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public static bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }
    }


    static class SocketExtensions
    {
        // detect if client has disconnected -- IsConnected returns false if data cannot be read from socket or a 
        // socket error (resulting in an exception) occurs

        // http://stackoverflow.com/questions/722240/instantly-detect-client-disconnection-from-server-socket

        public static bool IsConnected(this Socket socket)
        {
            try
            {
                return !(socket.Poll(1, SelectMode.SelectRead) && socket.Available == 0);
            }
            catch (SocketException) { return false; }
        }
    }
}

