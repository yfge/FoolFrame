using System;
using System.Collections.Generic;
using System.Text;

namespace Soway.Data.Discription.ORM
{
    /// <summary>
    /// 用在条目类中的父类属性
    /// </summary>
    /// 
    [AttributeUsage(AttributeTargets.Property)]
    public class ParentRelationAttribute :Attribute 
    {
        /// <summary>
        /// 引用的父属性的名称
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// ORM的列名称
        /// </summary>
        public string ColumnName { get; set; }
    }
}
