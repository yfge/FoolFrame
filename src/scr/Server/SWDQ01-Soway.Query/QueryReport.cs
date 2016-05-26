using System;
using System.Collections.Generic;
using System.Text;
using Soway.Query.Entity;

namespace Soway.Query
{
    public interface QueryReport
    {
        /// <summary>
        /// 查询的Sql
        /// </summary>
        string SqlStript
        {
            get;
        }

        /// <summary>
        /// 输出的列
        /// </summary>
        List<IQueryColumn> Columns
        {
            get;
         
        }

        /// <summary>
        /// 查询参数
        /// </summary>
          List<QueryParameter> Parameters
        {
            get;
        }

        string ReportName
        {
            get;
            set;
        }

        string ReportNo
        {
            get;
            set;
        }
    }
}
