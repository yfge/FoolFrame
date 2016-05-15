using System;
using System.Collections.Generic;
using System.Text;

namespace Soway.Query.Entity
{

    /// <summary>
    /// 接口QueryAtom表示构成查询的一个基本语法元素
    /// </summary>
    /// <remarks>
    /// <p>由于SQL语句本身与中文语义的不同，以及表现层和数据层之间的区别.对于每一个查询元素，需要有面向用户的表示以及面向数据库的表示。</p>
    /// <p>如，在一个权限的系统中，用户表可能被设计为表Users,而面向用户，而应该表现为“用户”或是“人员”，IQueryAtom则实现这种表示的架构。</p>
    /// 
    /// </remarks>
    public interface IQueryAtom
    {
        /// <summary>
        /// 面向用户表示的名称
        /// </summary>
        string ShowName { get; set; }
        /// <summary>
        /// 数据库系统中的名称
        /// </summary>
        String DBName { get; set; }
    }
}
