using System;
using DFM.BusinessLogic.Bases;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities;
using DFM.Entities.Bases;
using DFM.Entities.Extensions;

namespace DFM.BusinessLogic.Services
{
    internal class DetailService : BaseService<Detail>
    {
        internal DetailService(IRepository<Detail> repository) : base(repository) { }

        internal Detail SaveOrUpdate(Detail detail, BaseMove baseMove)
        {
            try
            {
                detail.SetMove(baseMove);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw DFMCoreException.WithMessage(ExceptionPossibilities.DetailWithoutMove);
            }

            return SaveOrUpdate(detail, validate);
        }


        private static void validate(Detail detail)
        {
            if (detail.Move == null && detail.FutureMove == null)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.DetailWithoutMove);
        }


        internal void SaveDetails(BaseMove baseMove)
        {
            foreach (var detail in baseMove.DetailList)
            {
                SaveOrUpdate(detail, baseMove);
            }
        }

    }
}
