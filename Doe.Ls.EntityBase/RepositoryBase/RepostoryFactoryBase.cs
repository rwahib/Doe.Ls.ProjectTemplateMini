using System.Dynamic;

using Unity;
using Unity.Registration;
using Unity.Resolution;

// ReSharper disable once RedundantUsingDirective

namespace Doe.Ls.EntityBase.RepositoryBase {
	public abstract class RepositoryFactoryBase : IRepositoryFactory {
		
        public UnityContainer Container { get; private set; }
		public abstract void RegisterAllDependencies();

		protected RepositoryFactoryBase(UnityContainer container) {
			Container = container;
		}


		protected RepositoryFactoryBase() {
			var container = new UnityContainer();
			Container = container;
		}


		public T GetService<T>(params ResolverOverride[] overrides) {

			var obj = Container.Resolve<T>(overrides);
			this.Container.BuildUp(obj);
			return obj;
		}

        

		/// <summary>
		/// Gets the service especially if it is registered by instance
		/// </summary>		
		/// <param name="name">instance name</param>
		/// <param name="overrides">The overrides.</param>
		/// <returns></returns>
		public T GetService<T>(string name, params ResolverOverride[] overrides) {
			var obj = Container.Resolve<T>(name, overrides);
			this.Container.BuildUp(obj);
			return obj;
		}

		public IUnityContainer RegisterType<TFrom, TTo>(string name = null, params InjectionMember[] injectionMembers)
			where TTo : TFrom {
			if (string.IsNullOrWhiteSpace(name)) {
				return Container.RegisterType<TFrom, TTo>(injectionMembers);
			} else {
				return Container.RegisterType<TFrom, TTo>(name, injectionMembers);

			}
		}

		public IUnityContainer RegisterInstance<TTo>(string name, TTo instance) {
			return Container.RegisterInstance(name, instance);

		}
        public IUnityContainer RegisterInstance<TTo>(TTo instance)
            {
            return Container.RegisterInstance(instance);

            }


        }
}
