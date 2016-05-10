using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Config;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Reflection;
namespace SC
{
	public class Logger
	{
		private static ILog log = null;
		static Logger()
		{
			log = LogManager.GetLogger(typeof(Logger));
			XmlConfigurator.Configure(new System.IO.FileInfo("log.config"));
		}
		public static void exception(Exception e)
		{
			error(e, e);
		}
		public static void info(object o, Exception e = null, string membername="")
		{
			StackTrace trace = new StackTrace(1);
			StackFrame frame = trace.GetFrame(0);
			MethodBase method = frame.GetMethod();
			string caller = method.ReflectedType.FullName + "." + method.Name + ":" + frame.GetFileLineNumber();
			if (e != null)
				log.Info(caller + ":" + o, e);
			else
				log.Info(caller + ":" + o);
		}
		public static void warn(object o, Exception e = null)
		{
			StackTrace trace = new StackTrace(1);
			StackFrame frame = trace.GetFrame(0);
			MethodBase method = frame.GetMethod();
			string caller = method.ReflectedType.FullName + "." + method.Name + ":" + frame.GetFileLineNumber();
			if (e != null)
				log.Warn(caller + ":" + o, e);
			else
				log.Warn(caller + ":" + o);
		}
		public static void debug(object o, Exception e = null)
		{
			StackTrace trace = new StackTrace(1);
			StackFrame frame = trace.GetFrame(0);
			MethodBase method = frame.GetMethod();
			string caller = method.ReflectedType.FullName + "." + method.Name + ":" + frame.GetFileLineNumber();
			if (e != null)
				log.Debug(caller + ":" + o, e);
			else
				log.Debug(caller + ":" + o);
		}
		public static void error(object o, Exception e = null)
		{
			StackTrace trace = new StackTrace(1);
			StackFrame frame = trace.GetFrame(0);
			MethodBase method = frame.GetMethod();
			string caller = method.ReflectedType.FullName + "." + method.Name + ":" + frame.GetFileLineNumber();
			if (e != null)
				log.Error(caller + ":" + o, e);
			else
				log.Error(caller + ":" + o);
		}
		public static void fatal(object o, Exception e = null)
		{
			StackTrace trace = new StackTrace(1);
			StackFrame frame = trace.GetFrame(0);
			MethodBase method = frame.GetMethod();
			string caller = method.ReflectedType.FullName + "." + method.Name + ":" + frame.GetFileLineNumber();
			if (e != null)
				log.Fatal(caller + ":" + o, e);
			else
				log.Fatal(caller + ":" + o);
		}
	}
}
