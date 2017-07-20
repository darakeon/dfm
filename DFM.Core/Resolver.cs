using DFM.BusinessLogic;
using DFM.BusinessLogic.Services;
using DFM.Entities.Bases;

namespace DFM.Core
{
    public class Resolver : IResolver
    {
        public BaseService<T>.IRepository Resolve<T>() where T : class, IEntity
        {
            return new BaseData<T>();
        }

    }
}
