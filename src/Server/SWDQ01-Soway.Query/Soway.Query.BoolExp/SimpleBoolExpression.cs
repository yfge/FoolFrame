using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Soway.Query.Entity;

namespace Soway.Query.BoolExp
{
    /// <summary>
    /// 表示一个简单的布尔表达式
    /// </summary>
    /// <remarks>
    /// 在
    /// </remarks>
    public class  SimpleBoolExpression :IBoolExpressionNode
    {
        /// <summary>
        /// 列
        /// </summary>
        public CompareCol Col { get; set; }

        /// <summary>
        /// 操作
        /// </summary>
        public CompareOp Op { get; set; }




        private object filtervalue;
        /// <summary>
        /// 值
        /// </summary>
        public object Value { get
            {
                return this.filtervalue;
            }
            set
            {
                this.filtervalue = value;

            }

        }

        private string valuefmt;
        /// <summary>
        /// 值的格式化串
        /// </summary>
        public string ValueStr
        {
            get
            {
                return valuefmt;
            }
            set
            {
                valuefmt = value;

            }

        }


        /// <summary>
        /// 参数名
        /// </summary>
        public string ParamName { get; set; }


     




        public override BoolExpressionSqlPart GetSqlPart(int index)
        {
            BoolExpressionSqlPart result = new BoolExpressionSqlPart();

              string sqlPramName = "";
            //如果参数名不为空
            if (String.IsNullOrEmpty(this.ParamName) == false)
            {
                //检查是否已经有了参数
                var param = this.Onwer.ReportParams.FirstOrDefault(p => p.Name == this.ParamName);
                if (param  ==null)
                {

                    //如果没有，加入
                    this.Onwer.ReportParams.Add(new ReportParameter()
                    {
                        Name = this.ParamName,
                        Exp = "@p" + index,
                        FmtValue = this.ValueStr,
                        Value = (this.Value??"").ToString()

                    });
                    result.Parameters.Add(new QueryParameter()
                    {
                        Column = this.Col,
                        SqlParam = new System.Data.SqlClient.SqlParameter("@p" + index.ToString(), this.Value)
                    });
                    sqlPramName = "@p" + index;

                }
                else
                {
                    //如果有了，不处理 进行取值 
                    this.ValueStr = param.FmtValue;
                    this.Value = param.Value;
                    sqlPramName = param.Exp;
                }

            }else
            {
                result.Parameters.Add(new QueryParameter()
                {
                    Column = this.Col,
                    SqlParam = new System.Data.SqlClient.SqlParameter("@p" + index.ToString(), this.Value)
                });
                sqlPramName = "@p" + index.ToString();

            }
            result.Stript = string.Format(this.Op.DBName,
                String.Format("[{0}].[{1}]", this.Col.SelectedTableName, 
                this.Col.Col.DBName),
                    sqlPramName);
            
             
            return result;

        }
        public SimpleBoolExpression(CompareCol col,CompareOp op,object value,String str,QueryInstance owner ,string pramName) {
            Col = col;
            Op = op;
            Value = value;
            ValueStr = str;
            this.Onwer = owner
                ;
            this.ParamName = pramName;
        }


        public override string ToString()
        {

            return string.Format("{0}.{1} {2} {3}", this.Col.SelectedTableName, this.Col.Col.ShowName, this.Op.ShowName, this.ValueStr);
        }

       
    }
}
