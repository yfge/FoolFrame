using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Model
{
    public enum PropertyTriggerType
    {
        /// <summary>
        /// 设置值时发生
        /// </summary>
        Set = 0,
        /// <summary>
        /// 当属性为集合时,增加时发生
        /// </summary>
        ItemsAdd = 1,
        /// <summary>
        /// 当属性为集合时,更改集合中项时发生
        /// </summary>
        ItemsSet = 2,
        /// <summary>
        /// 当属性为集合时,删除集合中项时发生
        /// </summary>
        ItemsDelete = 3,
    }
}
