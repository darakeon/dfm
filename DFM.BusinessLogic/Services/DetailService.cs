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

        internal void SaveDetails(BaseMove baseMove)
        {
            foreach (var detail in baseMove.DetailList)
            {
                saveOrUpdate(detail, baseMove);
            }
        }

        private void saveOrUpdate(Detail detail, BaseMove baseMove)
        {
            try
            {
                detail.SetMove(baseMove);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw DFMCoreException.WithMessage(ExceptionPossibilities.DetailWithoutMove);
            }

            SaveOrUpdate(detail, validate);
        }


        private static void validate(Detail detail)
        {
            if (detail.Move == null && detail.FutureMove == null)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.DetailWithoutMove);

            if (String.IsNullOrEmpty(detail.Description))
                throw DFMCoreException.WithMessage(ExceptionPossibilities.MoveDetailDescriptionRequired);

            if (detail.Amount == 0)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.MoveDetailAmountRequired);

            if (detail.Value == 0)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.MoveDetailValueRequired);
        }



    }
}
