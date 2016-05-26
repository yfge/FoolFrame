using System;
using System.Collections.Generic;
 
using System.Text;
using Soway.Query.Entity;

namespace Soway.Query.BoolExp
{
    /// <summary>
    /// 对于布尔表达式，AddBoolExpression表示一个布尔运算符（And 或 Or)与一个布尔表达式的组合
    /// </summary>
    public class AddBoolExpression  
    {
      
    
        /// <summary>
        /// 布尔运算符
        /// </summary>
        public BoolOp BoolOp
        {
            get;
            set;
        }


        /// <summary>
        /// 附加的布尔表达式
        /// </summary>
        public BoolExpression BoolExpression
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="op">布尔运算符</param>
        /// <param name="exp">要组合的表达式</param>
        public AddBoolExpression(BoolOp op, BoolExpression exp)
        {
            BoolOp = op;
            BoolExpression = exp;
        }

        public AddBoolExpression() { }

    }
}
