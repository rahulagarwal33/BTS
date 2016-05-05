using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MySql.Data;
using System.Xml;
using System.IO;
using System.Data.SqlClient;
using System.Data;

namespace DB
{
	public class SDB
	{
        private BatchQuery createConnInfoQuery = new BatchQuery();
        private BatchQuery addSitesQuery = new BatchQuery();
        private BatchQuery addSiteRawDataQuery = new BatchQuery();
        private BatchQuery addSiteSensorDataQuery = new BatchQuery();

        private static string previousSiteKey;
		private string server;
		private string user;
		private string passwd;
		private string dbname;
		public SC.Server connListner;
        private SC.ConnectionInfo connInfo;
		//private MySqlConnection conn = new MySqlConnection();
        public string connString;
        public List<Circle> lstCircles;
		public SDB()
		{
		}
        public void read(string infoFile)
        {
            XmlDocument xmlDoc = new XmlDocument();
            FileStream fs = new FileStream(infoFile, FileMode.Open, FileAccess.Read);
            xmlDoc.Load(fs);
            XmlNode infoNode = xmlDoc.GetElementsByTagName("info")[0];
            XmlNode databaseNode = infoNode.SelectSingleNode("Database");
            server = databaseNode.Attributes["Server"].Value;
            dbname = databaseNode.Attributes["DB"].Value;
            user = databaseNode.Attributes["User"].Value;
            passwd = databaseNode.Attributes["Password"].Value;

            XmlNode connInfoNode = infoNode.SelectSingleNode("ConnectionInfo");
            connInfo = new SC.ConnectionInfo();
            connInfo.read(connInfoNode);
            connect();
            //lstCircles = Hierarchy.buildHierarchy("Data/circles");
            lstCircles = Hierarchy.buildHierarchyFromDB(this);
        }
		public SC.Server createListener()
		{
			if (connListner == null)
            {
                connListner = new SC.Server();
                connListner.Data += connListner_Data;
                connListner.connInfo = connInfo;
            }

			return connListner;
		}
        Site getSite(string key)
        {
            foreach (Circle c in lstCircles)
            {
                foreach (SSA ssa in c.vecSSA)
                {
                    foreach (SDCA sdca in ssa.vecSDCA)
                    {
                        foreach (Site site in sdca.vecSites)
                        {
                            if (site.id == key)
                                return site;
                        }
                    }
                }
            }
            return null;
        }
	    int getSiteId(string key)
	    {
            List<MySqlParameter> paramList = new List<MySqlParameter>();
            paramList.Add(new MySqlParameter("@SiteId", key));
            int id = -1;
            try
            {
                object ret = MySqlHelper.ExecuteScalar(connString, "SELECT id From sites WHERE siteid = @SiteId", paramList.ToArray());
                if (ret != null)
                {
                    id = Convert.ToInt32(ret);
                }
            }
            catch (Exception e)
            {
            }
			return id;
	    }
	    void updateSensorData(int siteId, int sensorId, float value)
		{
            List<MySqlParameter> paramList = new List<MySqlParameter>();
            paramList.Add(new MySqlParameter("@SensorId", sensorId));
            paramList.Add(new MySqlParameter("@SiteId", siteId));
            paramList.Add(new MySqlParameter("@Value", value));
            paramList.Add(new MySqlParameter("@TimeStamp", DateTime.Now));
            try
            {
                int ret = MySqlHelper.ExecuteNonQuery(connString, "INSERT INTO sensors_data(sensorid, siteid, value, timestamp) VALUE(@SensorID, @SiteId, @Value, @TimeStamp)", 
                    paramList.ToArray());
            }
            catch (Exception e)
            {
            }
       	}
		void updateSiteData(int siteId, byte[] data)
        {
            {
                List<MySqlParameter> paramList = new List<MySqlParameter>();
                paramList.Add(new MySqlParameter("@SiteId", siteId));
                MySqlParameter dataParam = new MySqlParameter("@Data", MySqlDbType.Blob, data.Length); dataParam.Value = data;
                paramList.Add(dataParam);
                paramList.Add(new MySqlParameter("@TimeStamp", DateTime.Now));
                addSiteRawDataQuery.addValues(paramList);
            }
            //parse this data into parts depending on the format of the data
            List<Tuple<int, float>> lst = new List<Tuple<int, float>>();
            DataParser parser = new DataParser();
            parser.SensorData += (int sensorId, float value) =>
            {
                lst.Add(new Tuple<int, float>(sensorId, value));
            };
            parser.parse(data);

            for (int i = 0; i < lst.Count; ++i)
            {
                List<MySqlParameter> paramList = new List<MySqlParameter>();
                paramList.Clear();
                paramList.Add(new MySqlParameter("@SensorId" + i, lst[i].Item1));
                paramList.Add(new MySqlParameter("@SiteId" + i, siteId));
                paramList.Add(new MySqlParameter("@Value" + i, lst[i].Item2));
                paramList.Add(new MySqlParameter("@TimeStamp" + i, DateTime.Now));
                addSiteSensorDataQuery.addValues(paramList);
            }
        }

