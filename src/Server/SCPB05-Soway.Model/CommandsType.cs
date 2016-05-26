using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Model
{
    public enum CommandsType
    {
        /// <summary>
        /// 设置值，即设置一个属性的值 
        /// </summary>
        SetValue = 0,
        /// <summary>
        /// 设置是否可用，即设置一个属性是否可以使用。
        /// </summary>
        SetAccess = 1,
        /// <summary>
        /// 执行，执行属性模型的一个操作
        /// </summary>
        ExuteProprtyModelMethod = 2,
        /// <summary>
        /// 执行外部模型的一个操作
        /// </summary>
        ExuteOutModelMethod = 3,
        /// <summary>
        /// 设置属性的数据源
        /// </summary>
        SetSource = 4,


        /// <summary>
        /// 当属性是列表时，执行列表的操作
        /// </summary>
        ExuteListMethod = 5,

        /// <summary>
        /// 条件
        /// </summary>
        Filter=6,

        /// <summary>
        /// 调用参数
        /// </summary>
        
        SetParamValue = 7,
        /// <summary>
        /// 构造函数
        /// </summary>
        SetConStrValue =8
    }
}
