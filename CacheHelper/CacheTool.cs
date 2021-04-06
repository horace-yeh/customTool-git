using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace CacheHelper
{
    public class CacheTool
    {
        //Ref : https://www.dotblogs.com.tw/wasichris/2015/11/14/153922
        //Ref : https://blog.miniasp.com/post/2008/01/14/Correct-using-Cache-in-ASPNET

        private static ObjectCache _cache = MemoryCache.Default;

        public object GetCache(string key)
        {
            return _cache[key];
        }

        public void SetCache(string key, object data, int cacheSec = 60)
        {
            var policy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(cacheSec)
            };
            _cache.Set(key, data, policy);
        }
    }
}
