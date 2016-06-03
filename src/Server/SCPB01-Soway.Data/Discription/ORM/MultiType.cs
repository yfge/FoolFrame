using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Data.Discription.ORM
{


    /// <summary>
    /// 表示一个多对多的应射
    /// </summary>
    [AttributeUsage(AttributeTargets.Property,AllowMultiple =false)]
    public class MultiTypeAttribute :Attribute 
    {
        
        /// <summary>
        /// 映射的表名，如不指定则为ParentTable_TO_ChildTable
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 映射的父类型的属性，如不指定则为id或是默认的列
        /// </summary>
        public string ParentProperty { get; set; }

        /// <summary>
        /// 映射的父列
        /// </summary>
        public string ParentColumn { get; set; }
        /// <summary>
        /// 子类型的属性
        /// </summary>
        public string ChildrenPeropery { get; set; }
        /// <summary>
        /// 子类型的列
        /// </summary>
        public string ChildrenColumn { get; set; }
    }
}
