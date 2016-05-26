using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Model
{
    internal class ObjectHash<T>
    {

        private List<object> keys = new List<object>();
        private List<T> items = new List<T>();


        public T this[object ob]{
            get{

                var index = keys.IndexOf(ob);
                if (index >= 0)
                    return items[index];
                return default(T);
            }
        }
        public bool ContainsKey(object ob)
        {
            return this.keys.Contains(ob);
        }


        public void Add(object ob, T item)
        {
            if (this.keys.Contains(ob))
                throw new Exception("已经存在");
            this.keys.Add(ob);
            this.items.Add(item);
        }

    }
}
