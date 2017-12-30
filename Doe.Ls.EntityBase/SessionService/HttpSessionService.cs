
#region

using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Doe.Ls.EntityBase.Models;

#endregion

namespace Doe.Ls.EntityBase.SessionService
{
    /// <summary>
    ///   Summary description for SessionHelper
    /// </summary>
    public class HttpSessionService : ISessionService
    {
        private readonly Hashtable _cached = new Hashtable();

        // this object will be injected via Unit test

        public ControllerContext ControllerContext { get; set; }


        public bool Expired
        {
            get
            {
                if (ReadFromSession(Cnt.CurrentUserKey) == null)
                {
                    Abandon();
                    return true;
                }


                return false;
            }
        }


        public void AddToSession(string key, object value)
        {
            if (HttpContext.Current == null)
            {
                _cached[key] = value;

                return;
            }



            if (ControllerContext != null && ControllerContext.HttpContext != null &&
                ControllerContext.HttpContext.Session != null)
            {
                if (ControllerContext.HttpContext.Session[key] == null)
                {
                    ControllerContext.HttpContext.Session.Add(key, value);
                }
                else
                {
                    ControllerContext.HttpContext.Session[key] = value;
                }

            }
            else
            {
                if (HttpContext.Current.Session[key] == null)
                {
                    HttpContext.Current.Session.Add(key, value);
                }
                else
                {
                    HttpContext.Current.Session[key] = value;
                }
            }
        }

        public object ReadFromSession(string key)
        {
            if (HttpContext.Current == null || HttpContext.Current.Session == null)
            {
                if (_cached.ContainsKey(key)) return _cached[key];
                else return null;

            }


            if (ControllerContext != null && ControllerContext.HttpContext != null &&
                ControllerContext.HttpContext.Session != null)
            {
                if (ControllerContext.HttpContext.Session[key] == null)
                {
                    return null;
                }
                else
                {
                    return ControllerContext.HttpContext.Session[key];
                }

            }
            else
            {
                if (HttpContext.Current.Session[key] == null)
                {
                    return null;
                }
                else
                {
                    return HttpContext.Current.Session[key];
                }
            }


        }

        public T ReadFromSession<T>(string key) where T : class
        {
            if (HttpContext.Current == null || HttpContext.Current.Session == null)
            {
                if (_cached.ContainsKey(key)) return _cached[key] as T;
                else return null;

            }


            if (ControllerContext != null && ControllerContext.HttpContext != null &&
                ControllerContext.HttpContext.Session != null)
            {
                if (ControllerContext.HttpContext.Session[key] == null)
                {
                    return null;
                }
                else
                {
                    return ControllerContext.HttpContext.Session[key] as T;
                }

            }
            else
            {
                if (HttpContext.Current.Session[key] == null)
                {
                    return null;
                }
                else
                {
                    return HttpContext.Current.Session[key] as T;
                }
            }


        }
        public bool Exists(string key)
        {
            if (HttpContext.Current == null)
            {
                return _cached.ContainsKey(key);
            }

            return HttpContext.Current.Session[key] != null;
        }

        public UserInfo GetCurrentUser()
        {
            var userInfo = ReadFromSession(Cnt.CurrentUserKey) as UserInfo;
            if(userInfo==null)return new UserInfo();
            return ReadFromSession(Cnt.CurrentUserKey) as UserInfo;
        }

        public string GetCurrentUserId()
        {
            if (HttpContext.Current == null || HttpContext.Current.User == null) return string.Empty;
            return HttpContext.Current.User.Identity.Name;
        }

        public void Abandon()
        {
            if (HttpContext.Current == null)
            {
                _cached.Clear();

                return;
            }

            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                var keys = (from object key in HttpContext.Current.Session.Keys select key.ToString()).ToList();
                try
                {
                    foreach (string key in keys)
                    {
                        HttpContext.Current.Session[key] = null;
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    Debug.WriteLine(e.ToString());
                }
                HttpContext.Current.Session.RemoveAll();
                HttpContext.Current.Session.Abandon();
            }

        }
    }
}