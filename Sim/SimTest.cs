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
                site.readSite(f);
                site.createConnection();
                Thread t = new Thread(()=> runSite(site));
                t.Start();
            }
        }
        private void SimTest_Load(object sender, EventArgs e)
        {
            loadSites("sts");
        }
    }
}
