using System;
using System.Collections.Generic;
using System.Text;

namespace Soway.Query.Entity
{
    /// <summary>
    /// 当数据列为表示状态信息时，ColStateValue表示状态值和用户值之间的关系
    /// </summary>
    public class ColStateValue :IQueryAtom
    {

        /// <summary>
        /// 显示的状态值
        /// </summary>
        public string ShowName
        {
            get;
            set;
        }

        /// <summary>
        /// 实际值
        /// </summary>
        public string DBName
        {
            get;
            set;
        }
    }
}
