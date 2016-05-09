using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using MySql.Data.MySqlClient;
using System.Data;

namespace DB
{
	public class SDCA
	{
		public string name { get; set; }
		public string addr { get; set; }
		public SSA ssa { get; set; }
		public void readInfo(string infoFile)
		{
			XmlDocument xmlDoc = new XmlDocument();
			FileStream fs = new FileStream(infoFile, FileMode.Open, FileAccess.Read);
			xmlDoc.Load(fs);
			XmlNode node = xmlDoc.GetElementsByTagName("info")[0];
			name = node.Attributes["Name"].Value;
			addr = node.Attributes["Address"].Value;
		}
		public Site insertSite(string name)
		{
			foreach (Site site in vecSites)
			{
				if (site.name == name)
					return site;
			}
			Site site1 = new Site();
			site1.name = name;
			vecSites.Add(site1);
			return site1;
		}
		public void readSDCA(string sdcaDir)
		{
			readInfo(sdcaDir + "/info.xml");
			string[] sites = Directory.GetFiles(sdcaDir);
			foreach (string site in sites)
			{
				if (site.LastIndexOf("info.xml") == -1)
				{
					Site s = new Site();
					s.sdca = this;
					s.readSite(site);
					vecSites.Add(s);
				}
			}
		}
		public List<DB.Site> vecSites = new List<DB.Site>();
	}
	public class SSA
	{
		public string name { get; set; }
		public string addr { get; set; }
		public Circle circle { get; set; }
		public SDCA insertSDCA(string name)
		{
			foreach (SDCA sdca in vecSDCA)
			{
				if (sdca.name == name)
					return sdca;
			}
			SDCA sdca1 = new SDCA();
			sdca1.name = name;
			vecSDCA.Add(sdca1);
			return sdca1;
		}
		public void readInfo(string infoFile)
		{
			XmlDocument xmlDoc = new XmlDocument();
			FileStream fs = new FileStream(infoFile, FileMode.Open, FileAccess.Read);
			xmlDoc.Load(fs);
			XmlNode node = xmlDoc.GetElementsByTagName("info")[0];
			name = node.Attributes["Name"].Value;
			addr = node.Attributes["Address"].Value;
		}
		public void readSSA(string ssaDir)
		{
			readInfo(ssaDir + "/info.xml");
			string[] sdcaDir = Directory.GetDirectories(ssaDir);
			foreach (string sdca in sdcaDir)
			{
				SDCA s = new SDCA();
				s.ssa = this;
				s.readSDCA(sdca);
				vecSDCA.Add(s);
			}
		}
		public List<SDCA> vecSDCA = new List<SDCA>();
	}
	public class Circle : PropHolder
	{
		public string name { get; set; }
		public string addr { get; set; }
		public void readInfo(string infoFile)
		{
			XmlDocument xmlDoc = new XmlDocument();
			FileStream fs = new FileStream(infoFile, FileMode.Open, FileAccess.Read);
			xmlDoc.Load(fs);
			XmlNode node = xmlDoc.GetElementsByTagName("info")[0];
			name = node.Attributes["Name"].Value;
			addr = node.Attributes["Address"].Value;
		}
		public SSA insertSSA(string name)
		{
			foreach (SSA ssa in vecSSA)
			{
				if (ssa.name == name)
					return ssa;
			}
			SSA ssa1 = new SSA();
			ssa1.name = name;
			vecSSA.Add(ssa1);
			return ssa1;
		}
		public void readCircle(string circleDir)
		{
			readInfo(circleDir + "/info.xml");
			string[] ssaDir = Directory.GetDirectories(circleDir);
			foreach (string ssa in ssaDir)
			{
				SSA s = new SSA();
				s.circle = this;
				s.readSSA(ssa);
				vecSSA.Add(s);
			}
		}
		public void enumProperties(PropertyBag bag)
		{
			bag.addProperty("Circle", new Property("Name", null));
		}
		public bool getProperty(Property prop)
		{
			switch (prop.name)
			{
				case "Name":
					{
						prop.value = name;
						break;
					}
				default:
					{
						return false;
					}
			}
			return true;
		}
		public bool setProperty(Property prop)
		{
			switch (prop.name)
			{
				case "Name":
					{
						name = (string)prop.value;
						break;
					}
				default:
					{
						return false;
					}
			}
			return true;
		}
		public List<SSA> vecSSA = new List<SSA>();
	}
	public class Hierarchy
	{
		public static List<Circle> buildHierarchy(string circleRootDir)
		{
			List<Circle> lst = new List<Circle>();
			try
			{
				string[] circles = Directory.GetDirectories(circleRootDir);
				foreach (string circle in circles)
				{
					Circle c = new Circle();
					c.readCircle(circle);
					lst.Add(c);
				}
			}
			catch (System.Exception e)
			{
				SC.Logger.exception(e);
			}
			return lst;
		}
<<<<<<< HEAD
        public static List<Circle> buildHierarchyFromDB(SDB db)
        {
            List<Circle> lst = new List<Circle>();
            try
            {
                string statement = "SELECT * FROM `sites` order by `id`";
                DataSet data = MySqlHelper.ExecuteDataset(db.connString, statement);
                foreach (DataRow row in data.Tables[0].Rows)
                {
                    Circle c = insertCircle(row["circle"].ToString(), lst);
                    SSA ssa = c.insertSSA(row["ssa"].ToString());
                    SDCA sdca = ssa.insertSDCA(row["sdca"].ToString());
                    Site site = sdca.insertSite(row["site"].ToString());
                    site.key = UInt32.Parse(row["sitekey"].ToString());
                    site.name = row["site"].ToString();
                    site.creationTime = DateTime.Parse(row["date_created"].ToString());
                    site.addr = new Address();
                    site.addr.addrLine1 = row["address"].ToString();
                    site.addr.lattitude = double.Parse(row["latitude"].ToString());
                    site.addr.longitude = double.Parse(row["longitude"].ToString());
                    site.addr.city = row["city"].ToString();
                    site.addr.state = row["state"].ToString();
                    site.addr.pincode = row["pincode"].ToString();
                    string conn_info = row["conn_info"].ToString();
                    if(conn_info != "")
                    {
                        site.connInfo = new SC.ConnectionInfo();
                        site.connInfo.read(conn_info);
                    }
                    else
                    {
                        int k = 1;
                    }
                }
            }
            catch (System.Exception e)
            {
				SC.Logger.exception(e);
            }
            return lst;
        }
        static Circle insertCircle(string name, List<Circle> lst)
        {
            foreach (Circle c in lst)
            {
                if (c.name == name)
                    return c;
            }
            Circle c1 = new Circle();
            c1.name = name;
            lst.Add(c1);
            return c1;
        }
=======
		public static List<Circle> buildHierarchyFromDB(SDB db)
		{
			List<Circle> lst = new List<Circle>();
			try
			{
				string statement = "SELECT * FROM `sites` order by `id`";
				DataSet data = MySqlHelper.ExecuteDataset(db.connString, statement);
				foreach (DataRow row in data.Tables[0].Rows)
				{
					Circle c = insertCircle(row["circle"].ToString(), lst);
					SSA ssa = c.insertSSA(row["ssa"].ToString());
					SDCA sdca = ssa.insertSDCA(row["sdca"].ToString());
					Site site = sdca.insertSite(row["site"].ToString());
					site.key = UInt32.Parse(row["sitekey"].ToString());
					site.name = row["site"].ToString();
					site.creationTime = DateTime.Parse(row["date_created"].ToString());
					site.addr = new Address();
					site.addr.addrLine1 = row["address"].ToString();
					site.addr.lattitude = double.Parse(row["latitude"].ToString());
					site.addr.longitude = double.Parse(row["longitude"].ToString());
					site.addr.city = row["city"].ToString();
					site.addr.state = row["state"].ToString();
					site.addr.pincode = row["pincode"].ToString();
					string conn_info = row["conn_info"].ToString();
					if (conn_info != "")
					{
						site.connInfo = new SC.ConnectionInfo();
						site.connInfo.read(conn_info);
					}
					else
					{
						int k = 1;
					}
				}
			}
			catch (System.Exception e)
			{
				SC.Logger.exception(e);
			}
			return lst;
		}
		static Circle insertCircle(string name, List<Circle> lst)
		{
			foreach (Circle c in lst)
			{
				if (c.name == name)
					return c;
			}
			Circle c1 = new Circle();
			c1.name = name;
			lst.Add(c1);
			return c1;
		}
>>>>>>> origin/master
	}

}
