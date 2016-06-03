using System;
using System.Collections.Generic;
using System.Text;
using Soway.Data.Discription.ORM;


namespace Soway.Query
{
    /// <summary>
    /// 选择列的形式，即是直接输出，还是按聚合类输出 
    /// </summary>
    /// 

    [Table(Name="SE_SELECTEDTYPE")]
    public class SelectType
    {

        private string show;
        /// <summary>
        /// 面向用户的表达 
        /// </summary>
        /// <remarks>显示给用户的具体值.</remarks>
        /// 
        [Column(ColumnName = "SE_SELECTEDSHOW")]
        public string Show
        {
            get;
            set;
        }
        /// <summary>
        /// 在Sql语句中的表达式
        /// </summary>
        /// <remarks>
        /// 在sql语句中的具体表达式
        /// </remarks>
        /// 
        [Column(ColumnName="SE_SELECTEDEXP")]
        public string DBExp
        {
            get;
            set;
        }

               [Column(ColumnName = "SE_REQUIREGROUP")]
        public bool RequireGroupCol { get; set; }

        [Column(ColumnName = "SysID",IsKey =true,IsIdentify =true,IsAutoGenerate = GenerationType.OnInSert)]
        public long ID { get; set; }
   
         
    }
}
