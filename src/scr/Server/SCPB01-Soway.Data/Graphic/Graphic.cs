using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Data.Graphic
{
    public class Graphic<T> where T: class
    {
        public List<GraphicNode<T>> Nodes = new List<GraphicNode<T>>();


        public Graphic()
        {
            Nodes = new List<GraphicNode<T>>();
        }
        public T GetTopNode()
        {

            var first= this.Nodes.FirstOrDefault(p => p.PointIn.Count()== 0);
            if (first != null)
            {
                return first.Data;
            }
            else
            {
                foreach (var item in this.Nodes)
                {
                    if (item.PointIn.Contains(item.Data) && item.PointIn.Count == 1)
                        return item.Data;
                }
            }
            return null;
        }

        public void Remove(T data)
        {
            var node = Nodes.First(p => p.Data == data);
            foreach (var other in Nodes)
            {

                if (other.PointIn.Contains(node.Data))
                    other.PointIn.Remove(node.Data);
                if (other.PointOut.Contains(node.Data))
                    other.PointOut.Remove(node.Data);
            }

            Nodes.Remove(node);// (p => p.Data == data);
        }



        public void AddEdge(T from, T to)
        {

            if (this[from] == null)
                this.Nodes.Add(new GraphicNode<T>(from));
            if(this[to]==null)
            this.Nodes.Add(new GraphicNode<T>(to));

            if(this[from].PointOut.Contains(to)==false)
             
            this[from].AddOut(to);
            if(this[to].PointIn.Contains(from)==false)
            this[to].AddIn(from);


          //  // System.Diagnostics.Trace.WriteLine(String.Format("{0}->{1}", from, to));

        }

        public GraphicNode<T> this[T index]
        {

            get
            {
                return this.Nodes.FirstOrDefault(p => p.Data == index);
            }

        }

        public void   Add(T data){
            if (this.Nodes.FirstOrDefault(p => p.Data == data) == null)
                this.Nodes.Add(new GraphicNode<T>(data));


        }

         
        
    }
}
