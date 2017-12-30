using Doe.Ls.EntityBase.BLLBase;
using Doe.Ls.EntityBase.Logging;
using Doe.Ls.EntityBase.RepositoryBase;
using Doe.Ls.EntityBase.SessionService;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.BL.DomainServices.FileService;
using Doe.Ls.ProjectTemplate.Core.BL.DomainServices.PdfService;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;
using Doe.Ls.TrimBase;
using Unity;

namespace Doe.Ls.ProjectTemplate.Core.Test.Mockups
{
    public class MockRepositoryFactory:RepositoryFactory
    {
        protected const string UnitTestToken = "@UNIT_TEST";
        public override void RegisterAllDependencies()
        {
            base.RegisterAllDependencies();
            Container.RegisterInstance<IUnitOfWork>(new UnitOfWorkMockup());

            Container.RegisterType<IUnitOfWork,UnitOfWorkMockup>("new-instance");
            Container.RegisterType<ILoggerService, MockoggerService>();
            Container.RegisterType<IEmailService, MockEmailService>();
            Container.RegisterType<IUserIdentityService, MockUserIdentityService>();
            Container.RegisterType<IFileResolver, MockupFileResolver>();
           

            var session = new CachedSessionService();

            var unitTestUser=new UserInfoExtension
            {
                UserName = Enums.Cnt.System,
                DepartmentId = Enums.Cnt.Na.ToString(),
                DepartmentName = "NA",
                DisplayName =$"Display name {UnitTestToken}",
                Email = "unit-test-user@det.nsw.edu.au",
                FirstName = $"FirstName name {UnitTestToken}",
                SurName = $"Lastname name {UnitTestToken}",
                
            };
        
            
             session.AddCurrentUserToSession(unitTestUser);

            Container.RegisterInstance<ISessionService>(session);
            
        }
    }
}
