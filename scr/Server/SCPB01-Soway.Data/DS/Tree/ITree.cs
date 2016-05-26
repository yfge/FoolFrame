using System;
using System.Collections.Generic;
using System.Text;

namespace Soway.Data.DS.Tree
{

    /// <summary>
    /// 表示树的定义
    /// </summary>
    /// <remarks>
    /// 这个类型实现了IEnumerable接口
    /// 注意，当进行遍历时，是进行的按层遍历</remarks>
    /// <typeparam name="T"></typeparam>
    public class Tree <T> :System.Collections.IEnumerable 
    {
        /// <summary>
        /// 树的顶层结点
        /// </summary>
        public List<TreeNode<T>> Nodes { get; private set; }


        public Tree()
    {
        this.Nodes = new List<TreeNode<T>>();
       
         
    }


        public IEnumerator<TreeNode<T>> GetEnumerator()
        {
            Queue<TreeNode<T>> queue = new Queue<TreeNode<T>>();
            foreach (var node in this.Nodes)
                queue.Enqueue(node);
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                yield return current;//.Data;
                for (int i = 0; i < current.Children.Count; i++)// auth in current)
                {
                    //yield return current.Nodes[i].Data;
                    queue.Enqueue(current.Children[i]);
                }
            }
        }


         
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            Queue<TreeNode<T>> queue = new Queue<TreeNode<T>>();
            foreach (var node in this.Nodes)
                queue.Enqueue(node);
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                yield return current;//.Data;
                for (int i = 0; i < current.Children.Count; i++)// auth in current)
                {
                   // yield return current.Data;//[i].Data;
                    queue.Enqueue(current.Children[i]);
                }
            }
        }


     

       


       
    }
}
