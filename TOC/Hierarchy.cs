using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using MySql.Data.MySqlClient;
using System.Net;
namespace TOC
{
	public partial class Hierarchy : DockContent
	{
		List<DB.Circle> lstCircles;
		TOC toc;
		public Hierarchy(TOC toc)
		{
			this.toc = toc;
			InitializeComponent();
            ContextMenu cm = new ContextMenu();
            cm.MenuItems.Add("Generate");
            this.ContextMenu = cm;
		}
        string getIP(int ip)
        {
            string ipStr = "10.10.0." + ip;
            return ipStr;
        }
        string incrementIP(string prevIP)
        {
            string[] splitIP = prevIP.Split('.');
            splitIP[3] = (Int32.Parse(splitIP[3]) + 1).ToString();
            string ip = splitIP[0] + "." + splitIP[1] + "." + splitIP[2] + "." + splitIP[3];
            return ip;
        }
        SC.ConnectionInfo initializeConnInfo(string ip, int port)
        {
            SC.ConnectionInfo connInfo = new SC.ConnectionInfo();
            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            connInfo.clientConnInfo.serverIP = ipAddress.ToString();//toc server ip
            connInfo.clientConnInfo.serverPort = 12345;//toc server port
            connInfo.serverConnInfo.listenPort = port;
            connInfo.serverConnInfo.ip = ip;
            return connInfo;
        }
        public void generateConnInfo(TreeNode node, SC.ConnectionInfo connInfo)
        {
            if (node.Nodes.Count == 0)
            {
                connInfo.serverConnInfo.listenPort += 1;
                DB.Site s = (DB.Site)node.Tag;
                toc.db.createConnectionInfo(s.key, connInfo);
            }
            else
            {
                int level = node.Level;
                foreach (TreeNode n in node.Nodes)
                {
                    if (level == 1)
                    {
                        connInfo.serverConnInfo.ip = incrementIP(connInfo.serverConnInfo.ip);
                        connInfo.serverConnInfo.listenPort = 8000;
                    }
                    generateConnInfo(n, connInfo);
                }
            }
        }
		public void initialize()
		{
            TreeNode root = new TreeNode("Circles");
            treeCtrl.Nodes.Add(root);
			lstCircles = toc.db.lstCircles;
			foreach (DB.Circle circle in lstCircles)
			{
				TreeNode circleNode = new TreeNode(circle.name);
				circleNode.Tag = circle;
                root.Nodes.Add(circleNode);
				foreach (DB.SSA ssa in circle.vecSSA)
				{
					TreeNode ssaNode = new TreeNode(ssa.name);
					ssaNode.Tag = ssa;
					circleNode.Nodes.Add(ssaNode);
					foreach (DB.SDCA sdca in ssa.vecSDCA)
					{
						TreeNode sdcaNode = new TreeNode(sdca.name);
						sdcaNode.Tag = sdca;
						ssaNode.Nodes.Add(sdcaNode);
						foreach (DB.Site site in sdca.vecSites)
						{
							TreeNode siteNode = new TreeNode(site.name);
							siteNode.Tag = site;
							sdcaNode.Nodes.Add(siteNode);
							//toc.siteWindow.setSite(site);
						}
					}
				}
			}
		}
        private void treeCtrl_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if(e.Node.Nodes.Count == 0 && e.Button == MouseButtons.Left) //site node
            {
                DB.Site site = (DB.Site)e.Node.Tag;
                toc.db.fillSiteInfo(site.key, site);
               // toc.siteWindow.setSite(site);
            }
        }

        private void treeCtrl_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point p = new Point(e.X, e.Y);
                TreeNode node = treeCtrl.GetNodeAt(p);
                if (node != null)
                {
                    SC.ConnectionInfo connInfo = initializeConnInfo(getIP(1), 8000);
                    generateConnInfo(node, connInfo);
                }
            }
        }
	}
}
