using System;
using System.Collections.Generic;
using System.Text;

namespace Soway.Query.Entity
{
    /// <summary>
    /// 表示数据库中的表
    /// </summary>
    public class IQueryTable :IQueryAtom
    {
        public string ShowName
        {
            get;
            set;
        }

        public string DBName
        {
            get;
            set;
        }
    }
}
