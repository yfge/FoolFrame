using Soway.Data;
using System;
using System.Collections.Generic;
using System.Text;
 

namespace Soway.Query.Entity
{
    /// <summary>
    /// 数据库中表的列
    /// </summary>
    public class IQueryColumn :IQueryAtom
    {
        /// <summary>
        /// 所属的表
        /// </summary>
        public IQueryTable Table { get; set; }
        /// <summary>
        /// 向用户表现的数据类型
        /// </summary>
        public  PropertyType DataType { get; set; }
        /// <summary>
        /// 格式化字符串
        /// </summary>
       public string FormatStr { get; set; }
        /// <summary>
        /// 是否是自增长ID
        /// </summary>
       public  bool IsIdentity { get; set; }


       public bool IsKey { get; set; }


        public string ShowName
        {
            get;
            set;
        }

        public string DBName
        {
            get;
            set;
        }

        public String ID { get; set; }
    }
}
