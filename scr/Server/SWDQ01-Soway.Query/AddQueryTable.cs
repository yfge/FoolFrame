using System;
using System.Collections.Generic;
using System.Text;

namespace Soway.Query
{
    /// <summary>
    /// 表示在一个查询中加入连接表时返回的结果
    /// </summary>
    public enum  AddQueryTable
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success=0,
        /// <summary>
        /// 加入的表与现在查询的表没有关联信息，即加入连接失败
        /// </summary>
        NoRelation=1,
        /// <summary>
        /// 现有查询中已经包含了要加入的表
        /// </summary>
        Exists=2
    }
}
