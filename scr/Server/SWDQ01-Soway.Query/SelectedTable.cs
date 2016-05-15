using Soway.Query.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Query
{

    public class SelectedTable
    {

        /// <summary>
        /// 表
        /// </summary>
        public IQueryTable Table { get; set; }
        /// <summary>
        /// 选择的表的名称
        /// </summary>
        public String SelectedTableName { get; set; }

        //public List<JoinTable> JoinTables { get; set; }


         
    }
}
