using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DB
{
	public class DataParser
	{
		public delegate void SensorDataHandler(int sensorid, float value);
		public event SensorDataHandler SensorData;
		void parseFormat0(byte[] bytes, int dataStart, int dataEnd)
		{
			while(dataStart + 5 <= dataEnd)
			{
				byte sensorid = bytes[dataStart];
				float value = BitConverter.ToSingle(bytes, dataStart + 1);
				dataStart += 5;
				SensorData(sensorid, value);
			}
		}
		void parseFormat1(byte[] bytes, int dataStart, int dataEnd)
		{
			SensorData(1, 10);
			SensorData(1, 11.3f);
			SensorData(1, 13.5f);
		}
		public void parse(byte[] data)
		{
            if (data.Length > 0)
			{
                byte format = data[0];
				switch(format)
				{
					case 0:
						{
                            parseFormat0(data, 1, data.Length);
							break;
						}
					case 1:
						{
                            parseFormat1(data, 1, data.Length);
							break;
						}
				}
			}
		}
		
	}
}
