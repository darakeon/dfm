using DFM.BusinessLogic;
using DFM.BusinessLogic.Bases;
using DFM.Entities.Bases;

namespace DFM.Repositories
{
    public class Connector : IConnector
    {
        public IData<T> Resolve<T>() where T : class, IEntity
        {
            return new BaseData<T>();
        }

        public ITransactionController GetTransactionController()
        {
            return new TransactionController();
        }

    }
}
