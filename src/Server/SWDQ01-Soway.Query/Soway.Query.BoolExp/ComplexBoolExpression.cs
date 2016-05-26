using System;
using System.Collections.Generic;
using System.Text;



namespace Soway.Query.BoolExp
{
    /// <summary>
    /// 表示复杂布尔表达式
    /// 
    /// </summary>
    public class ComplexBoolExpression :IBoolExpressionNode
    {

        /// <summary>
        /// 第一个布尔表达式
        /// </summary>
        public BoolExpression First { get; set; }
        /// <summary>
        /// 布尔表达的序列
        /// </summary>
        /// 
        [System.Xml.Serialization.XmlArray]
        public  ExpressionSequences Sequeces { get; set; }

        public override  BoolExpressionSqlPart GetSqlPart(int index)
        {

            int i = index;
            BoolExpressionSqlPart result = new BoolExpressionSqlPart();
            var temp = First.Exp.GetSqlPart(i);
            i += temp.Parameters.Count;
            result.Parameters.AddRange(temp.Parameters.ToArray());
            result.Stript = "(" + temp.Stript + ")";
            foreach (var add in this.Sequeces)
            {
                temp = add.BoolExpression.Exp.GetSqlPart(i);
                i += temp.Parameters.Count;
                result.Parameters.AddRange(temp.Parameters.ToArray());
                result.Stript += add.BoolOp.DBName + "(" + temp.Stript + ")";
            }
            return result;

        }
        internal ComplexBoolExpression(QueryInstance ins)
        {
            this.Sequeces = new ExpressionSequences();
            this.Onwer = ins;
        }



 
    }
}
