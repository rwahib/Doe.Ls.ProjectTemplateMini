
using Unity;
using Unity.Resolution;

namespace Doe.Ls.EntityBase.RepositoryBase
{
    public interface IRepositoryFactory
    {
        UnityContainer Container { get; }
        void RegisterAllDependencies();
        T GetService<T>(params ResolverOverride[] overrides);
        T GetService<T>(string name, params ResolverOverride[] overrides);

        
       
    }
}