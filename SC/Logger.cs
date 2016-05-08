using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Windows.Forms;
using log4net.Config;
namespace SC
{
	public class Logger
	{
		private static readonly ILog logger = LogManager.GetLogger(typeof(Logger));
		public Logger()
		{
			XmlConfigurator.Configure(new System.IO.FileInfo("logging.config"));
		}
		public static void info(Object o, Exception e = null)
		{
			if (e != null)
				logger.Info(o, e);
			else
				logger.Info(o);
		}
		public static void debug(Object o, Exception e = null)
		{
			if (e != null)
				logger.Debug(o, e);
			else
				logger.Debug(o);
		}
		public static void error(Object o, Exception e = null)
		{
			if (e != null)
				logger.Error(o, e);
			else
				logger.Error(o);
		}
		public static void fatal(Object o, Exception e = null)
		{
			if (e != null)
				logger.Fatal(o, e);
			else
				logger.Fatal(o);
		}
		public static void warn(Object o, Exception e = null)
		{
			if (e != null)
				logger.Warn(o, e);
			else
				logger.Warn(o);
		}
		public static void exception(Exception e)
		{
			logger.Error("Exception", e);
		}
	}
}
