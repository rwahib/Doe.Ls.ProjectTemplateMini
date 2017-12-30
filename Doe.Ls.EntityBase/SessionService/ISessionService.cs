using System.Web.Mvc;
using Doe.Ls.EntityBase.Models;

namespace Doe.Ls.EntityBase.SessionService
{
    public interface ISessionService
    {
        ControllerContext ControllerContext { get; set; }
        bool Expired { get; }
        void AddToSession(string key, object value);
        object ReadFromSession(string key);
        T ReadFromSession<T>(string key) where T:class;
        bool Exists(string key);
        UserInfo GetCurrentUser();        
        void Abandon();
        
        
    }
}