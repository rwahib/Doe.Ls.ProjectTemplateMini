using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models;


namespace Doe.Ls.ProjectTemplate.Core.BL.UI.Caching
    {
    public static class AppCacheHelper
        {
        private static ObjectCache Cached { get; } = MemoryCache.Default;

        private static string _token = null;
        public static string Token
            {
            get
                {
                if(string.IsNullOrEmpty(_token)) _token = Guid.NewGuid().ToString().Substring(0, 5);
                return _token;
                }
            }

      /*   /// <summary>
        /// Add to cache
        /// </summary>
        /// <param name="key"></param>
        /// <param name="objectValue"></param>
        /// <param name="region"></param>
        /// <param name="minutes"></param>
        public static void Cache(string key, object objectValue, Enums.CacheRegion region, int minutes = 30)
            {
            var policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30) };
            var keyWithRegion = key.GetKeyWithRegion(region);
            Cached.Add(keyWithRegion, objectValue, policy);
            }
        /// <summary>
        /// remove from cache
        /// </summary>
        /// <param name="key"></param>
       public static void Expire(string key, Enums.CacheRegion region)
            {
            Cached.Remove(key.GetKeyWithRegion(region));
            }
        public static T Get<T>(string key, Enums.CacheRegion region) where T : class
            {
            var item = Cached.GetCacheItem(key.GetKeyWithRegion(region));
            return item?.Value as T;

            }

        public static bool HasKey(string key, Enums.CacheRegion region)
            {
            return Cached.Contains(key.GetKeyWithRegion(region));
            }

        public static void ExpireAll(Enums.CacheRegion region)
            {
            var keys = Cached.Select(item => item.Key.GetKeyWithRegion(region)).ToList();
            keys.ForEach(k => Cached.Remove(k));
            }
*/
       
        
        }
    }
