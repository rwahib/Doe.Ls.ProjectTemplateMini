using System.Web;
using Doe.Ls.EntityBase.BLLBase;
using Doe.Ls.EntityBase.Logging;
using Doe.Ls.EntityBase.RepositoryBase;
using Doe.Ls.EntityBase.SessionService;
using Doe.Ls.ProjectTemplate.Core.BL.DomainServices;
using Doe.Ls.ProjectTemplate.Core.BL.DomainServices.FileService;
using Doe.Ls.ProjectTemplate.Core.BL.DomainServices.Logging;
using Doe.Ls.ProjectTemplate.Core.BL.DomainServices.PdfService;
using Doe.Ls.TrimBase;
using Unity;

namespace Doe.Ls.ProjectTemplate.Core.BL
{
    public abstract class RepositoryFactoryApplicationBase : RepositoryFactoryBase
    {
        protected RepositoryFactoryApplicationBase(UnityContainer container)
            : base(container)
        {
        }

        protected RepositoryFactoryApplicationBase()
        {
        }

        public override void RegisterAllDependencies()
        {

            # region Domain services registration

            Container.RegisterType<ILoggerService, DecLsLoggerService>();
            Container.RegisterType<IEmailService, EmailService>();
            Container.RegisterType<IUserIdentityService, DecLsUserIdentityService>();
            Container.RegisterType<ILoginService, LoginService>();
            Container.RegisterType<IFileService, FileSystemService>();
            //  Container.RegisterType<IPdfService, PdfService>();
            Container.RegisterType<IPdfService, PdfXsltTransformer>();
            Container.RegisterType<ITrimService, TrimService>();

            #region Session helper

            ISessionService sessionService;
            if (HttpContext.Current == null)
            {
                sessionService = new CachedSessionService();
            }
            else
            {
                sessionService = new HttpSessionService();
            }

            #endregion

            Container.RegisterInstance(sessionService);

            #endregion

            #region Entity services registration

            var unitOfWork = new UnitOfWork();
            unitOfWork.DbContext.Configuration.LazyLoadingEnabled = false;
            Container.RegisterInstance<IUnitOfWork>(unitOfWork);

            RegisterEntityRepositories();

            #endregion

            // Registering the domain service
            var factory = this;
            Container.RegisterInstance<IRepositoryFactory>(factory);
        }

        public abstract void RegisterEntityRepositories();
    }
}
