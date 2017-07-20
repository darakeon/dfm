using DFM.BusinessLogic.Services;
using DFM.Entities.Bases;

namespace DFM.BusinessLogic
{
    public interface IResolver
    {
        BaseService<T>.IRepository Resolve<T>() where T : IEntity;
        void Register<T>() where T : class, IEntity;
    }
}
