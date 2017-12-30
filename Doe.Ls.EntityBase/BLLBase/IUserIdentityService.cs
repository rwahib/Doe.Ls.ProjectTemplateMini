using Doe.Ls.EntityBase.Models;

namespace Doe.Ls.EntityBase.BLLBase
{
    public interface IUserIdentityService : IDomainService
    {
        UserInfo GetUserByPortalId(string id);
        UserInfo GetUserByEmail(string email);
    }
}