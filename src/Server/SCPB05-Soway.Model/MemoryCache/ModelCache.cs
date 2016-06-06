using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Model.MemoryCache
{
   internal static class ModelCache
    {
        private static System.Collections.Concurrent.ConcurrentDictionary<



            String,
            System.Collections.Concurrent.ConcurrentDictionary<String, Model>>
            Caches = new System.Collections.Concurrent.ConcurrentDictionary<string, System.Collections.Concurrent.ConcurrentDictionary<string, Model>>();

        public static  Model Get(String con,String typeName)
        {
            if (Caches.ContainsKey(con) == false)
                while(false==Caches.TryAdd(con, 
                    new System.Collections.Concurrent.ConcurrentDictionary<string, Model>()));
            var dblist = Caches[con];
            if (dblist.ContainsKey(typeName) == false)
                while (false == dblist.TryAdd(typeName,
                    new Model()));
            return dblist[typeName];
            

        }




    }
}
