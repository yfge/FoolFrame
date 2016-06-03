using System;
using System.Collections.Generic;
using System.Text;

////
namespace Soway.Query
{
    /// <summary>
    /// 
    /// </summary>
    public class ReportParameter
    {
        public string Name { get; set; }

        public string Exp { get; set; }
        public object Value { get; set; }
        public string FmtValue { get; set; }
    }
}
