using System;
using System.Collections.Generic;
using System.Text;

namespace Soway.Data.Discription.ORM
{
    /// <summary>
    /// 表示自动生成的形式
    /// </summary>
    public  enum GenerationType
    {

        /// <summary>
        /// 从不
        /// </summary>
        Never=0,
        /// <summary>
        /// 在插入时自动生成 
        /// </summary>
        OnInSert = 1,
        /// <summary>
        /// 在更新时生成
        /// </summary>
        OnUpdate=2,
        /// <summary>
        /// 在插入和更新时生成
        /// </summary>
        OnInsertAndUpate = 3
    }
}
