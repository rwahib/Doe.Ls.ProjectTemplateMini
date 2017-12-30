using System;
using System.Collections.Generic;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.Settings;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Core.BL.UI
    {
    public static class AppCacheHelper
        {
        private static Dictionary<string, object> _cached = null;
        private static Dictionary<string, object> Cached
            {
            get
                {
                if(_cached == null) return (_cached = new Dictionary<string, object>());
                return _cached;
                }
            }

        private static string _token = null;
        public static string Token
        {
            get
            {
                if (ProjectTemplateSettings.Site.IsTestSite) return string.Empty;
                _token = null;
                if (string.IsNullOrEmpty(_token)) _token = Guid.NewGuid().ToString().Substring(0, 5);
                return _token;
            }
        }

        public static void Cache<T>(string key, T result)
            {
            Cached[key] = result;
            }

        public static T GetResult<T>(string key)
            {
            if(_cached == null) return default(T);

            if(Cached.ContainsKey(key))
                {
                var result = (T)Cached[key];
                return result;
                }

            return default(T);
            }

        public static bool HasKey(string key)
            {
            if(_cached == null) return false;
            return Cached.ContainsKey(key);
            }

        public static void Expire()
            {
            if(_cached != null)
                {
                _cached.Clear();
                _cached = null;
                }
            }

        public static void Expire(Enums.CacheRegion cacheRegion)
        {
            if (HasKey(cacheRegion.ToString()))
            {
                
                Cached.Remove(cacheRegion.ToString());
            }
           }
        public static class PositionCache
        {
            public static void Cache(List<Position> result)
            {
                
                AppCacheHelper.Cache("PositionsList", result);
            }
          
        }


    }
    }
