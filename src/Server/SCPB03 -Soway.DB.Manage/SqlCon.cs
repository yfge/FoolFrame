using System;
using System.Collections.Generic;
 
using System.Text;

namespace Soway.DB.Manage
{
    public class SqlCon
    {
        public SqlCon(String ConStr)
        {
            this.ConnectionString = ConStr;
        }

        private String ConnectionString
        {
            get;
            set;
        }

        public SqlCon() : this(Global.ConnectionString ) { }
        public System.Data.DataTable GetTable(String sql)
        {
            using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(this.ConnectionString))
            {
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql);
                cmd.Connection = conn;
                cmd.CommandTimeout = cmd.CommandTimeout * 100;
                cmd.Connection.Open();
                System.Data.SqlClient.SqlDataAdapter adaper = new System.Data.SqlClient.SqlDataAdapter(cmd);
                System.Data.DataTable table = new System.Data.DataTable();
                adaper.Fill(table);
                conn.Close();
                return table;

            }
        }

        public bool ExuteSqls(string[] sqls)
        {
            using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(this.ConnectionString))
            {

                conn.Open();
                var trans = conn.BeginTransaction();
                System.Data.SqlClient.SqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = trans;
                try
                {
                    foreach (var sql in sqls)
                    {

                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                    }
                    trans.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    return false;
                }

            }
        }
        public int ExcuteSql(String sql)
        {
            using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(this.ConnectionString))
            {
                conn.Open();
                var command = conn.CreateCommand();
                command.CommandText = sql;
                int i = command.ExecuteNonQuery();
                conn.Close();
                return i;
            }
        }
    }

}
