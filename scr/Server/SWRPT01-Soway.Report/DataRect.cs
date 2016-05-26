using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Report
{
    public class DataRect
    {
        public DataRect()
        {
            Cells = new List<Views.Cell>();
        }
        public int ColHeaderIndex
        {
            get;
            set;
        }

        public int RowHeaderIndex
        {
            get;
            set;
        }

        public List<Soway.Report.Views.Cell> Cells
        {
            get;
            set;
        }

        
    }
}
