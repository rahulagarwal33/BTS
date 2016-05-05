﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Net.Sockets;
namespace DB
{
	public class Site
	{
        public string id { get; set; }
		public string name { get; set; }
		public Address addr { get; set; }
        public UInt32 key { get; set; }
        public SC.ConnectionInfo connInfo { get; set; }
		public DateTime creationTime { get; set; }
		public SDCA sdca { get; set; }

		public SC.Connection conn { get; set; }
		public SC.Server listener { get; set; }
		private bool connected { get; set; }

        public delegate void UpdateDataHandler(byte[] data);
        public event UpdateDataHandler UpdateData;
		public void readSite(string infoFile)
		{
			XmlDocument xmlDoc = new XmlDocument();
			FileStream fs = new FileStream(infoFile, FileMode.Open, FileAccess.Read);
			xmlDoc.Load(fs);
			XmlNode infoNode = xmlDoc.GetElementsByTagName("info")[0];
			name = infoNode.Attributes["Name"].Value;
            id = infoNode.Attributes["ID"].Value;
			XmlNode addrNode = infoNode.SelectSingleNode("Address");

			addr = new Address();
			addr.read(addrNode);

			XmlNode connInfoNode = infoNode.SelectSingleNode("ConnectionInfo");
			connInfo = new SC.ConnectionInfo();
			connInfo.read(connInfoNode);
		}
		public SC.Connection createConnection()
		{
			if (conn == null)
			{
				conn = new SC.Connection(this);
				conn.connInfo = connInfo;
                conn.connInfo.ownID = id;
			}
			return conn;
		}
		public SC.Server createListener()
		{
			if (listener == null)
			{
				listener = new SC.Server();
				listener.connInfo = connInfo;
			}
			return listener;
		}

		void conn_Data(string data)
		{
			throw new NotImplementedException();
		}

		void conn_Disconnected(object sender, EventArgs e)
		{
			connected = false;
			conn = null;
		}

		void conn_Conncted(object sender, EventArgs e)
		{
			connected = true;
		}

		void getCurrentStatus()
		{
		}

		//update the data to the database
        public void updateData(byte[] data)
        {
            UpdateData(data);
        }
	}
}