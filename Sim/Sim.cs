using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sim
{
	public partial class Sim : Form
	{
		private DB.Site site;
		public Sim()
		{
			InitializeComponent();
			addDefaultSensors();
            chkRandomFill.Checked = true;
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                string file = args[1];
                connectToSite(file);
            }
		}
        ~Sim()
        {
            site = null;
        }
        private void connectToSite(string siteInfoFile)
        {
            site = new DB.Site();
            site.readSite(siteInfoFile);
            SC.Connection conn = site.createConnection();
            //conn.connect();
            conn.Conncted += conn_Conncted;
            conn.Disconnected += conn_Disconnected;
            conn.Data += conn_Data;
            SC.Server listener = site.createListener();
            listener.Data += listener_Data;
            listener.listen();
        }
		private void connect_Click(object sender, EventArgs e)
		{
            connectToSite("site.xml");
		}

		void listener_Data(SC.StateObject s, byte[] data)
		{
            if(this.InvokeRequired)
            {
                BeginInvoke((Action<byte[]>)((byte[] d) =>
                {
                    serverReceivedData.Text += Encoding.UTF8.GetString(d) + "\r\n";
                }), data);
            }
            else
                serverReceivedData.Text += Encoding.UTF8.GetString(data) + "\r\n";

        }

		void conn_Data(byte[] data)
		{
            if(this.InvokeRequired)
            {
                BeginInvoke((Action<byte[]>)((byte[] d) =>
                {
                    clientReceiveData.Text += Encoding.UTF8.GetString(d) + "\r\n";
                }), data);
            }
            else
                clientReceiveData.Text += Encoding.UTF8.GetString(data) + "\r\n";
		}

		void conn_Disconnected(object sender, EventArgs e)
		{
            if (this.InvokeRequired)
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
            if (this.InvokeRequired)
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

		private void sendClientData_Click(object sender, EventArgs e)
		{
			if (site != null)
			{
                site.conn.Send(Encoding.UTF8.GetBytes(clientSendData.Text), true);
			}
		}

		private void Sim_Load(object sender, EventArgs e)
		{

		}

		private void sendServerData_Click(object sender, EventArgs e)
		{
			if (site != null && site.listener != null)
                site.listener.SendToAll(Encoding.UTF8.GetBytes(serverSendData.Text));
		}

        private void Sim_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(site != null && site.listener != null)
            {
                site.listener.shutDown();
            }
        }

		private void chkRandomFill_CheckedChanged(object sender, EventArgs e)
		{

		}
		private void randomizeValues()
		{
			Random rnd = new Random();
			foreach (ListViewItem item in lstSensorData.Items)
			{
				item.SubItems[2].Text = (25 + 50 * rnd.NextDouble()).ToString("0.00");
			}
		}
		private void addDefaultSensors()
		{
			lstSensorData.Items.Add(new ListViewItem(new String[] { "Temperature", "1", "0" }));
			lstSensorData.Items.Add(new ListViewItem(new String[] { "DG1 Oil Level", "2", "0" }));
			lstSensorData.Items.Add(new ListViewItem(new String[] { "DG2 Oil Level", "3", "0" }));
			lstSensorData.Items.Add(new ListViewItem(new String[] { "DG1 Power", "4", "0" }));
			lstSensorData.Items.Add(new ListViewItem(new String[] { "DG2 Power", "5", "0" }));
			lstSensorData.Items.Add(new ListViewItem(new String[] { "DG1 LO Pressure", "6", "0" }));
			lstSensorData.Items.Add(new ListViewItem(new String[] { "DG2 LO Pressure", "7", "0" }));
			lstSensorData.Items.Add(new ListViewItem(new String[] { "Battery1 Voltage", "8", "0" }));
			lstSensorData.Items.Add(new ListViewItem(new String[] { "Battery2 Voltage", "9", "0" }));
		}
		private List<byte> buildAllSensorData()
		{
			List<byte> bytes = new List<byte>();

			bytes.Add(0);	//format header
			for (int i = 0; i < lstSensorData.Items.Count; ++i)
			{
				bytes.Add(Byte.Parse(lstSensorData.Items[i].SubItems[1].Text));
				float val = Single.Parse(lstSensorData.Items[i].SubItems[2].Text);
				byte[] valBytes = BitConverter.GetBytes(val);
				float v = BitConverter.ToSingle(valBytes, 0);
				bytes.AddRange(valBytes);
			}
			return bytes;
		}
		private void tmrUpdateData_Tick(object sender, EventArgs e)
		{
			if (chkRandomFill.Checked)
				randomizeValues();
            if (site != null && site.conn != null)
            {
                List<byte> bytes = buildAllSensorData();
                bool invokeRequired = this.InvokeRequired;
                site.conn.connect(false);
                site.conn.Send(bytes.ToArray(), false);
                site.conn.disconnect();
            }
        }

		private void btnSendSensorData_Click(object sender, EventArgs e)
		{
			if (site != null && site.conn != null)
			{
				List<byte> bytes = buildAllSensorData();
                site.conn.connect(false);
				site.conn.Send(bytes.ToArray(), false);
                site.conn.disconnect();
			}
		}
	}
}
