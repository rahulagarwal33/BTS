using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace Sim
{
    public partial class SimTest : Form
    {
        List<DB.Site> lstSites = new List<DB.Site>();
        List<Thread> lstThreads = new List<Thread>();
        Random rnd = new Random();

        public SimTest()
        {
            InitializeComponent();
 
        }
        private List<byte> buildAllSensorData()
        {
            List<byte> bytes = new List<byte>();

            bytes.Add(0);	//format header
            for (int i = 0; i < 10; ++i)
            {
                bytes.Add((byte)i);
				float val = (float)(25 + 50 * rnd.NextDouble());
                byte[] valBytes = BitConverter.GetBytes(val);
                float v = BitConverter.ToSingle(valBytes, 0);
                bytes.AddRange(valBytes);
            }
            return bytes;
        }

        void runSite(DB.Site site)
        {
            while(true)
            {
                site.conn.connect(false);
                List<byte> bytes = buildAllSensorData();
                site.conn.Send(bytes.ToArray(), false);
                site.conn.disconnect();
                Thread.Sleep(60000);
            }
        }
        public void loadSites(string folder)
        {
            string[] files = Directory.GetFiles(folder);
            foreach (string f in files)
            {
                DB.Site site = new DB.Site();
                lstSites.Add(site);
                site.readSite(f);
                site.createConnection();
                site.conn.Conncted += new SC.Connection.ConnectionEventHandler(conn_Conncted);
                site.conn.Disconnected += new SC.Connection.ConnectionEventHandler(conn_Disconnected);
                site.conn.Data += new SC.Connection.DataHandler(conn_Data);
                Thread t = new Thread(()=> runSite(site));
                lstThreads.Add(t);
                t.Start();

                BeginInvoke((Action<int>)((int cnt) =>
                {
                    this.Text = "Total Sites: " + cnt;
                }), lstSites.Count);

                Thread.Sleep((int)(rnd.NextDouble() * 1000));
            }
        }

        void conn_Data(object sender, byte[] data)
        {
            
        }

        void conn_Disconnected(object sender, EventArgs e)
        {
            
        }

        void conn_Conncted(object sender, EventArgs e)
        {
            
        }
        private void SimTest_Load(object sender, EventArgs e)
        {
            Thread t = new Thread(() => loadSites("sts"));
            t.Start();
        }

        private void SimTest_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (Thread t in lstThreads)
            {
                try
                {
                    t.Abort();
                }
                catch (Exception e1)
                {

                }
            }
            foreach (DB.Site s in lstSites)
            {
                try
                {
                    if (s.conn != null)
                    {
                        s.conn.disconnect();
                    }
                }
                catch (Exception e1)
                {

                }
            }
        }
    }
}
