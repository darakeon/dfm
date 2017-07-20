using DFM.BusinessLogic.Bases;
using DFM.Entities;

namespace DFM.BusinessLogic.Services
{
    internal class FutureMoveService : BaseMoveService<FutureMove>
    {
        internal FutureMoveService(IRepository<FutureMove> repository) : base(repository) { }
    }
}
