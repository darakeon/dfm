using DFM.BusinessLogic;
using DFM.BusinessLogic.Services;
using DFM.Entities.Bases;

namespace DFM.Repositories
{
    public class Connector : IConnector
    {
        public BaseService<T>.IRepository Resolve<T>() where T : class, IEntity
        {
            return new BaseData<T>();
        }

    }
}
