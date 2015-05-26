using System;
using System.Runtime.Caching;

namespace Com.Meten.WeChatSmeten.Helper
{
    public class MemoryCacheHelper
    {
        static readonly ObjectCache cache = MemoryCache.Default;

        public static object AddCache<T>(string key, object value)
        {
            return AddCache<T>(key,value, new TimeSpan(0, 0, 30, 0));
        }

        public static object AddCache<T>(string key, object value, TimeSpan ts)
        {
            if (key == null) throw new ArgumentNullException("key");
            if (value == null) throw new ArgumentNullException("value");

            if (!cache.Contains(key))
            {
                cache.Add(key, value, DateTime.Now.Add(ts));
                return (T)cache[key];
            }
            else
            {
                return null;
            }
        }

        public static Object GetExistsCache<T>(string key)
        {
            if (key == null) throw new ArgumentNullException("key");

            if (cache.Contains(key))
            {
                return (T)cache[key];
            }
            else
            {
                return  null;
            }
        }

        public static void RemoveCacheByKey(string key)
        {
            if (key == null) throw new ArgumentNullException("key");
            if (cache.Contains(key))
            {
                cache.Remove(key);
            }
            return;
        }
    }
}