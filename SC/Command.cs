using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SC
{
	public enum DataFormat
	{
		KEY_VALUE,
	};
	public enum ECommand
	{
		//connection
		UPDATE_KEY,
		UPDATE_TOC_IP_PORT,
		//sensor data
		GET_ALL_DATA,
		//set data
		SET_DATA,
	};
}
