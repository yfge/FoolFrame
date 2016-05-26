using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Report
{
    public class StaticFormat
    {
        public StaticFormat()
        {
            this.StaticsCells = new List<StaticCellFormate>();
        }
        /// <summary>
        /// 要统计的值
        /// </summary>
        public System.Collections.Generic.List<Soway.Report.StaticCellFormate> StaticsCells
        {
            get;
            set;
        }


        /// <summary>
        /// 输出的名称(小计,合计,平均等)
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        public String Fileter { get; set; }
    }
}
