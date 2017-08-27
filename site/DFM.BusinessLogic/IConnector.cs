using DFM.BusinessLogic.Bases;
using DFM.Entities.Bases;

namespace DFM.BusinessLogic
{
    public interface IConnector
    {
        IData<T> Resolve<T>() where T : class, IEntity;
        ITransactionController GetTransactionController();

    }

}
