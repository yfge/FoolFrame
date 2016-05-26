using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Report.Views
{
    public class MatrixTable
    {
        public MatrixTable()
        {
            ColHeaders = new List<List<SingleCell>>();
            RowHeaders = new List<List<SingleCell>>();
            Cells = new List<DataRect>();
        }


        /// <summary>
        /// 列头
        /// </summary>
        public List<List< SingleCell>> ColHeaders
        {
            get;
            set;
        }

        /// <summary>
        /// 行头
        /// </summary>
        public List<List<SingleCell>> RowHeaders
        {
            get;
            set;
        }

        /// <summary>
        /// 输出的数据
        /// </summary>
        public System.Collections.Generic.List<DataRect> Cells
        {
            get;
            set;
        }
    }
}
