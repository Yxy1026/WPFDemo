using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Npgsql;
using NpgsqlTypes;

namespace FaceSystem.Tool
{
    class DataBaseHelper
    {
        public static NpgsqlConnection getSqlConnection()
        {
            NpgsqlConnection Connection = new NpgsqlConnection();
            try
            {
                //获取数据库字符串
                Connection.ConnectionString = "Host=127.0.0.1;Username=postgres;Password=1234;Database=postgres";
                Connection.Open();
                Connection.Close();

            }
            catch
            {

                throw new Exception("无法连接数据库服务器");
            }

            return Connection;
        }

        public static int GetNonQueryEffect(string sqlstr)
        {
            NpgsqlConnection Connection = new NpgsqlConnection();
            try
            {
                Connection.Open();
                
                var cmd = new NpgsqlCommand();
                cmd.Connection = Connection;
                cmd.CommandText = sqlstr;

                return cmd.ExecuteNonQuery();//返回执行语句中的错误
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }
            finally
            {
                Connection.Close();
                Connection.Dispose();//释放资源
            }

        }

        public static DataTable GetDataset(string sqlstr)
        {
            NpgsqlConnection conn = getSqlConnection();
            try
            {
                conn.Open();//打开数据库连接
                DataTable dt = new DataTable();
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(sqlstr, conn);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.ToString());
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public static List<ScoreDetail> FillModel(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }
            List<ScoreDetail> modelList = new List<ScoreDetail>();
            foreach (DataRow dr in dt.Rows)
            {
                ScoreDetail model = new ScoreDetail();
                for (int i = 0; i < dr.Table.Columns.Count; i++)
                {
                    PropertyInfo propertyInfo = model.GetType().GetProperty(dr.Table.Columns[i].ColumnName);
                    if (propertyInfo != null && dr[i] != DBNull.Value)
                        propertyInfo.SetValue(model, dr[i], null);
                }

                modelList.Add(model);
            }
            Console.WriteLine("学号 "+ modelList[0].Xh);
            return modelList;
        }

        public static List<ScoreDetail> DataTableToList<ScoreDetail>(DataTable dt) where ScoreDetail : class, new()
        {
            // 定义集合 
            List<ScoreDetail> scoreDetails = new List<ScoreDetail>();
            //定义一个临时变量 
            string tempName = string.Empty;
            //遍历DataTable中所有的数据行 
            foreach (DataRow dr in dt.Rows)
            {
                ScoreDetail scoreDetail = new ScoreDetail();
                // 获得此模型的公共属性 
                PropertyInfo[] propertys = scoreDetail.GetType().GetProperties();
                //遍历该对象的所有属性 
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;//将属性名称赋值给临时变量 
                                       //检查DataTable是否包含此列（列名==对象的属性名）  
                    if (dt.Columns.Contains(tempName))
                    {
                        //取值 
                        object value = dr[tempName];
                        //如果非空，则赋给对象的属性 
                        if (value != DBNull.Value)
                        {
                            pi.SetValue(scoreDetail, value, null);
                        }
                    }
                }
                //对象添加到泛型集合中 
                scoreDetails.Add(scoreDetail);
            }
            return scoreDetails;
        }

    }
}
   