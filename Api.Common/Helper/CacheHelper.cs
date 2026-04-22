using Microsoft.Extensions.Caching.Memory;
using System;

namespace Api.Common.Helper
{
    public class CacheHelper
    {
        static readonly MemoryCache Cache = new MemoryCache(new MemoryCacheOptions());
        /// <summary>
        /// 获取缓存中的值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public static object GetCacheValue(string key)
        {
            if (!string.IsNullOrEmpty(key) && Cache.TryGetValue(key, out var val))
            {
                return val;
            }
            return default(object);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public static void SetCacheValue(string key, object value, int Time, CacheType cacheType)
        {
            if (!string.IsNullOrEmpty(key))
            {
                switch (cacheType)
                {
                    case CacheType.Hours:
                        Cache.Set(key, value, new MemoryCacheEntryOptions
                        {
                            SlidingExpiration = TimeSpan.FromHours(Time)
                        });
                        break;
                    case CacheType.Minutes:
                        Cache.Set(key, value, new MemoryCacheEntryOptions
                        {
                            SlidingExpiration = TimeSpan.FromMinutes(Time)
                        });
                        break;
                    case CacheType.Seconds:
                        Cache.Set(key, value, new MemoryCacheEntryOptions
                        {
                            SlidingExpiration = TimeSpan.FromSeconds(Time)
                        });
                        break;
                    default:
                        break;
                }
            }
        }

        public enum CacheType
        {
            Hours,
            Minutes,
            Seconds
        }
    }
}
