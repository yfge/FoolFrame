using Soway.Model.Query;
using Soway.Query.BoolExp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soway.Service.Report
{
    class BoolExpAdapter
    {
        private  List<ReportCol> Cols { get;  set; }
        private Model.Query.QueryModel QModel { get;  set; }

        public BoolExpAdapter(Model.Query.QueryModel qmodel)
        {
            this.QModel = qmodel;
        }
        public Soway.Query.BoolExp.BoolExpression GetQueryExp(Soway.Service.Report.BoolExp exp, QueryModel model,Model.SqlCon con,Soway.Query.QueryInstance ins)
        {
            Soway.Query.BoolExp.BoolExpresstionFacotry fac = new Query.BoolExp.BoolExpresstionFacotry(ins);
            var result = new BoolExpression();
            if (exp.Col != null)
            {
                //简单类型
                var col = model.Columns.First(p => p.ID == exp.Col.ID);

                result.Exp =  fac.CreateBoolExpression(new Query.CompareCol()
                {
                    Col = col,
                    SelectedTableName = model.Table.ShowName
                }, QueryCache.GetCompareType(con, col.DataType).First(p => p.ID.ToString() == exp.CompareOp.ID), exp.ValueExp,
                exp.ValueFmt,exp.ParamName);
                 
            }else if (exp.FirstExp != null)
            {
                //复杂类型
               
                result =
                    GetQueryExp(exp.FirstExp, model, con,ins);
                foreach(var sequence in exp.Sequences)
                {
                    fac.AddBoolExpression(
                        result,
                        sequence.BoolOp,
                        GetQueryExp(sequence.AddedExp, model, con,ins));
                }


            }
            
            
            return result;
            
        }

        public Soway.Service.Report.BoolExp GetContractExp(Soway.Query.BoolExp.BoolExpression exp)
        {
            return null;
        }
    }
}
