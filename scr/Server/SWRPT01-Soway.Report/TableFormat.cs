using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Report
{
    public class TableFormat
    {



        public TableFormat()
        {
            this.Colums = new List<CellFormat>();
            this.Rows = new List<CellFormat>();
            this.ValueCell = new List<ValueCell>();
        }
        public string SourceTable
        {
            get;
            set;
        }

        public List<CellFormat> Colums
        {
            get;
            set;
        }

        public List<CellFormat> Rows
        {
            get;
            set;
        }

        public List<Soway.Report.ValueCell> ValueCell
        {
            get;
            set;
        }
    }
}
