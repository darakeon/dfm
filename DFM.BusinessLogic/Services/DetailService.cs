using DFM.Entities;

namespace DFM.BusinessLogic.Services
{
    public class DetailService : BaseService<Detail>
    {
        internal DetailService(DataAccess father, IRepository repository) : base(father, repository) { }

        public Detail SaveOrUpdate(Detail detail)
        {
            return base.SaveOrUpdate(detail);
        }

    }
}
