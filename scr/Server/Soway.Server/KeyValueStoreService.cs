using Soway.Service.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soway.Service
{
     static  class KeyValueStoreService
    {
        private static System.Collections.Concurrent.ConcurrentDictionary<string,User.CacheInfo>
            sessionStores = new System.Collections.Concurrent.ConcurrentDictionary<string, CacheInfo>();
        public static void Store(string guid, CacheInfo info)
        {
           while(false == sessionStores.TryAdd(guid, info))
            {
                ;
            }
        }
        public static CacheInfo Get(string guid)
        {
            if (sessionStores.ContainsKey(guid))
            {
                return sessionStores[guid];
            }
            else
                return null;
        }
        public static  void Remove(string guid)
        {
            CacheInfo info = null; ;

            while (false == sessionStores.TryRemove(guid, out info)) ;
        }


    }
}
