using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Model.Report
{
    public class Param
    {
        public int ViewIndex { get; set; }

        public ViewItem Item { get; set; }
        public String Name { get; set; }
        public string Exp { get; set; }
        public bool UserInput { get; set; }
    }
}
