using System;
using System.Collections.Generic;
using System.Text;

namespace Soway.Query.BoolExp
{
    /// <summary>
    /// 中间结果类，表示一个表达式返回的Sql语句部分
    /// </summary>
    public class BoolExpressionSqlPart
    {
        /// <summary>
        /// 返回的Sql语句，类似于[Column]=@p1 and [Column2]=@p2
        /// </summary>
        public String Stript { get; internal set; }
        /// <summary>
        /// 语句中包含的参数的列表
        /// </summary>
        public List<QueryParameter> Parameters { get; internal set; }
        internal BoolExpressionSqlPart() { this.Parameters = new List<QueryParameter>(); }
    }
}
