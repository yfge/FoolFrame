using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Model.Report
{
    public class Report
    {
        public string Name
        {
            get; set;
        }

        public View.View CoreView
        {
            get;set;
        }

        public List<AddedView> AddedViews
        {
            get; set;
        }

        public List<ReportCol> ReportCols { get; set; }
        public List<Param> Params { get; set; }

        public int BoolExp
        {
            get; set;
        }

        public int Id
        {
            get;set;
        }
    }
}
