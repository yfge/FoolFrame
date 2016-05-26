using System;
using System.Collections.Generic;
using System.Text;

namespace Soway.Data.DS.Tree
{



    /// <summary>
    /// 生成树的工厂类
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    /// <typeparam name="I">接口</typeparam>
    public class ITreeFactory<T,I> where T:class,I,new()
    {

        private int CompareResult(T ob1, T ob2)
        { 
            return (int)compare(ob1, ob2);

        }
         
       TeeNodeCompareResult TreeCompare(ITreeData child, ITreeData parent) { 
           return child.TreeDataComPare(parent);
            
       }

        /// <summary>
        /// 新建一个树的生成器，同时传入一个委托，在生成树型结构时，会调用 传入的委托比较两个点
        /// </summary>
        /// <param name="Compare"></param>
        public ITreeFactory(Tree.TeeNodeCompare<T> Compare)
        {
            this.compare = Compare;
        }

      
        private Tree.TeeNodeCompare<T> compare;
 
        /// <summary>
        /// 按层建立树
        /// </summary>
        /// <param name="Items">建立树的集合</param>
        /// <returns>建立好的树结构</returns>
        public List<TreeNode<I>> CreateTreeByLevel
            (List<T> Items)
        {

            Items.Sort(new Comparison<T>(this.CompareResult));
            List<TreeNode<I>> result = new List<TreeNode<I>>();
            TreeNode<I> lastNode = null;
            Queue<TreeNode<I>>  queue = new Queue<TreeNode<I>>();
            TreeNode<I> currentNode = null;
            var current = result;
            if (Items.Count > 0)
            {



                for (int i = 0; i < Items.Count; i++)
                {



                    TreeNode<I> AddedNode = new TreeNode<I>()
                    {
                        Data = Items[i],
                        Parent = null,
                        Children =new List<TreeNode<I>>()
                    };//生成要添加的数据 

                    queue.Enqueue(AddedNode);//入队
                    //看是否到了下一层的结点
                    if (lastNode != null &&
                        (compare(AddedNode.Data as T, lastNode.Data as T) == Tree.TeeNodeCompareResult.Child
                         || compare(AddedNode.Data as T, lastNode.Data as T) == Tree.TeeNodeCompareResult.NexLevelNode)//下一层：即结点是子结点或是下一层结点
                        )
                    {
                        currentNode = queue.Dequeue();

                    }
                    //找到对应的父结点
                    while (currentNode != null
                        &&
                        compare(AddedNode.Data as T, currentNode.Data as T) != TeeNodeCompareResult.Child
                        &&queue.Count>0
                        )
                    {
                        currentNode = queue.Dequeue();
                    }
                    if (currentNode != null && compare(AddedNode.Data as T , currentNode.Data as T) != TeeNodeCompareResult.EquealNode)
                    {
                        AddedNode.Parent = currentNode;
                        current = currentNode.Children;
                    }
                    current.Add(AddedNode);
                    lastNode = AddedNode;
                }
            }
            return result;


        }

        public Tree<I> CreateTree(List<TreeNode<I>> items)
        {
            Tree<I> result = new Tree<I>();
            result.Nodes.AddRange(items);
            items.ForEach(p => { p.Tree = result; });
            return result;
        }


        public TreeNode<I> CreateNode(TreeNode<I> parent)
        {

            return new TreeNode<I>() { Data = new T(), Children = new List<TreeNode<I>>(), Parent = parent };

        }



        /// <summary>
        /// 计算一个树的宽度？起个什么名字比较好?
        /// </summary>
        /// <param name="tree"></param>
        /// <returns></returns>
        public int CalTreeWidth(Tree<I> tree)
        {

            int sum = 0;
            foreach (var node in tree.Nodes)
            {
                sum += CalNodeWidth(node);
            }
            return sum;

        }

        /// <summary>
        /// 计算一个结点的宽度
        /// </summary>
        /// <param name="Node"></param>
        /// <returns></returns>
        private int CalNodeWidth(TreeNode<I> node)
        {
            if (node.Children.Count == 0)
                return 1;
            else
            {
                int sum = 0;
                foreach (var childNode in node.Children)
                {
                    sum += CalNodeWidth(childNode);
                }
                return sum;
            }
        }


        public int  AddArrayToLeaf(Tree<I> tree,I[] itemArray)
        {
            return this.AddArrayToLeaf(tree.Nodes, itemArray, 0,null);
        }

        public int AddArrayToLeaf(List<TreeNode<I>> nodes, I[] itemArray, int offIndex = 0,TreeNode<I> parent=null)
        {
              TreeNode<I> addnode = null;
            int addCount=0;
            //加入的数组为空
            if (itemArray == null || itemArray.Length == 0 )
                return addCount ;


       
            //当前没有元素
            if (itemArray.Length - offIndex >= 1)
            {

                ///寻找是否存在相同的值
                int i = 0;
                while (i < nodes.Count)
                {

                    if (nodes[i].Data.Equals(itemArray[offIndex]))
                    {
                        addnode = nodes[i];
                        break;
                    }
                    else
                        addCount +=CalNodeWidth(nodes[i]);
                    i++;
                }

                //没有找到对应的值,加入新值
                if (addnode == null)
                {
                    addnode = new TreeNode<I>(){Data=itemArray[offIndex],Parent =parent,Level = parent==null ?0:parent.Level +1};
                    nodes.Add(addnode);

                }

                //接着增加
                return addCount + AddArrayToLeaf(addnode.Children,itemArray, offIndex + 1,addnode);
            }
            else
                addCount++;
            return addCount;
        }
        public List<TreeNode<I>> GetBottomNodes(Tree<I> tree)
        {
            List<TreeNode<I>> result = new List<TreeNode<I>>();
            foreach (var node in tree.Nodes)
            {
                result.AddRange(getBottomNodes(node));

            }
            return result;

        }

        private List<TreeNode<I>> getBottomNodes(TreeNode<I> node)
        {
            List<TreeNode<I>> result = new List<TreeNode<I>>();
            if(node.Children!=null && node.Children.Count > 0)
            {
                node.Children.ForEach(p =>
                {
                    result.AddRange(getBottomNodes(p));

                });
            }
            else
            {
                result.Add(node);
            }
            return result;
        }
       











    }
}
