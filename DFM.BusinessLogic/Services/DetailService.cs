using System;
using DFM.BusinessLogic.Bases;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities;

namespace DFM.BusinessLogic.Services
{
    internal class DetailService : BaseService<Detail>
    {
        internal DetailService(IRepository<Detail> repository) : base(repository) { }

        internal void SaveDetails(Move move)
        {
            foreach (var detail in move.DetailList)
            {
                saveOrUpdate(detail, move);
            }
        }

        private void saveOrUpdate(Detail detail, Move move)
        {
            detail.Move = move;

            SaveOrUpdate(detail, validate);
        }


        private static void validate(Detail detail)
        {
            if (detail.Move == null)
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
