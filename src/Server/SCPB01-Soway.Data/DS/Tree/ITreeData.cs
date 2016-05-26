using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Data.DS.Tree
{
    /// <summary>
    /// 一个表示树结点的接口
    /// 设计它 的意义主要是为了可以调用setParent
    /// </summary>
    public interface ITreeData 
    {
        TeeNodeCompareResult TreeDataComPare(ITreeData ob);
        void SetParent(ITreeData ob);

       

    }
}
