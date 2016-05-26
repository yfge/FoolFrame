using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Report
{
    public class SingleCell
    {
        public SingleCell()
        {

            Span = 1;
            OtherSpan = 1;
        }

        /// <summary>
        /// 值
        /// </summary>
        public string Value
        {
            get;
            set;
        }

        /// <summary>
        /// 表达式
        /// </summary>
        public string Expression
        {
            get;
            set;
        }
        /// <summary>
        /// 跨度
        /// </summary>
        public int Span
        {
            get;
            set;
        }

        public int OtherSpan
        {
            get;
            set;
        }

        public bool MegerToParent { get; set; }
        public override string ToString()
        {

            return string.Format("{0} {1}", Value, Span);
        }
    }
}
