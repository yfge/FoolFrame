using System; 
using System.Collections.Generic;
using System.Text;

namespace Soway.Query.BoolExp
{
    /// <summary>
    /// 一个布尔表达式的元子内容
    /// </summary>
    /// 

    [System.Xml.Serialization.XmlInclude(typeof(SimpleBoolExpression))]
    [System.Xml.Serialization.XmlInclude(typeof(ComplexBoolExpression))]
    public abstract class IBoolExpressionNode 
    {
        /// <summary>
        /// 得到这个布尔表达式的sql组成部分
        /// </summary>
        /// <param name="index">参数起始的索引值</param>
        /// <returns>该布尔表达式的Sql组成</returns>
         public abstract  BoolExpressionSqlPart GetSqlPart(int index);

        public QueryInstance Onwer { get; set; }
    }
}
