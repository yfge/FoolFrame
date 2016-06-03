
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Report
{
    public   class ReportEmptyValue
    {
        private ReportEmptyValue (){}
        static ReportEmptyValue (){
            Value = new ReportEmptyValue();
        }
        public static ReportEmptyValue Value { get; private set; }
    }
}
