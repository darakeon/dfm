using System;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities;
using DFM.Entities.Bases;

namespace DFM.BusinessLogic.Repositories
{
    internal class DetailRepository : BaseRepository<Detail>
    {
        internal void SaveDetails(Move move)
        {
            foreach (var detail in move.DetailList)
            {
                detail.Move = move;
            }

            saveDetails(move);
        }

        internal void SaveDetails(Schedule schedule)
        {
            foreach (var detail in schedule.DetailList)
            {
                detail.Schedule = schedule;
            }

            saveDetails(schedule);
        }

        private void saveDetails(IMove move)
        {
            foreach (var detail in move.DetailList)
            {
                SaveOrUpdate(detail, validate);
            }
        }


        private static void validate(Detail detail)
        {
            if (detail.Move == null && detail.Schedule == null)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.DetailWithoutParent);

            if (String.IsNullOrEmpty(detail.Description))
                throw DFMCoreException.WithMessage(ExceptionPossibilities.MoveDetailDescriptionRequired);

            if (detail.Amount == 0)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.MoveDetailAmountRequired);

            if (detail.Value == 0)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.MoveDetailValueRequired);
        }



    }
}
