using System;
using System.Collections.Generic;
using System.Text;

namespace Soway.Query
{
    /// <summary>
    /// 表示两个表连接的条件
    /// 
    /// </summary>
    public class JoinCondition
    {

        /// <summary>
        /// 表1的数据列名（在数据库中的名字）
        /// </summary>
        public String  LeftCol
        {
            get;
            set;
        }


        /// <summary>
        /// 表2的数据列名（在数据库中的名字）
        /// </summary>
        public String  RightCol
        {
            get;
            set;
        }

        public JoinCondition(String   Left, String  Right)
        {
            this.LeftCol = Left;
            this.RightCol = Right;
        }

    }
}
