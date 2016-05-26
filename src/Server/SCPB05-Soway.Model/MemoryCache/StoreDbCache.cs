using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Model.MemoryCache
{
    static class StoreDbCache
    {
        private static System.Collections.Concurrent.ConcurrentDictionary
            <string, System.Collections.Concurrent.ConcurrentDictionary<string,
                System.Collections.Concurrent.ConcurrentDictionary<object, CacheItem>>>
           DbCachedList = new System.Collections.Concurrent.ConcurrentDictionary<string, System.Collections.Concurrent.ConcurrentDictionary<string, System.Collections.Concurrent.ConcurrentDictionary<object, CacheItem>>>();

        private static System.Collections.Concurrent.ConcurrentDictionary
            <string, System.Collections.Concurrent.ConcurrentDictionary<string,
                System.Collections.Concurrent.ConcurrentDictionary<object, CacheItem>>>
           MemoryCachedList = new System.Collections.Concurrent.ConcurrentDictionary<string, System.Collections.Concurrent.ConcurrentDictionary<string, System.Collections.Concurrent.ConcurrentDictionary<object, CacheItem>>>();
        public static IObjectProxy GetDbMemoryCache(SqlCon con, Model Model, object id,Context.ICurrentContextFactory confac)
        {



            var modellist =
                GetCachedList(con.ToString(), Model.Name, DbCachedList);
            if (modellist.ContainsKey(id))
            {
                var ob = modellist[id];
                ob.LastUsedTime = DateTime.Now;
                ob.UsedCount++;
                return ob.CachedOb;
            }else
            {
                var memitem = new CacheItem();
                memitem.Key = id;
                memitem.CachedOb = new SqlDataProxy(Model, confac, LoadType.Null, con) {
                    ID = id
                };
                modellist.TryAdd(id, memitem) ;
                return memitem.CachedOb;
            }
        }
       
        public static void UpdateOrAddDbMemoryCache(Soway.Model.SqlCon con,Soway.Model.Model model,object id,IObjectProxy proxy)
        {



            var modellist = GetCachedList(con.ToString(), model.Name, DbCachedList);
            if (modellist.ContainsKey(id) == false)
            {
                MemoryCache.CacheItem item = new CacheItem();
                item.CachedOb = proxy;
                item.Key = id;

                modellist.TryAdd(id, item) ;
            }else
            {
                var item = modellist[id];
                item.CachedOb = proxy;
                item.UpdateTime = DateTime.Now;
            }
        }
           

    

        public static IObjectProxy GetObjMemoryCache()
        {
            return null;
        }
    
        private static System.Collections.Concurrent.ConcurrentDictionary<object, CacheItem> 
            GetCachedList(String ConStr,String ModelName, System.Collections.Concurrent.ConcurrentDictionary
            <string, System.Collections.Concurrent.ConcurrentDictionary<string,
                System.Collections.Concurrent.ConcurrentDictionary<object, CacheItem>>> CachedDictionary)
        {

            if (CachedDictionary.ContainsKey(ConStr) == false)
                CachedDictionary.TryAdd(ConStr, new System.Collections.Concurrent.ConcurrentDictionary<string, System.Collections.Concurrent.ConcurrentDictionary<object, CacheItem>>());
            var dblist = CachedDictionary[ConStr];
            if (dblist.ContainsKey(ModelName) == false)
                dblist.TryAdd(ModelName, new System.Collections.Concurrent.ConcurrentDictionary<object, CacheItem>());
            var modellist = dblist[ModelName];
            return modellist;
        }

    }
}
