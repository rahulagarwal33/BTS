using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SC
{
	public class StateObject
	{
		// Client socket.
		public Socket workSocket = null;
		// Size of receive buffer.
		public const int BufferSize = 256;
		// Receive buffer.
		public byte[] buffer = new byte[BufferSize];
		public TCPMessage msg;
		//Authenticated status
		public bool authenticated = false;
		public Object obj;
		public DateTime lastDataReceived;
		public StateObject()
		{
			msg = new TCPMessage(this);
			lastDataReceived = DateTime.Now;
		}
	}
	public class Connection
	{
		public delegate void ConnectionEventHandler(object sender, EventArgs e);
		public delegate void DataHandler(object sender, byte[] data);
		private static ManualResetEvent connectedEvent = new ManualResetEvent(false);
		public ConnectionInfo connInfo { get; set; }
		public enum Status
		{
			CONNECTED,
			DISCONNECTED,
		};
		private Status stat { get; set; }
		private object site { get; set; }
		private StateObject socketState;
		public Connection(object site_)
		{
			site = site_;
		}
		~Connection()
		{
			socketState = null;
		}
		void connectByTCP(bool bAsync)
		{
			try
			{
				connectedEvent.Reset();
				// Establish the remote endpoint for the socket.
				// The name of the 
				// remote device is "host.contoso.com".
				IPHostEntry ipHostInfo = Dns.Resolve(connInfo.clientConnInfo.serverIP);
				IPAddress ipAddress = ipHostInfo.AddressList[0];
				IPEndPoint remoteEP = new IPEndPoint(ipAddress, connInfo.clientConnInfo.serverPort);

				// Create a TCP/IP socket.
				Socket client = new Socket(AddressFamily.InterNetwork,
					SocketType.Stream, ProtocolType.Tcp);

				socketState = new StateObject();
				socketState.msg.MessageComplete += msg_MessageComplete;
				// Connect to the remote endpoint.
				client.BeginConnect(remoteEP,
					new AsyncCallback(ConnectCallback), client);
				if (bAsync == false)
				{
					connectedEvent.WaitOne(TimeSpan.FromMinutes(5));
				}
			}
			catch (Exception e)
			{
				SC.Logger.exception(e);
			}
		}

		void msg_MessageComplete(StateObject stOb, byte[] msg)
		{
			Data(this, msg);
		}
		private void ConnectCallback(IAsyncResult ar)
		{
			try
			{
				// Retrieve the socket from the state object.
				Socket client = (Socket)ar.AsyncState;

				// Complete the connection.
				client.EndConnect(ar);

				socketState.workSocket = client;
				Conncted(this, null);
				Console.WriteLine("Socket connected to {0}",
					client.RemoteEndPoint.ToString());
				//send the password
				Send(Encoding.UTF8.GetBytes(connInfo.remoteKey), false);
				Send(Encoding.UTF8.GetBytes(connInfo.ownID), false);
				connectedEvent.Set();
				//start receiving data
				client.BeginReceive(socketState.buffer, 0, StateObject.BufferSize, 0,
				new AsyncCallback(ReceiveCallback), socketState);
			}
			catch (Exception e)
			{
				Thread.Sleep(1000);
				connect();
				Console.WriteLine(e.ToString());
			}
		}
		private void ReceiveCallback(IAsyncResult ar)
		{
			try
			{
				// Retrieve the state object and the client socket 
				// from the asynchronous state object.
				StateObject state = (StateObject)ar.AsyncState;
				Socket client = state.workSocket;

				// Read data from the remote device.
				int bytesRead = client.EndReceive(ar);

				if (bytesRead > 0)
				{
					state.lastDataReceived = DateTime.Now;
					state.msg.addBytes(state.buffer, 0, bytesRead);

					// Get the rest of the data.
					client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
						new AsyncCallback(ReceiveCallback), state);
				}
				else
				{
					disconnect();
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
				disconnect();
			}
		}
		public void Send(byte[] data, bool bAsync)
		{
			// Convert the string data to byte data using ASCII encoding.
			TCPMessage msg = TCPMessage.wrap(data);
			try
			{
				if (bAsync)
				{
					// Begin sending the data to the remote device.
					socketState.workSocket.BeginSend(msg.bytes.ToArray(), 0, msg.bytes.Count, 0,
						new AsyncCallback(SendCallback), socketState.workSocket);
				}
				else
				{
					socketState.workSocket.Send(msg.bytes.ToArray(), 0, msg.bytes.Count, 0);
				}
			}
			catch (Exception e)
			{
				SC.Logger.exception(e);
			}
		}

		private void SendCallback(IAsyncResult ar)
		{
			try
			{
				// Retrieve the socket from the state object.
				Socket client = (Socket)ar.AsyncState;

				// Complete sending the data to the remote device.
				int bytesSent = client.EndSend(ar);
				Console.WriteLine("Sent {0} bytes to server.", bytesSent);

				// Signal that all bytes have been sent.
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
				disconnect();
			}
		}

		public bool connect(bool bAsync = true)
		{
			connectByTCP(bAsync);
			return true;
		}
		public void disconnect()
		{
			if (socketState != null)
			{
				StateObject s = socketState;
				socketState = null;
				Disconnected(this, null);
				if (s.workSocket != null)
				{
					s.workSocket.Shutdown(SocketShutdown.Both);
					s.workSocket.Close();
					s.workSocket = null;
				}
				s = null;
			}
		}

		public event ConnectionEventHandler Conncted;
		public event ConnectionEventHandler Disconnected;
		public event DataHandler Data;

	}
}
