using System.Collections.Generic;
using Doe.Ls.EntityBase.BLLBase;
using Doe.Ls.EntityBase.Logging;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.EntityBase.RepositoryBase;
using Doe.Ls.EntityBase.SessionService;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;


namespace Doe.Ls.ProjectTemplate.Core.Test.Mockups
{
    public class MockUserIdentityService : IUserIdentityService
    {
        public List<string> MessageList { get; set; }
        public MockUserIdentityService()
        {
            MessageList = new List<string>();

        }
        public void Dispose()
        {
            MessageList.Add("Dispose called");
        }

        [Unity.Attributes.Dependency]
        public ILoggerService LoggerService { get; set; }

        [Unity.Attributes.Dependency]
        public IRepositoryFactory RepositoryFactory { get; set; }

        [Unity.Attributes.Dependency]
        public ISessionService SessionService { get; set; }

        public UserInfo GetUserByPortalId(string id)
        {
            MessageList.Add("GetUserByPortalId called:" + id);
            return new UserInfoExtension
            {
                UserName = id,
                Title = id,
                Email = $"{id}@det.nsw.edu.au"
            };
        }

        public UserInfo GetUserByEmail(string email)
        {
            MessageList.Add("GetUserByEmail called:" + email);
            return new UserInfo
            {
                UserName = email,                
                Email = email
            };
        }
    }
}