		//listening data from the site
		void connListner_Data(SC.StateObject s, byte[] data)
		{
            if (s.obj != null)
            {
                //((Site)s.obj).updateData(data);
				updateSiteData((int)s.obj, data);
		    }
            else
            {
                //search for the site with key as data
                //s.obj = getSite(data);
				s.obj = getSiteId(System.Text.Encoding.UTF8.GetString(data));
            }
		}
		public bool connect()
		{
            connString = "SERVER=" + server + ";DATABASE=" + dbname + ";UID=" + user + ";PASSWORD=" + passwd + ";";
            addSiteRawDataQuery.connString = connString;
            addSiteRawDataQuery.statement = "INSERT INTO sites_data(siteid, data, timestamp) VALUE({0}, {1}, {2});";

            addSiteSensorDataQuery.connString = connString;
            addSiteSensorDataQuery.statement = "INSERT INTO sensors_Data(sensorid, siteid, value, timestamp) VALUE({0}, {1}, {2}, {3});";

            addSitesQuery.connString = connString;
			addSitesQuery.statement = "INSERT INTO sites(sitekey, site, sdca, ssa, circle, address, city, state, pincode, latitude, longitude, date_created) VALUE({0}, {1}, {3}, {4}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11});";

			createConnInfoQuery.connString = connString;
            createConnInfoQuery.statement = "UPDATE sites SET conn_info={0} WHERE sitekey={1};";
                
            return true;
		}
		public void addSite(Site site)
		{
            string siteKey = site.name + site.sdca.name + site.sdca.ssa.name + site.sdca.ssa.circle.name + site.addr.pincode;
            if(previousSiteKey != siteKey)
            {
                Crc32 crc32 = new Crc32();
                byte[] arr = Encoding.ASCII.GetBytes(siteKey);
                byte[] hash = crc32.ComputeHash(arr);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(hash);
                UInt32 key = BitConverter.ToUInt32(hash, 0);
            
                List<MySqlParameter> listPara = new List<MySqlParameter>();
                listPara.Add(new MySqlParameter("1", key));
                listPara.Add(new MySqlParameter("2", site.name));
                listPara.Add(new MySqlParameter("3", site.sdca.name));
                listPara.Add(new MySqlParameter("4", site.sdca.ssa.name));
                listPara.Add(new MySqlParameter("5", site.sdca.ssa.circle.name));
                listPara.Add(new MySqlParameter("6", site.name + "\r\n" + site.sdca.name));
                listPara.Add(new MySqlParameter("7", site.sdca.ssa.name));
                listPara.Add(new MySqlParameter("8", site.sdca.ssa.circle.name));
                listPara.Add(new MySqlParameter("9", site.addr.pincode));
                listPara.Add(new MySqlParameter("10", site.addr.lattitude));
                listPara.Add(new MySqlParameter("11", site.addr.longitude));
                listPara.Add(new MySqlParameter("12", DateTime.Now));
                addSitesQuery.addValues(listPara);
            }
            previousSiteKey = siteKey;
        }
        public void createConnectionInfo(UInt32 sitekey, SC.ConnectionInfo connInfo)
        {
            try
            {
				connInfo.ownkey = sitekey.ToString();
				connInfo.remoteKey = "12345";
				string connInfoStr = connInfo.ToXML();
				string statement = "UPDATE sites SET conn_info=" + connInfoStr  + " WHERE sitekey=" + sitekey;
                List<MySqlParameter> param = new List<MySqlParameter>();
				param.Add(new MySqlParameter("@ConnInfo", connInfoStr));
				param.Add(new MySqlParameter("@SiteKey", sitekey));
				createConnInfoQuery.addValues(param);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\r\n" + e.StackTrace);
            }
        }
        public void fillSiteInfo(UInt32 key, Site site)
        {
            try
            {
                string statement = "SELECT * FROM sites WHERE sitekey=" + key;
                DataSet data = MySqlHelper.ExecuteDataset(connString, statement);
                if (data.Tables[0].Rows.Count == 1) //only one row should come
                {
                    DataRow row = data.Tables[0].Rows[0];
                    site.key = key;
                    site.name = row["site"].ToString();
                    site.creationTime = DateTime.Parse(row["date_created"].ToString());
                    site.addr = new Address();
                    site.addr.addrLine1 = row["address"].ToString();
                    site.addr.lattitude = double.Parse(row["latitude"].ToString());
                    site.addr.longitude = double.Parse(row["longitude"].ToString());
                    site.addr.city = row["city"].ToString();
                    site.addr.state = row["state"].ToString();
                    site.addr.pincode = row["pincode"].ToString();
                    site.connInfo = new SC.ConnectionInfo();
                    site.connInfo.read(row["connection_info"].ToString());
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\r\n" + e.StackTrace);
            }
        }
	}
}
