using System;
using System.Collections.Generic;
using System.Text;

namespace Soway.Data.DS.Tree
{

    /// <summary>
    /// 表示树型结构中的一个结点
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class  TreeNode<T>

    {

        
       
        public Tree<T> Tree { get; set; }


        public TreeNode()
        {
            Children = new List<TreeNode<T>>();
         
        }

        /// <summary>
        /// 结点的数据
        /// </summary>
        public T Data { get; set; }
        /// <summary>
        /// 父结点
        /// </summary>
        public TreeNode<T> Parent { get; set; }

        /// <summary>
        /// 子结点
        /// </summary>
        public List<TreeNode<T>> Children { get; set; }

        /// <summary>
        /// 得到同级结点的下一结点
        /// </summary>
        /// <returns></returns>
        public TreeNode<T> GetNext()
        {
            if (this.Parent != null)
            {
                int i = this.Parent.Children.IndexOf(this);
                if (i < this.Parent.Children.Count - 1)
                    return this.Parent.Children[i + 1];

            }
            return null;
        }


        private int width;
        public int Width
        {
            get
            {
                if (this.Children.Count == 0)
                    return 1;
                else
                {
                    int sum = 0;
                    foreach (TreeNode<T> node in this.Children)
                        sum += node.Width;
                    return sum;
                }
            }
        }


        public int Level
        {
            get;
            internal set;
        }


        
        
    }

 
       
}
