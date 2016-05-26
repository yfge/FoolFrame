using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Report.Views
{
    public class MatrixHeader :IComparable
    {

        public CellFormat FormatCell { get; set; }
        public object Value { get; set; }


        public bool Compute
        {
            get;
            set;
        }
        public StaticFormat StaticCell { get; set; }  
        public string ComputeExp { get; set; }




        public int CompareTo(object obj)
        {
            var ftm = obj as MatrixHeader;
            if (ftm == null)
                return -1;
            if (ftm == this || (ftm.FormatCell == this.FormatCell 
                && ftm.Value.Equals(this.Value) && ftm.StaticCell == this.StaticCell))
                return 0;
            return -1;
        }
        public override bool Equals(object obj)
        {

            return CompareTo(obj) == 0;
        }
    }
}
