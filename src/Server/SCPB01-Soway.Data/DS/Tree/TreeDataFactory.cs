using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Data.DS.Tree
{ 
    public class TreeDataFactory <T> :ITreeFactory<T,T> where T :class, ITreeData,new()
    {

        private static TeeNodeCompareResult ITreeDataCompare(T child, T parent){
            return (child ).TreeDataComPare(parent);
        }
        public TreeDataFactory() : base(new Soway.Data.DS.Tree.TeeNodeCompare<T>(ITreeDataCompare)) { }


         
    }
}
