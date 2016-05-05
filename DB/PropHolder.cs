using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DB
{
	public class Property
	{
		public object value { get; set; }
		public string name { get; set; }

		public Property(string n, object o)
		{
			name = n;
			value = o;
		}
	}
	public class PropertyBag
	{
		public Dictionary<string, List<Property>> mapProps;
		public void addProperty(string category, Property prop)
		{
			mapProps[category].Add(prop);
		}
	}
	public interface PropHolder
	{
		void enumProperties(PropertyBag bag);
		bool getProperty(Property prop);
		bool setProperty(Property prop);
	}
}
