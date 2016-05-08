using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using System.Net;
using System.Xml.Linq;
namespace TOC
{
	public partial class MapView : DockContent
	{
		public MapView()
		{
			InitializeComponent();
		}
        private List<POAddress> readPo(string file)
        {
            List<POAddress> list = new List<POAddress>();
            TextFieldParser csvParser = new TextFieldParser(file);
            csvParser.CommentTokens = new string[] { "#" };
            csvParser.SetDelimiters(new string[] { "," });
            csvParser.HasFieldsEnclosedInQuotes = true;
            csvParser.ReadLine();
            int cnt = 0;
            while (!csvParser.EndOfData)
            {
                string[] values = csvParser.ReadFields();
                if (values.Length == 15)
                {
                    POAddress add = new POAddress();
                    add.pincode = values[1];
                    add.site = values[0].Replace("B.O", "").Replace("S.O","");
                    add.sdca = values[8];
                    add.ssa = values[5];
                    add.circle = values[9];
                    list.Add(add);
                }

                ++cnt;
            }
            csvParser.Close();
            return list;
        }
        private void writePo(string file, List<POAddress> listPO)
        {
            StreamWriter writer = new StreamWriter(File.OpenWrite(file));
            writer.WriteLine("Pincode,Circle,SSA,SDCA,Site,Lat,Lon");
            foreach (POAddress add in listPO)
            {
                writer.WriteLine(add.pincode + "," + add.circle + "," + add.ssa + "," + add.sdca + "," + add.site + "," + add.lat + "," + add.lon);
            }
            writer.Close();
        }
		private void MapView_Load(object sender, EventArgs e)
		{
/*
            List<POAddress> listPO = readPo("polist.csv");
            writePo("POlistlatlon.csv", listPO);
			gmap.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
			GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
			gmap.SetPositionByKeywords("Delhi, India");
			gmap.MinZoom = 0;
			gmap.MaxZoom = 24;
			gmap.Zoom = 12;
			gmap.ShowCenter = false;
			gmap.MapScaleInfoEnabled = true;
			gmap.DragButton = MouseButtons.Left;
			GMap.NET.PointLatLng  pos = gmap.Position;
*/
		}
		public void setPosition(GMap.NET.PointLatLng pt)
		{
			gmap.Position = pt;
		}
		public void scaleToFit(List<GMap.NET.PointLatLng> lstPts)
		{
			GMap.NET.PointLatLng center = new GMap.NET.PointLatLng();
			GMap.NET.PointLatLng min = new GMap.NET.PointLatLng(20000000, 20000000);
			GMap.NET.PointLatLng max = new GMap.NET.PointLatLng(-20000000, -20000000);
			foreach (GMap.NET.PointLatLng pt in lstPts)
			{
				if (min.Lat > pt.Lat) min.Lat = pt.Lat;
				if (min.Lng > pt.Lng) min.Lng = pt.Lng;
				if (max.Lat < pt.Lat) max.Lat = pt.Lat;
				if (max.Lng < pt.Lng) max.Lng = pt.Lng;
			}
			center.Lat = (min.Lat + max.Lat) / 2;
			center.Lng = (min.Lng + max.Lng) / 2;
			setPosition(center);

			//calc zoom

		}

        private void btnGenLL_Click(object sender, EventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
            worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
            worker.WorkerReportsProgress = true;
            worker.RunWorkerAsync(worker);
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            barProgress.Value = e.ProgressPercentage;
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Complete");
        }
        List<POAddress> listPO;
        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            listPO = readPo("polist.csv");
/*
            gmap.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
            gmap.MinZoom = 0;
            gmap.MaxZoom = 24;
            gmap.Zoom = 0;
            gmap.ShowCenter = false;
            gmap.MapScaleInfoEnabled = true;
            gmap.DragButton = MouseButtons.Left;*/
            MessageBox.Show("Count " + listPO.Count);
            int cnt = 0;
            int lastProgress = 0;
            foreach (POAddress add in listPO)
            {
                //string address = add.pincode + "," + add.ssa + ","+ add.circle + ",India";
                string address = add.pincode + "&components=country:IN|postal_code:" + add.pincode;
//                gmap.SetPositionByKeywords(add.pincode + ",India");
//                add.lat = "" + gmap.Position.Lat;
//                add.lon = "" + gmap.Position.Lng;
                try
                {
                    string requestUri = string.Format("http://maps.googleapis.com/maps/api/geocode/xml?address={0}&sensor=false", 
                        Uri.EscapeDataString(address));
                    WebRequest req = WebRequest.Create(requestUri);
                    WebResponse resp = req.GetResponse();
                    XDocument xdoc = XDocument.Load(resp.GetResponseStream());
                    XElement result = xdoc.Element("GeocodeResponse").Element("result");
                    XElement loc = result.Element("geometry").Element("location");
                    add.lat = loc.Element("lat").Value;
                    add.lon = loc.Element("lng").Value;
                }
                catch(Exception e1)
                {
					SC.Logger.exception(e1);
                }
                int progress = (int)(cnt * 100.0 / listPO.Count);
                ++cnt;
                if(lastProgress != progress)
                {
                    ((BackgroundWorker)e.Argument).ReportProgress(progress);
                    lastProgress = progress;
                }
            }
            ((BackgroundWorker)e.Argument).ReportProgress(100);
            writePo("polistll.csv", listPO);
        }

        private void btnWrite_Click(object sender, EventArgs e)
        {
            if(listPO != null)
                writePo("polistll.csv", listPO);
        }
	}
    public class POAddress
    {
        public string pincode;
        public string site;
        public string sdca;
        public string ssa;
        public string circle;
        public string lat;
        public string lon;
    }
}
