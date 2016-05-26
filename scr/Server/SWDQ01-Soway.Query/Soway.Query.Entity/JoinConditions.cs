using System;
using System.Collections.Generic;
 
using System.Text;

namespace Soway.Query.Entity
{
    /// <summary>
    /// 表连接条件的集合
    /// </summary>
    /// <remarks>
    /// 派生此类的原因是因为在一些情况下，两个表的连接是复杂连接的，即有可能为</br>
    /// from tableA join tableB on C1=C3 and C2=C4
    /// <p>在这种情况下，设计JoinConditions表示两个表连接的条件集</p>
    /// </remarks>
    public class JoinConditions : List<JoinCondition>
    {

         
    }
}
