using System;
using System.Collections.Generic;
using System.Text;
using Soway.Query.Entity;
using Soway.Data.Discription.ORM;
using Soway.Data;

namespace Soway.Query.BoolExp
{
    /// <summary>
    /// 比较操作符，用于数据列和值之间的比较
    /// </summary>
    /// 
    [Table(Name = "SE_COMPARETYPE")]
    public class CompareOp :IQueryAtom
    {

        public CompareOp() { }




        [Column(ColumnName = "SE_COMPARESHOW")]


        public string ShowName
        {
            get;
            set;
        }
        [Column(ColumnName = "SE_COMPAREEXP")]
        public string DBName
        {
            get;
            set;
        }

        [Column(ColumnName = "PROPERTYTYPE_VALUE")]
        public PropertyType PropertyType
        {
            get; set;
        }

        [Column(ColumnName = "SysID",IsKey =true,IsIdentify =true,IsAutoGenerate = GenerationType.OnInSert)]
        public long ID { get; set; }

    }
}
