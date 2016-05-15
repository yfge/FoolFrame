using System;
using System.Collections.Generic;
using System.Text;

namespace Soway.Data.DS
{
    public abstract class BasicEnum
    {
        public int Value { get; private set; }
        public string State { get; private set; }
        private static System.Collections.Hashtable _hash = new System.Collections.Hashtable();
   
        public static implicit operator int(BasicEnum ob)
        {
            return ob.Value;
        }
        public override string ToString()
        {

            return State;
        }

        protected static BasicEnum GetBy(int i, Type childtype)
        {

            System.Collections.Hashtable hash = _hash[childtype] as System.Collections.Hashtable;

            if (hash.ContainsKey(i))
                return hash[i] as BasicEnum;
            return null;
        }
        public BasicEnum(int v, string s,Type type)
        {
            Value = v;
            State = s;
            if (!_hash.ContainsKey(type))
                _hash.Add(type, new System.Collections.Hashtable());
            var isTable = _hash[type] as System.Collections.Hashtable;
            if (isTable.ContainsKey(v))
                throw new Exception("不能有重复的值");
            isTable.Add(v, this);

            System.IO.File.WriteAllLines("enum.txt", new string[] { s, type.ToString() });
            

        }


        public static BasicEnum[] ALL(Type type)
        {

            var isTable = _hash[type] as System.Collections.Hashtable;
            List<BasicEnum> a = new List<BasicEnum>();
            foreach (var i in isTable.Values)
                a.Add(i as BasicEnum);
            return a.ToArray();

        }






    }
}
