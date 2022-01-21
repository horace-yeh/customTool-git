using System;
using System.Runtime.Caching;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace CacheHelper
{
    public static class CacheHelper
    {

        /* Reference
         * https://blog.darkthread.net/blog/improved-getcachabledata
         * https://www.dotblogs.com.tw/wasichris/2015/11/14/153922
         */

        private static readonly ObjectCache _cache = MemoryCache.Default;
        private const string LockPrefix = "$$CacheLock#"; //加入Lock機制限定同一Key同一時間只有一個Callback執行

        /// <summary>
        /// 取得可以被Cache的資料(注意：非Thread-Safe)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">Cache保存號碼牌</param>
        /// <param name="callback">傳回查詢資料的函數</param>
        /// <param name="cacheSec">快取秒數</param>
        /// <returns></returns>
        public static T GetCacheData<T>(string key, Func<T> callback, int cacheSec = 60) where T : class
        {
            string cacheKey = key;
            //取得每個Key專屬的鎖定對象
            lock (GetLock(cacheKey))
            {
                T res = _cache[cacheKey] as T;
                if (res == null)
                {
                    res = callback();
                    _cache.Set(cacheKey, res, new CacheItemPolicy() { AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(cacheSec) });
                }
                return res;
            }
        }

        /// <summary>
        /// 取得可以被Cache的資料(注意：非Thread-Safe)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">Cache保存號碼牌</param>
        /// <param name="callback">傳回查詢資料的函數</param>
        /// <param name="timeSpan">滑動過期</param>
        /// <returns></returns>
        public static T GetCacheData<T>(string key, Func<T> callback, TimeSpan timeSpan) where T : class
        {
            string cacheKey = key;
            //取得每個Key專屬的鎖定對象
            lock (GetLock(cacheKey))
            {
                T res = _cache[cacheKey] as T;
                if (res == null)
                {
                    res = callback();
                    _cache.Set(cacheKey, res, new CacheItemPolicy() { SlidingExpiration = timeSpan });
                }
                return res;
            }
        }

        /// <summary>
        /// 取得可以被Cache的資料(注意：非Thread-Safe)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">Cache保存號碼牌</param>
        /// <param name="callback">傳回查詢資料的函數</param>
        /// <param name="cacheSec">快取秒數</param>
        /// <returns></returns>
        public static async Task<T> GetCacheData<T>(string key, Func<Task<T>> callback, int cacheSec = 60) where T : class
        {
            string cacheKey = key;
            //取得每個Key專屬的鎖定對象
            using (await GetAsyncLock(cacheKey).LockAsync())
            {
                T res = _cache[cacheKey] as T;
                if (res == null)
                {
                    res = await callback();
                    _cache.Set(cacheKey, res, new CacheItemPolicy() { AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(cacheSec) });
                }
                return res;
            }
        }

        /// <summary>
        /// 取得可以被Cache的資料(注意：非Thread-Safe)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">Cache保存號碼牌</param>
        /// <param name="callback">傳回查詢資料的函數</param>
        /// <param name="timeSpan">滑動過期</param>
        /// <returns></returns>
        public static async Task<T> GetCacheData<T>(string key, Func<Task<T>> callback, TimeSpan timeSpan) where T : class
        {
            string cacheKey = key;
            //取得每個Key專屬的鎖定對象
            using (await GetAsyncLock(cacheKey).LockAsync())
            {
                T res = _cache[cacheKey] as T;
                if (res == null)
                {
                    res = await callback();
                    _cache.Set(cacheKey, res, new CacheItemPolicy() { SlidingExpiration = timeSpan });
                }
                return res;
            }
        }

        /// <summary>
        /// 取得快取資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">Cache保存號碼牌</param>
        /// <returns></returns>
        public static T GetCache<T>(string key) where T : class
        {
            string cacheKey = key;
            //取得每個Key專屬的鎖定對象
            lock (GetLock(cacheKey))
            {
                return (T)_cache[key];
            }
        }

        /// <summary>
        /// 設定快取資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">Cache保存號碼牌</param>
        /// <param name="callback">傳回查詢資料的函數</param>
        /// <param name="cacheSec">快取秒數</param>
        public static void SetCache<T>(string key, Func<T> callback, int cacheSec = 60) where T : class
        {
            string cacheKey = key;
            //取得每個Key專屬的鎖定對象
            lock (GetLock(cacheKey))
            {
                T res = callback();
                _cache.Set(cacheKey, res, new CacheItemPolicy() { AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(cacheSec) });
            }
        }

        #region Method

        /// <summary>
        /// 取得每個Key專屬的鎖定對象
        /// </summary>
        /// <param name="key">Cache保存號碼牌</param>
        /// <returns></returns>
        private static object GetLock(string key)
        {
            //取得每個Key專屬的鎖定對象（object）
            string asyncLockKey = $"{LockPrefix}{key}";
            lock (_cache)
            {
                if (_cache[asyncLockKey] == null)
                {
                    _cache.Add(asyncLockKey, new object(), new CacheItemPolicy() { SlidingExpiration = new TimeSpan(0, 10, 0) });
                }
            }
            return _cache[asyncLockKey];
        }

        /// <summary>
        /// 取得每個Key專屬的鎖定對象(Async)
        /// </summary>
        /// <param name="key">Cache保存號碼牌</param>
        /// <returns></returns>
        private static AsyncLock GetAsyncLock(string key)
        {
            //取得每個Key專屬的鎖定對象（object）
            string asyncLockKey = $"{LockPrefix}-async-{key}";
            lock (_cache)
            {
                if (_cache[asyncLockKey] == null)
                {
                    // 使用 Nito.AsyncEx 套件的 AsyncLock 作為鎖定對象
                    _cache.Add(asyncLockKey, new AsyncLock(), new CacheItemPolicy() { SlidingExpiration = new TimeSpan(0, 10, 0) });
                }
            }
            return _cache[asyncLockKey] as AsyncLock;
        }

        #endregion Method

    }
}
