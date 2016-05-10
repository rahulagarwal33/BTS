using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SC
{
	public class DataBuilder
	{
		public delegate void SensorDataHandler(int sensorid, float value);
		public event SensorDataHandler SensorData;

		protected List<byte> data = new List<byte>();
		public DataFormat Format { get; set; }
		public static DataBuilder Builder(DataFormat format)
		{
			switch (format)
			{
				case DataFormat.KEY_VALUE: return new DataBuilderKVPair();
				default: return null;
			}
		}
		public static DataBuilder Parser(byte[] d)
		{
			DataFormat format = (DataFormat)d[0];
			DataBuilder parser = null;
			switch (format)
			{
				case DataFormat.KEY_VALUE:
					{
						parser = new DataBuilderKVPair();
						break;
					}
			}
			if (parser != null)
			{
				parser.data.AddRange(d);
			}
			return parser;
		}
		public virtual void parse()
		{

		}
		public virtual void addSensorData(int idx, float value)
		{

		}
		public virtual void reset()
		{
			data.Clear();
		}
		public List<byte> getData()
		{
			return data;
		}
	}
	class DataBuilderKVPair :  DataBuilder
	{
		public DataBuilderKVPair()
		{
			reset();
		}
		public override void reset()
		{
			base.reset();
			data.Add((byte)Format);
		}
		public override void addSensorData(int idx, float value)
		{
			data.Add((byte)idx);
			byte[] valBytes = BitConverter.GetBytes(value);
			data.AddRange(valBytes);
		}
		public override void parse()
		{
			byte[] arr = data.ToArray();
			int dataStart = 1;
			int dataEnd = data.Count;
			while (dataStart + 5 <= dataEnd)
			{
				byte sensorid = arr[dataStart];
				float value = BitConverter.ToSingle(arr, dataStart + 1);
				dataStart += 5;
				SensorData(sensorid, value);
			}

		}
	}
}
