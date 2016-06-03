using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Data.Discription.ORM
{
    /// <summary>
    /// 表示一个动态更新的列属性
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <example>
    /// 所谓的动态更新指的是在生成Update语句时，语句生成的引擎不会基于现有的属性的值去生成update值，而是以表达式计算的形式生成语句
    /// <code>
    /// // geration Sql "update test set sum = sum+@add where id =@id"
    /// [Table(TableName="TestTable")]
    /// class Test {
    /// 
    /// [Column(IsKey=true,ColName="Id")]
    /// public Id{get;set;}
    /// 
    /// [Column(ColName="sum")]
    /// [DynamicColumn(SourcePropertyName="Add",DanymicOperationType=DanymicOperationType.Add)]
    /// public decimal Sum{get;set;}
    /// 
    /// [Colum(NoMap=true)]
    /// public decimal Add{get;set;}
    /// }
    /// 
    /// 
    /// 
    /// </code></example>
    public class DynamicColumnAttribute:Attribute
    {
        /// <summary>
        /// 更新的源属性
        /// </summary>
        public String SourcePropertyName { get; set; }
        /// <summary>
        /// 更新的类型
        /// </summary>
        public DanymicOperationType Operation { get; set; }
    }
}
