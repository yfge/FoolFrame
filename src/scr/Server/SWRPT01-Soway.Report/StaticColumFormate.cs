using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Report
{
    public class StaticCellFormate
    {
        /// <summary>
        /// 要统计的列
        /// </summary>
        public ValueCell StaticCell
        {
            get;
            set;
        }

        /// <summary>
        /// 统计的类型
        /// </summary>
        public StaticType StaticType
        {
            get;
            set;
        }
    }
}
