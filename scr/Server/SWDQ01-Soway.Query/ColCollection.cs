using System;
using System.Collections.Generic;
using System.Text;
using Soway.Query.Entity;
using System.Linq;

////hehehe 
namespace Soway.Query
{
    /// <summary>
    /// 表示查询的列的集合
    /// </summary>
    public class ColCollection : List<IQueryColumn>
    {
         

        /// <summary>
        /// 可以按列的名称检索列
        /// </summary>
        /// <param name="ColExp">列名称，即可以是表示给用户的名称，也可以是数据库中的名称</param>
        /// <returns>对应的列的实例</returns>
        public IQueryColumn this[String ColExp]{
            get{
                foreach (var cols in this)
                {

                    if (cols.DBName.Trim().ToUpper() == ColExp.Trim().ToUpper()
                             || cols.ShowName.Trim().ToUpper() == ColExp.Trim().ToUpper()
                             || cols.Table.DBName.Trim().ToUpper() + "." + cols.DBName.Trim().ToUpper() == ColExp.Trim().ToUpper()
                            || cols.Table.ShowName.Trim().ToUpper() + "." + cols.ShowName.Trim().ToUpper() == ColExp.Trim().ToUpper()
                         )
                        return cols;
                } return null;
                
            }
        }
    }
}
