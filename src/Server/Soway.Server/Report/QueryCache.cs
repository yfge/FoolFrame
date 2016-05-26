using Soway.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soway.Service.Report
{

    static class QueryCache
    {
        private static Dictionary<String, Dictionary<PropertyType, List<Soway.Query.SelectType>>> TypeCaches = new Dictionary<string, Dictionary<PropertyType, List<Query.SelectType>>>();
        private static Dictionary<String, Dictionary<PropertyType, List<Soway.Query.BoolExp.CompareOp>>> CopareCache = new Dictionary<string, Dictionary<PropertyType, List<Query.BoolExp.CompareOp>>>();
        
     
        public static List<Soway.Query.SelectType> GetSelectedType(Model.SqlCon con,PropertyType property)
        {
            var str = con.ToString();
            if (TypeCaches.ContainsKey(str) == false)
                TypeCaches.Add(str, new Dictionary<PropertyType, List<Query.SelectType>>());
            var dics = TypeCaches[str];
            if(dics.ContainsKey(property)==false)
            {
                dics.Add(property, new Soway.Query.SelectedTypeFac(str).GetSelectedType(property));
            }
            return dics[property];
        }

        public static List<Soway.Query.BoolExp.CompareOp> GetCompareType(Model.SqlCon con, PropertyType property)
        {
            var str = con.ToString();
            if (CopareCache.ContainsKey(str) == false)
                CopareCache.Add(str, new Dictionary<PropertyType, List<Query.BoolExp.CompareOp>>());
            var dics = CopareCache[str];
            if (dics.ContainsKey(property) == false)
            {
                dics.Add(property, new Soway.Query.BoolExp.CompareOpFac(str).GetSelectedType(property));
            }
            return dics[property];
        }
    }
}
