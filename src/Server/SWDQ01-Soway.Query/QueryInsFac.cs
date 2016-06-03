using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Query
{
    public class QueryInsFac
    {
        public void RefreshQueryInsReportParam(QueryInstance ins)
        {
            if(ins.BoolExp != null  && ins.BoolExp.Exp!=null)
            {

                ins.BoolExp.Exp.GetSqlPart(0);
            }
        }
    }
}
