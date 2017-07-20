using DFM.BusinessLogic.Services;
using DFM.Entities.Bases;

namespace DFM.BusinessLogic
{
    public interface IConnector
    {
        BaseService<T>.IRepository Resolve<T>() where T : class, IEntity;
    }
}
