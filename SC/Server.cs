using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SC
{
	public class Server
	{
		private static ManualResetEvent allDone = new ManualResetEvent(false);
		private Thread listnerThread;
		private Thread cleanupThread;
		private object syncObj = new object();
		private bool bShuttingDown = false;
		public ConnectionInfo connInfo { get; set; }
		public delegate void DataEvent(StateObject s, byte[] data);
		public event DataEvent Data;
		bool isListening = false;
		private List<StateObject> lstStates = new List<StateObject>();
		Socket listener;
		public void shutDown()
		{
			bShuttingDown = true;
			if (isListening)
			{
				foreach (StateObject st in lstStates)
				{
					st.workSocket.Close();
				}
				isListening = false;
				listener.Close();
				listener = null;
			}
		}
		private void startListening()
		{
			// Data buffer for incoming data.
			byte[] bytes = new Byte[1024];

			// Establish the local endpoint for the socket.
			// The DNS name of the computer
			// running the listener is "host.contoso.com".
			IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
			IPAddress ipAddress = ipHostInfo.AddressList[0];
			IPEndPoint localEndPoint = new IPEndPoint(ipAddress, connInfo.serverConnInfo.listenPort);

			// Create a TCP/IP socket.
			listener = new Socket(AddressFamily.InterNetwork,
				SocketType.Stream, ProtocolType.Tcp);
			isListening = true;
			// Bind the socket to the local endpoint and listen for incoming connections.
			try
			{
				listener.Bind(localEndPoint);
				listener.Listen(100);

				while (isListening)
				{
					// Set the event to nonsignaled state.
					allDone.Reset();

					// Start an asynchronous socket to listen for connections.
					Console.WriteLine("Waiting for a connection...");
					listener.BeginAccept(
						new AsyncCallback(AcceptCallback),
						listener);

					// Wait until a connection is made before continuing.
					allDone.WaitOne();
				}

			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}
		}
		private void AcceptCallback(IAsyncResult ar)
		{
			// Signal the main thread to continue.
			allDone.Set();
			if (isListening == false)
				return;
			// Get the socket that handles the client request.
			Socket listener = (Socket)ar.AsyncState;
			Socket handler = listener.EndAccept(ar);

			// Create the state object.
			StateObject state = new StateObject();
			state.msg.MessageComplete += msg_MessageComplete;
			state.workSocket = handler;
			lock (syncObj)
			{
				lstStates.Add(state);
			}
			handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
				new AsyncCallback(ReadCallback), state);
		}
		public int countStates()
		{
			return lstStates.Count;
		}
		void msg_MessageComplete(StateObject st, byte[] msg)
		{
			if (st.authenticated)
			{
				Data(st, msg);
			}
			else
			{
				String key = System.Text.Encoding.UTF8.GetString(msg);
				if (key == connInfo.ownkey)
					st.authenticated = true;
				else
				{
					byte[] data = Encoding.UTF8.GetBytes("Not Authenticated");
					Send(st.workSocket, data);
				}
			}
		}
		private void ReadCallback(IAsyncResult ar)
		{
			try
			{
				String content = String.Empty;

				// Retrieve the state object and the handler socket
				// from the asynchronous state object.
				StateObject state = (StateObject)ar.AsyncState;
				Socket handler = state.workSocket;

				// Read data from the client socket. 
				int bytesRead = handler.EndReceive(ar);

				if (bytesRead > 0)
				{
					state.lastDataReceived = DateTime.Now;
					state.msg.addBytes(state.buffer, 0, bytesRead);
					// There  might be more data, so store the data received so far.

					// Not all data received. Get more.
					handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
					new AsyncCallback(ReadCallback), state);
				}
				else
				{
					lock (syncObj)
					{
						lstStates.Remove(state);
					}
					state = null;
				}
			}
			catch (Exception e)
			{
				Logger.exception(e);
			}
		}

		private void SendCallback(IAsyncResult ar)
		{
			try
			{
				// Retrieve the socket from the state object.
				Socket handler = (Socket)ar.AsyncState;

				// Complete sending the data to the remote device.
				int bytesSent = handler.EndSend(ar);
				Console.WriteLine("Sent {0} bytes to client.", bytesSent);

			}
			catch (Exception e)
			{
				Logger.exception(e);
			}
		}

		public void listen()
		{
			listnerThread = new Thread(startListening);
			listnerThread.Start();
			startCleanup();
		}
		public void startCleanup()
		{
			cleanupThread = new Thread(cleanup);
			cleanupThread.Start();
		}
		public void Send(Socket handler, byte[] data)
		{
			// Convert the string data to byte data using ASCII encoding.
			//byte[] byteData = Encoding.ASCII.GetBytes(data);
			TCPMessage msg = TCPMessage.wrap(data);

			// Begin sending the data to the remote device.
			handler.BeginSend(msg.bytes.ToArray(), 0, msg.bytes.Count, 0,
				new AsyncCallback(SendCallback), handler);
		}
		public void SendToAll(byte[] data)
		{
			// Convert the string data to byte data using ASCII encoding.
			TCPMessage msg = TCPMessage.wrap(data);

			//byte[] byteData = Encoding.ASCII.GetBytes(data);

			foreach (StateObject state in lstStates)
			{
				// Begin sending the data to the remote device.
				state.workSocket.BeginSend(msg.bytes.ToArray(), 0, msg.bytes.Count, 0,
					new AsyncCallback(SendCallback), state.workSocket);

			}
		}
		private void cleanup()
		{
			while (!bShuttingDown)
			{
				lock (syncObj)
				{
					DateTime currTime = DateTime.Now;
					TimeSpan delta = TimeSpan.FromSeconds(10);
					List<StateObject> lstRemovedState = new List<StateObject>();
					foreach (StateObject state in lstStates)
					{
						if (currTime - state.lastDataReceived > delta)
						{
							try
							{
								state.workSocket.Shutdown(SocketShutdown.Both);
								state.workSocket.Close();
								state.workSocket = null;
								lstRemovedState.Add(state);
							}
							catch (System.Exception ex)
							{
								SC.Logger.exception(ex);
							}
						}
					}
					foreach (StateObject state in lstRemovedState)
					{
						lstStates.Remove(state);
					}
					lstRemovedState.Clear();
				}
				Thread.Sleep(10000);
			}
		}
	}

}
