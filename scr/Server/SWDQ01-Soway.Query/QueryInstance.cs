using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Query
{
    public class QueryInstance
    {
        public SelectedTables SelectedTables
        {
            get;
            set;
        }

        public SelectedColCollection SelectedCols
        {
            get;
            set;
        }

        private BoolExp.BoolExpression exp;
        public BoolExp.BoolExpression BoolExp
        {
            get
            {
                return exp;
            }
            set
            {
                exp = value;
               
              

            }
        }

        public List<QueryParameter> Params
        {
            get;
            set;
        }


        public QueryInstance()
        {
            this.SelectedCols = new SelectedColCollection();
            this.Params = new List<QueryParameter>();
            this.ReportParams = new List<ReportParameter>();
        }

        public List<ReportParameter> ReportParams { get; set; }
    }
}
