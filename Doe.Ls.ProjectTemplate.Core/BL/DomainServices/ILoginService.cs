using Doe.Ls.EntityBase.BLLBase;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;

namespace Doe.Ls.ProjectTemplate.Core.BL.DomainServices
{
    public interface ILoginService:IDomainService
    {
        UserInfoExtension GetUser(string userName);
        UserInfoExtension GetUserAndCacheIt(string userName);
        UserInfoExtension GetCachedUserInfo();
        UserInfoExtension GetUserAndCacheItInDb(string userName);
        bool ValidDoEUser(string userName);
        }
}
