using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Model.MemoryCache
{
    class CacheItem
    {
        public DateTime LastUsedTime { get; set; }
        public DateTime CachedTime { get; set; }
        public DateTime UpdateTime { get; set; }



        public IObjectProxy CachedOb { get; set; }
        public object Key { get; set; }

        public long UsedCount { get; set; }

        public CacheItem()
        {
            UsedCount = 0;

            CachedTime = DateTime.Now;
            LastUsedTime = DateTime.Now;
            UpdateTime = DateTime.Now;
        }
    }
}
