using System;
using System.Collections.Generic;
using System.Text;

namespace Soway.Data.DS.Tree
{
    /// <summary>
    /// 表明一个委托，返回两个结点比较铁结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="child"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public delegate TeeNodeCompareResult TeeNodeCompare<T>(T child, T parent);
}
