using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Report
{

    /// <summary>
    /// 单元格定义
    /// </summary>
    public class CellFormat
    {
        public CellFormat()
        {
            StaticFormats = new List<StaticFormat>();
        }
        /// <summary>
        /// 源列名（Table中的Column.Name)
        /// </summary>
        public string SourceColumn
        {
            get;
            set;
        }
        /// <summary>
        /// 输出的列名
        /// </summary>

        public string ColName
        {
            get;
            set;
        }

        /// <summary>
        /// 格式
        /// </summary>

        public string Format
        {
            get;
            set;
        }
        /// <summary>
        /// 排序的辅助列
        /// </summary>
        public string OrderColumn
        {

            get;
            set;
        }
        /// <summary>
        /// 排序类型
        /// </summary>
        public OrderType OrderType
        {
            get;
            set;
        }

        /// <summary>
        /// 统计列
        /// </summary>
        public List<Soway.Report.StaticFormat> StaticFormats
        {
            get;
            set;
        }
    }
}
