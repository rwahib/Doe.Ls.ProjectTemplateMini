using Doe.Ls.ProjectTemplate.Core.BL.DomainServices.FileService;

using Unity;


namespace Doe.Ls.ProjectTemplate.Core.BL
{
    public class HttpRepositoryFactory:RepositoryFactory
    {
        public override void RegisterAllDependencies()
        {
            base.RegisterAllDependencies();
            this.Container.RegisterType<IFileService, HttpFileService>();
            this.Container.RegisterType<IFileResolver, HttpFileResolver>();
        

        }
    }
}
