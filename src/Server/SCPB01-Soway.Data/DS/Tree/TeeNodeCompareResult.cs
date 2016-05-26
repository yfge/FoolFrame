using System;
using System.Collections.Generic;
using System.Text;

namespace Soway.Data.DS.Tree
{

    /// <summary>
    /// 表示两个树结点比较的结果
    /// </summary>
    public enum  TeeNodeCompareResult
    {
        /// <summary>
        /// 树结点
        /// </summary>
        Parent=-2,
        /// <summary>
        /// 子结点
        /// </summary>
        Child=1,
        /// <summary>
        /// 下一个同级结点
        /// </summary>
        NextNode=2,
        /// <summary>
        /// 前一个同级结点
        /// </summary>
        PreNode=-1,
        /// <summary>
        /// 同一个结点
        /// </summary>
        EquealNode =0,
        /// <summary>
        /// 下一层的结点
        /// </summary>
        NexLevelNode=3

    }
}
