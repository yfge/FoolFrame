using System;
using System.Collections.Generic;
 
using System.Text;
using System.Xml.Serialization;

namespace Soway.Query.BoolExp
{
    /// <summary>
    /// 表示一个布尔表达式
    /// </summary>
    /// 
 
    [Serializable]
    public class BoolExpression
    {
   

        public QueryInstance QueryIns { get; set; }
        
        /// <summary>
        /// 表达式的元子内容
        /// </summary>
        /// 
        public IBoolExpressionNode Exp { get; set; }
      
    }
}
