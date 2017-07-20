using DFM.BusinessLogic.Bases;
using DFM.Entities;

namespace DFM.BusinessLogic.Services
{
    internal class DetailService : BaseService<Detail>
    {
        internal DetailService(IRepository<Detail> repository) : base(repository) { }

        internal Detail SaveOrUpdate(Detail detail)
        {
            return base.SaveOrUpdate(detail);
        }

    }
}
