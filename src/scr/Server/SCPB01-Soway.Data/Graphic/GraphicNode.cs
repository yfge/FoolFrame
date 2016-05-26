using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Data.Graphic
{
    public class GraphicNode<T>
    {
        public T Data { get; set; }

        internal List<T> PointIn { get; set; }
        internal List<T> PointOut { get; set; }



        public GraphicNode(T data)
        {
            this.Data = data;
            this.PointIn = new List<T>();
            this.PointOut = new List<T>();
        }


        public bool AddIn(T item)
        {
            if (this.PointIn.Contains(item))
                return false;
            else
            {
                this.PointIn.Add(item);
                return true;
            }
        }

        public bool AddOut(T item)
        {
            if (this.PointOut.Contains(item))
                return false;
            else
            {
                this.PointOut.Add(item);
                return true;
            }
        }
       
    }
}
