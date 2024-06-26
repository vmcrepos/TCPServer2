﻿using System.Text;
using System.Net.Sockets;

namespace TCPServer2
{
    // based on https://msdn.microsoft.com/en-us/library/fx6588te%28v=vs.110%29.aspx

    // State object for reading client data asynchronously

    public class StateObject
    {
        
        // Client  socket.
        public Socket workSocket = null;
        // Size of receive buffer.
        public const int BufferSize = 1024;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder sb = new StringBuilder();
    }
}

