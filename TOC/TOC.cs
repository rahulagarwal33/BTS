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
	public partial class TOC : Form
	{
		Hierarchy hierarchy;
		PropertyWindow propWindow = new PropertyWindow();
        LogView logView = new LogView();
		MapView mapView = new MapView();
		public Site siteWindow;
		public DB.SDB db;

		public TOC()
		{
			InitializeComponent();
			hierarchy = new Hierarchy(this);
            siteWindow = new Site(this);
			db = new DB.SDB();
            db.read("Data/toc.xml");
			db.connect();
            SC.Server listener = db.createListener();
            listener.Data += listener_Data;
            listener.listen();

			hierarchy.HideOnClose = true;
			propWindow.HideOnClose = true;
            logView.HideOnClose = true;
			mapView.HideOnClose = true;
			hierarchy.initialize();
			hierarchy.Show(pnlMain, DockState.DockLeft);
			propWindow.Show(pnlMain, DockState.DockRight);
			siteWindow.HideOnClose = true;
			siteWindow.Show(pnlMain, DockState.Document);
            logView.Show(pnlMain, DockState.DockBottom);
			mapView.Show(pnlMain, DockState.Document);
		}

        void listener_Data(SC.StateObject s, byte[] data)
        {
            
        }

		private void propertyViewToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (propWindow.IsHidden)
				propWindow.Show();
			else
				propWindow.Hide();
		}
		private void propertyViewToolStripMenuItem_Paint(object sender, PaintEventArgs e)
		{
			propertyViewToolStripMenuItem.Checked = !propWindow.IsHidden;
		}

		private void hierarchyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (hierarchy.IsHidden)
				hierarchy.Show();
			else
				hierarchy.Hide();
		}

		private void hierarchyToolStripMenuItem_Paint(object sender, PaintEventArgs e)
		{
			hierarchyToolStripMenuItem.Checked = !hierarchy.IsHidden;
		}

        private void logViewToolStripMenuItem_Paint(object sender, PaintEventArgs e)
        {
            logViewToolStripMenuItem.Checked = !logView.IsHidden;
        }

        private void logViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (logView.IsHidden)
                logView.Show();
            else
                logView.Hide();
        }

        private void TOC_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(db != null && db.connListner != null)
                db.connListner.shutDown();
        }
	}
}
