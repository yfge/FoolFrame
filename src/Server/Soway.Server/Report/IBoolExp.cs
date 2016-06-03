using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soway.Service.Report
{
    public   class BoolExp
    {
        public QueryCol Col { get; set; }
        public CompareOpItem CompareOp { get; set; }
        public String ValueExp { get; set; }
        public string ValueFmt { get; set; }

        public BoolExp FirstExp { get; set; }
        public List<AddBoolExp> Sequences { get; set; }

        public String ParamName { get; set; }

        public bool IsFixed { get; set; }
        public bool IsMerged { get; set; }
    }
}
