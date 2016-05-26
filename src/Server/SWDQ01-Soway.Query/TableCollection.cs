using System;
using System.Collections.Generic;

using System.Text;
using Soway.Query.Entity;

namespace Soway.Query
{
    /// <summary>
    /// 表的集合，重写了this[]，增加了按名称索引表的功能
    /// 1
    /// 1
    /// 1
    /// 1
    /// 1
    /// </summary>
    public class TableCollection : List <IQueryTable>
    {

        public IQueryTable this[String Exp]
        {
            get
            {
                foreach (IQueryTable table in this)
                {
                    if (table.DBName.ToUpper().Trim() == Exp.ToUpper().Trim() || table.ShowName.ToUpper().Trim() == Exp.ToUpper().Trim())
                        return table;
                }
                return null;
            }
        }

    }
}
