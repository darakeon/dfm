using Castle.MicroKernel.Registration;
using Castle.Windsor;
using DFM.BusinessLogic;
using DFM.BusinessLogic.Services;
using DFM.Entities.Bases;

namespace DFM.Core
{
    public class Resolver : IResolver
    {
        public Resolver()
        {
            container = new WindsorContainer();
        }

        private static WindsorContainer container;


        public BaseService<T>.IRepository Resolve<T>() where T : IEntity
        {
            return container.Resolve<BaseService<T>.IRepository>();
        }

        public void Register<T>() where T : class, IEntity
        {
            container.Register(Component.For<BaseService<T>.IRepository>()
                                        .ImplementedBy<BaseData<T>>()
                                        .LifeStyle.Singleton);
        }

    }
}
