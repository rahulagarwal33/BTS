using Microsoft.VisualBasic.FileIO;
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

namespace TOC
{
	public partial class Site : DockContent
	{
        private TOC toc;
        private DB.Site site;
        public DB.Site getSite() {return site;}
        public void setSite(DB.Site value)
        {
            if (site != null)
            {
                if (site.conn != null)
                {
                    site.conn.Conncted -= conn_Conncted;
                    site.conn.Disconnected -= conn_Disconnected;
                    site.conn.Data -= conn_Data;
                    site.UpdateData -= site_UpdateData;
                    site.conn.disconnect();
                }
            }
            site = value;
            if (site != null)
            {
                site.conn = site.createConnection();
                site.conn.Conncted += conn_Conncted;
                site.conn.Disconnected += conn_Disconnected;
                site.conn.Data += conn_Data;
                site.UpdateData += site_UpdateData;
                site.conn.connect();
            }
        }
        public Site(TOC toc)
        {
            this.toc = toc;
            InitializeComponent();
        }
        void site_UpdateData(byte[] data)
        {
            if(this.InvokeRequired)
            {
                BeginInvoke((Action<byte[]>)((d) =>
                {
                    txtServerReceiveData.Text += Encoding.UTF8.GetString(d) + "\r\n";
                }), data);
            }
            else
                txtServerReceiveData.Text += data + "\r\n";
        }
        void conn_Data(byte[] data)
        {
            if(this.InvokeRequired)
            {
                BeginInvoke((Action<byte[]>)((byte[] d) =>
                {
                    txtClientReceiveData.Text += Encoding.UTF8.GetString(d) + "\r\n";
                }), data);
            }
            else
                txtClientReceiveData.Text += Encoding.UTF8.GetString(data) + "\r\n";
        }

        void conn_Disconnected(object sender, EventArgs e)
        {
            if(this.InvokeRequired)
            {
                BeginInvoke((Action)(() =>
                {
                    connectionStatus.Checked = false;
                    connectionStatus.Text = "Disconnected";
                }));
            }
            else
            {
                connectionStatus.Checked = false;
                connectionStatus.Text = "Disconnected";
            }
        }

        void conn_Conncted(object sender, EventArgs e)
        {
            if(this.InvokeRequired)
            {
                BeginInvoke((Action)(() =>
                {
                    connectionStatus.Checked = true;
                    connectionStatus.Text = "Connected";
                }));
            }
            else
            {
                connectionStatus.Checked = true;
                connectionStatus.Text = "Connected";
            }
        }

		public Site()
		{
			InitializeComponent();
		}

		private void btnclientSendData_Click(object sender, EventArgs e)
		{
            if (site != null && site.conn != null)
            {
                site.conn.Send(Encoding.UTF8.GetBytes(txtClientSendData.Text), false);
            }
		}

		private void btnServerSendData_Click(object sender, EventArgs e)
		{
            if (site != null && toc.db.connListner != null)
            {
                toc.db.connListner.SendToAll(Encoding.UTF8.GetBytes(txtServerSendData.Text));
            }
		}

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (site != null)
            {
                SC.Connection conn = site.createConnection();
                conn.connect();
            }
        }

		private void btnImport_Click(object sender, EventArgs e)
		{
			BackgroundWorker worker = new BackgroundWorker();
			worker.ProgressChanged += worker_ProgressChanged;
			worker.DoWork += worker_DoWork;
			worker.WorkerReportsProgress = true;
			worker.RunWorkerCompleted += worker_RunWorkerCompleted;
			worker.RunWorkerAsync(worker);
		}
		void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			barProgress.Value = 100;
			MessageBox.Show("Complete");
		}
		class POAddress
		{
			public string pincode;
			public string site;
			public string sdca;
			public string ssa;
			public string circle;
			public string lat;
			public string lng;
		}
		List<POAddress> readPO(string file)
		{
			try
			{
				List<POAddress> list = new List<POAddress>();
				Dictionary<string, List<POAddress>> dic = new Dictionary<string, List<POAddress>>();
				using (TextFieldParser csvParser = new TextFieldParser(file))
				{
					csvParser.CommentTokens = new string[] { "#" };
					csvParser.SetDelimiters(new string[] { "," });
					csvParser.HasFieldsEnclosedInQuotes = true;

					// Skip the row with the column names
					csvParser.ReadLine();

					while (!csvParser.EndOfData)
					{
						// Read current line fields, pointer moves to the next line.
						string[] fields = csvParser.ReadFields();
						POAddress add = new POAddress();
						add.site = fields[0];
						add.sdca = fields[1];
						add.ssa = fields[2];
						add.circle = fields[3];
						add.pincode = fields[4];
						add.lat = fields[5];
						add.lng = fields[6];
						list.Add(add);
					}
					csvParser.Close();
				}
				return list;
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace);
			}
			return null;
		}

		void worker_DoWork(object sender, DoWorkEventArgs e)
		{
			List<POAddress> list = readPO("polist.csv");
			int cnt = 0;
			int lastProgress = 0;
			foreach (POAddress add in list)
			{
				DB.Site site = new DB.Site();
				site.name = add.site;
				site.addr = new DB.Address();
				site.addr.addrLine1 = add.site;
				site.addr.addrLine2 = add.sdca;
				site.addr.city = add.ssa;
				site.addr.state = add.circle;
				site.addr.pincode = add.pincode;
				site.addr.lattitude = Double.Parse(add.lat);
				site.addr.longitude = Double.Parse(add.lng);
				site.sdca = new DB.SDCA();
				site.sdca.name = add.sdca;
				site.sdca.addr = add.sdca;
				site.sdca.ssa = new DB.SSA();
				site.sdca.ssa.name = add.ssa;
				site.sdca.ssa.addr = add.ssa;
				site.sdca.ssa.circle = new DB.Circle();
				site.sdca.ssa.circle.name = add.circle;
				site.sdca.ssa.circle.addr = add.circle;
				toc.db.addSite(site);

				++cnt;
				int progress = cnt * 100 / list.Count;
				if (lastProgress != progress)
				{
					((BackgroundWorker)e.Argument).ReportProgress(progress);
					lastProgress = progress;
				}
			}
		}

		void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			barProgress.Value = e.ProgressPercentage;
		}

	}
}
