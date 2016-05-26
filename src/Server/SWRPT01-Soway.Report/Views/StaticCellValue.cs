using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Report.Views
{
    internal class StaticCellValue
    {
        public StaticFormat CellFormat { get; set; }
        public object[] StaticValue { get; set; }
        public int StaticIndex { get; set; }
        public string StaticFilter { get; set; }
    }
}
