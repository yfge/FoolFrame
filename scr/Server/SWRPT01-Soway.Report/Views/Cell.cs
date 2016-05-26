using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Report.Views
{
    public class Cell
    {
        public int Column
        {
            get;
            set;
        }

        public int Row
        {
            get;
            set;
        }
        public int ColSpan { get; set; }
        public int RowSpan { get; set; }
        public object Value
        {
            get;
            set;
        }

        public override string ToString()
        {

            return string.Format("Col:{0},Row{1},ColSpan:{2},RowSpan:{3},Value:{4}", Column, Row, ColSpan, RowSpan, Value);
        }
        public bool IsCalculate
        {
            get; set;
        }
        public CalDirection CalDirection
        {
            get; set;
        }
        public string CalScope { get; set; }
        public StaticType Expression { get; set; }


        public string GetScopeFromOffSet (int offSet)
        {
            var strs = this.CalScope.Split(new char[] { ',', '-' },StringSplitOptions.RemoveEmptyEntries);

            string result = "";
            for (int i = 0; i < strs.Length; i++)// str in strs)
            {
                var str = strs[i];
                if (i % 2 == 0)
                    result += ",";
                else
                    result += "-";
                result += (System.Convert.ToInt32(str) + offSet).ToString();
            }
            return result.Substring(1);


        }
    }
}
