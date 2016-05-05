using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DB
{
	public class Address
	{
		public string houseno { get; set; }
		public string addrLine1 { get; set; }
		public string addrLine2 { get; set; }
		public string lndmark { get; set; }
		public string city { get; set; }
		public string state { get; set; }
		public string pincode { get; set; }
		public double lattitude { get; set; }
		public double longitude { get; set; }

		public void read(XmlNode node)
		{
			houseno = node.SelectSingleNode("HouseNo").Attributes["Value"].Value;
			addrLine1 = node.SelectSingleNode("Line1").Attributes["Value"].Value;
			addrLine2 = node.SelectSingleNode("Line2").Attributes["Value"].Value;
			lndmark = node.SelectSingleNode("Landmark").Attributes["Value"].Value;
			pincode = node.SelectSingleNode("PIN").Attributes["Value"].Value;
			lattitude = double.Parse(node.SelectSingleNode("Latitude").Attributes["Value"].Value);
			longitude = double.Parse(node.SelectSingleNode("Longitude").Attributes["Value"].Value);
		}
	}
}
