using System;
using System.Web.Mvc;

namespace Doe.Ls.ProjectTemplate.Core.BL.UI
{
    public static class ViewDataWrapper
    {
        public static T GetValue<T>(string key, ViewDataDictionary vd)
        {
            if (!vd.Keys.Contains(key))
            {
                return default(T);
            }
            return (T) vd[key];
        }
    }

    public static class TempViewDataWrapper
    {
        public static T GetValue<T>(string key, TempDataDictionary td)
        {
            if(!td.Keys.Contains(key))
                {
                return default(T);
                }

            return (T)td[key];
        }
    }
}