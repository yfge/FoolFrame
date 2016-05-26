using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace Soway.Data.Discription.ORM
{
    /// <summary>
    /// 用于标记类以与数据库中的表对应
    /// </summary>
    /// <remarks>
    /// 这里加入一个列名的前缀，用于当子类从父类继承时生成 
    /// <code >
    /// 
    /// 
    /// public class A{
    /// [Column(ColName="No")]
    /// public string No {get;set;}
    /// [Column(ColName="TestNo2",
    /// public String No2{get;set;}
    /// }
    /// 
    /// // No --> B_No
    /// 
    /// Table(ColPreStr="B_",Name="TableB")]
    /// public class B:A{
    /// }
    /// </code></remarks>
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Interface)]
    public class TableAttribute :Attribute 
    {

        /// <summary>
        /// 表名
        /// </summary>
        public string Name { get; set; }



        /// <summary>
        /// 列表前缀
        /// </summary>
        public string ColPreStr { get; set; }


        public TableAttribute() { ColPreStr = ""; }


        public bool IsView { get; set; }

        /// <summary>
        /// 向用于显示的说明，用于自成自定义查询 
        /// </summary>
        public string ShowName { get; set; }
    }
}
