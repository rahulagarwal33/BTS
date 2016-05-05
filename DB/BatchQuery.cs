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

        object sync = new object();
        List<List<MySqlParameter>> listValues = new List<List<MySqlParameter>>();

        public BatchQuery()
        {
            maxBatchSize = 100;
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
                if (listValues.Count > maxBatchSize)
                    execute1();
            }
        }
        public void execute1()
        {
            lock (sync)
            {
                int cnt = 0;
                string result = "";
                List<MySqlParameter> finalList = new List<MySqlParameter>();
                foreach (List<MySqlParameter> listParam in listValues)
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
                }
                try
                {
                    int ret = MySqlHelper.ExecuteNonQuery(connString, result,
                        finalList.ToArray());
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message + "\r\n" + e.StackTrace);
                }
                listValues.Clear();
            }
        }
        public void execute()
        {
            lock (sync)
            {
                StringBuilder builder = new StringBuilder(statement + " VALUES");
                List<MySqlParameter> finalList = new List<MySqlParameter>();
                int cnt = 0;
                int i = 0;
                foreach (List<MySqlParameter> listParam in listValues)
                {
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
                    if (i != listValues.Count - 1)
                        builder.AppendFormat("), ");
                    else
                        builder.AppendFormat(");");
                    ++i;
                }
                string str = builder.ToString();
                try
                {
                    int ret = MySqlHelper.ExecuteNonQuery(connString, str,
                        finalList.ToArray());
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message + "\r\n" + e.StackTrace);
                }
                listValues.Clear();
            }
        }
        public int count()
        {
            lock (sync)
            {
                return listValues.Count;
            }
        }
    }
}
