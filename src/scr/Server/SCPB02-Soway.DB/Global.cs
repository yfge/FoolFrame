using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
namespace Soway.DB
{
    public static  class GlobalSqlContext
    {

        private class ConSet: IEqualityComparer {
            public Type type{get;set;}
            public String ViewName{get;set;}




            bool IEqualityComparer.Equals(object x, object y)
            {

                var obsx = x as ConSet;
                var obsy = y as ConSet;

                if (obsx != null && obsy != null)
                    return obsx.type == obsy.type && obsx.ViewName == obsy.ViewName;
                return false;
            }


            int IEqualityComparer.GetHashCode(object obj)
            {
                var ob = (obj as ConSet)??this;

                return (ob.type.ToString() + ob.ViewName).GetHashCode();
            }
        }
        public static string ConStr { get; set; }


        private static Dictionary<Type, String> table = new Dictionary<Type, string>();
     
        public static  void RegSqlCon(Type type,string KeyName, String SqlCon)
        {
            var con = new ConSet() { type = type, ViewName = KeyName };
            if (table.ContainsKey(type))
                table[type] = SqlCon;
            else
                table.Add(type, SqlCon);
       
       
        }

        public static string GetConStr(Type type, String KeyName)
        {


            var con = new ConSet() { type = type, ViewName = KeyName };

            if (table.ContainsKey(type))
                return table[type].ToString();
            else
                return ConStr;
        }
    }
}
