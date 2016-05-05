using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace SC
{
    public class ServerConnectionInfo
    {
        public ServerConnectionInfo() { }
        public ServerConnectionInfo(XmlNode node)
        {
            XmlNode tcpNode = node.SelectSingleNode("TCP");
            listenPort = Int32.Parse(tcpNode.Attributes["ListenPort"].Value);
            ip = tcpNode.Attributes["IP"].Value;
        }
        public void AddToXMLNode(XmlElement elem)
        {
            XmlDocument doc = elem.OwnerDocument;
            XmlElement tcpElem = doc.CreateElement("TCP");
            elem.AppendChild(tcpElem);

            XmlAttribute attrib1 = doc.CreateAttribute("ListenPort");
            attrib1.Value = listenPort.ToString();
            tcpElem.Attributes.Append(attrib1);
            XmlAttribute attrib2 = doc.CreateAttribute("IP");
            attrib2.Value = ip;
            tcpElem.Attributes.Append(attrib2);
        }
        public int listenPort;
        public string ip;
    }
    public class ClientConnectionInfo
    {
        public ClientConnectionInfo() { }
        public ClientConnectionInfo(XmlNode node)
        {
            XmlNode tcpNode = node.SelectSingleNode("TCP");
            serverIP = tcpNode.Attributes["ServerIP"].Value;
            serverPort = Int32.Parse(tcpNode.Attributes["ServerPort"].Value);
        }
        public void AddToXMLNode(XmlElement elem)
        {
            XmlDocument doc = elem.OwnerDocument;
            XmlElement tcpElem = doc.CreateElement("TCP");
            elem.AppendChild(tcpElem);

            XmlAttribute attrib1 = doc.CreateAttribute("ServerIP");
            attrib1.Value = serverIP.ToString();
            XmlAttribute attrib2 = doc.CreateAttribute("ServerPort");
            attrib2.Value = serverPort.ToString();
            tcpElem.Attributes.Append(attrib1);
            tcpElem.Attributes.Append(attrib2);
        }
        public string serverIP;
        public int serverPort;
    }
    public class ConnectionInfo
    {
        public ServerConnectionInfo serverConnInfo { get; set; }
        public ClientConnectionInfo clientConnInfo { get; set; }
		public string ownkey;
		public string remoteKey;
        public string ownID;
        public ConnectionInfo()
        {
            serverConnInfo = new ServerConnectionInfo();
            clientConnInfo = new ClientConnectionInfo();
        }
		public void read(XmlNode node)
		{
            XmlNode serverNode = node.SelectSingleNode("Server");
            if (serverNode != null)
                serverConnInfo = new ServerConnectionInfo(serverNode);
            XmlNode clientNode = node.SelectSingleNode("Client");
            if (clientNode != null)
                clientConnInfo = new ClientConnectionInfo(clientNode);
            ownkey = node.SelectSingleNode("Key").Attributes["Own"].Value;
			remoteKey = node.SelectSingleNode("Key").Attributes["Remote"].Value;
		}
        public void read(string xmlTxt)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlTxt);
            XmlNode infoNode = xmlDoc.GetElementsByTagName("ConnectionInfo")[0];
            read(infoNode);
        }
        public void write(XmlNode node)
        {
            XmlDocument xmlDoc = node.OwnerDocument;
            XmlElement clientConnInfoElem = xmlDoc.CreateElement("Client");
            clientConnInfo.AddToXMLNode(clientConnInfoElem);
            XmlElement serverConnInfoElem = xmlDoc.CreateElement("Server");
            serverConnInfo.AddToXMLNode(serverConnInfoElem);
            node.AppendChild(serverConnInfoElem);
            node.AppendChild(clientConnInfoElem);

            XmlElement keyElem = xmlDoc.CreateElement("Key");
            XmlAttribute attrib1 = xmlDoc.CreateAttribute("Own");
            attrib1.Value = ownkey.ToString(); keyElem.Attributes.Append(attrib1);
            XmlAttribute attrib2 = xmlDoc.CreateAttribute("Remote");
            attrib2.Value = remoteKey.ToString(); keyElem.Attributes.Append(attrib2);
            node.AppendChild(keyElem);
        }
        public string ToXML()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement connInfoElem = xmlDoc.CreateElement("ConnectionInfo");
            write(connInfoElem);
            xmlDoc.AppendChild(connInfoElem);

            StringWriter writer = new StringWriter();
            XmlWriter writerXML = XmlWriter.Create(writer);
            xmlDoc.WriteTo(writerXML);
            writerXML.Flush();
            String str = writer.GetStringBuilder().ToString();
            return str;
        }
	}
}
