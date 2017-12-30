using Doe.Ls.EntityBase.BLLBase;
using Doe.Ls.EntityBase.Logging;
using Doe.Ls.EntityBase.RepositoryBase;
using Doe.Ls.EntityBase.SessionService;
using Doe.Ls.ProjectTemplate.Core.BL.DomainServices;
using Doe.Ls.ProjectTemplate.Core.BL.DomainServices.FileService;
using Doe.Ls.ProjectTemplate.Core.BL.DomainServices.PdfService;

using System;
using Doe.Ls.TrimBase;
using Unity.Resolution;

namespace Doe.Ls.ProjectTemplate.Core.BL
{
    public partial class ServiceRepository
    {
        public ITrimService TrimService(params ResolverOverride[] overrides) {
            return _repositoryFactory.GetService<ITrimService>(overrides);
            }

        public IEmailService EmailService(params ResolverOverride[] overrides)
        {
            return _repositoryFactory.GetService<IEmailService>(overrides);
        }

        public ILoggerService LoggerService(params ResolverOverride[] overrides)
        {
            return _repositoryFactory.GetService<ILoggerService>(overrides);
        }

        public ISessionService SessionService(params ResolverOverride[] overrides)
        {
            return _repositoryFactory.GetService<ISessionService>(overrides);
        }

        public IUserIdentityService UserIdentityService(params ResolverOverride[] overrides)
        {
            return _repositoryFactory.GetService<IUserIdentityService>(overrides);
        }

        public LoginService LoginService(params ResolverOverride[] overrides)
        {
            return _repositoryFactory.GetService<ILoginService>(overrides) as LoginService;
        }

        public IFileService FileService(params ResolverOverride[] overrides)
        {
            return _repositoryFactory.GetService<IFileService>(overrides);
        }

        public IPdfService PdfService(params ResolverOverride[] overrides)
        {
            return _repositoryFactory.GetService<IPdfService>(overrides);
        }

        public void CleanUnitTestData(string unitTestData)
        {
            var ctx = this.GetUnitOfWork().DbContext as Data.SampleProjectTemplateEntities;
            ctx.udp_cleanUnitTestData(unitTestData);

        }

     
        public void LogLinqSQL(System.Action<string> action)
            {
            var ctx = this.GetUnitOfWork().DbContext as Data.SampleProjectTemplateEntities;
            ctx.Database.Log = action;

            }
        public void LogLinqSQLToConsole()
            {
            var ctx = this.GetUnitOfWork().DbContext as Data.SampleProjectTemplateEntities;
            ctx.Database.Log = Console.WriteLine;

            }
        }
}