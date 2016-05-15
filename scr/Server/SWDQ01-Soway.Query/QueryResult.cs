using System;
using System.Collections.Generic;
using System.Text;

namespace Soway.Query
{
    /// <summary>
    /// 查询结果，用于表示一个自定义查询的分页结果
    /// </summary>
   public  class QueryResult
    {
        /// <summary>
        /// 总页数
        /// </summary>
        public long  TotalPages { get; internal set; }
        /// <summary>
        /// 当前页
        /// </summary>
        public long CurrentPage { get; set; }
        /// <summary>
        /// 总记录数
        /// </summary>
        public long TotalRecords { get; set; }
        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize { get; private set; }
 


        private System.Data.SqlClient.SqlCommand Command { get; set; }

        private string ConStr { get; set; }
        internal QueryResult(string conStr,System.Data.SqlClient.SqlCommand command)
        {
            this.ConStr = conStr;
            this.Command = command;
            this.PageSize = (int)
                command.Parameters["@pagesize"].Value;
            this.CurrentPage = 1;
            GetData();
            
        }


       /// <summary>
       /// 得到当前的数据
       /// </summary>
       /// <returns></returns>
        public System.Data.DataTable GetData()
        
        {
            using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConStr))
            {
                var command = Command.Clone();
                command.Connection = con;

                command.Connection.Open();

                command.Parameters["@pagesize"].Value = this.PageSize;
                command.Parameters["@page"].Value=this.CurrentPage;

                System.Data.DataSet set = new System.Data.DataSet();
                new System.Data.SqlClient.SqlDataAdapter(command).Fill(set);
                this.TotalRecords = (int)set.Tables[0].Rows[0][0];
                this.TotalPages = (this.TotalRecords / PageSize) + (this.TotalRecords % PageSize == 0 ? 0 : 1);
                return set.Tables[1];
            }
        }
    }
}
