using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace DB
{
    public class BatchQuery
    {
        public string connString { get; set; }
        public string statement { get; set; }
        public int maxBatchSize { get; set; }
        public bool queryType = true;   //insert
        object sync = new object();
        List<List<MySqlParameter>> listValues = new List<List<MySqlParameter>>();

        public BatchQuery()
        {
            maxBatchSize = 1000;
        }
        public void addValues(List<MySqlParameter> listParam)
        {
            lock (sync)
            {
                if (listValues.Count != 0 && listParam.Count != listValues[listValues.Count- 1].Count)
                {
                    int k = 1;
                }
                listValues.Add(listParam);
            }
        }
		void executeSqlQuery(string command, List<MySqlParameter> listParam)
		{
			try
			{
				if (command != "")
				{
					int ret = MySqlHelper.ExecuteNonQuery(connString, command,
						listParam.ToArray());
				}
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message + "\r\n" + e.StackTrace);
			}
		}
        public void executeUpdate()
        {
			if (listValues.Count < maxBatchSize)
				return;
            List<List<MySqlParameter>> listLocalValues = new List<List<MySqlParameter>>();
            lock (sync)
            {
                listLocalValues.AddRange(listValues.ToArray());
                listValues.Clear();
            }
            
            int cnt = 0;
            string result = "";
            List<MySqlParameter> finalList = new List<MySqlParameter>();
            foreach (List<MySqlParameter> listParam in listLocalValues)
            {
                List<string> pList = new List<string>();    
                foreach(MySqlParameter p in listParam)
                {
                    string paramName = "@Param" + cnt;
                    pList.Add(paramName);
                    p.ParameterName = paramName;
                    finalList.Add(p);
                    ++cnt;
                }
                string current = String.Format(statement, pList.ToArray());
                result += current + "\r\n";
				if (cnt > maxBatchSize)
                {
					executeSqlQuery(result, finalList);
                    cnt = 0;
                    result = "";
                    finalList.Clear();
                }
            }
			executeSqlQuery(result, finalList);
        }
        public void executeInsert()
        {
			if (listValues.Count < maxBatchSize)
				return;
            List<List<MySqlParameter>> listLocalValues = new List<List<MySqlParameter>>();
            lock (sync)
            {
                listLocalValues.AddRange(listValues.ToArray());
                listValues.Clear();
            }
            StringBuilder builder = new StringBuilder(statement + " VALUES");
            List<MySqlParameter> finalList = new List<MySqlParameter>();
            int cnt = 0;
            foreach (List<MySqlParameter> listParam in listLocalValues)
            {
                if (cnt != 0)
                {
                    builder.AppendFormat("), ");
                }
                builder.AppendFormat("(");
                int j = 0;
                foreach(MySqlParameter param in listParam)
                {
                    string paramName = "@Param" + cnt;
                    param.ParameterName = paramName;
                    finalList.Add(param);
                    if (j != listParam.Count - 1)
                        builder.Append(paramName + ", ");
                    else
                        builder.Append(paramName);
                    ++j;
                    ++cnt;
                }
                if (cnt > 500)
                {
                    builder.AppendFormat(");");
                    string str = builder.ToString();
					executeSqlQuery(str, finalList);
                    finalList.Clear();
                    builder = new StringBuilder(statement + " VALUES");
                    cnt = 0;
                }
            }
			if (finalList.Count != 0)
			{
				builder.AppendFormat(");");
				string str1 = builder.ToString();
				executeSqlQuery(str1, finalList);
			}
			listLocalValues.Clear();
        }
        public int count()
        {
            return listValues.Count;
        }
    }
}
