using System;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Helpers;
using DFM.Entities;
using DFM.Entities.Bases;
using Keon.NHibernate.Base;

namespace DFM.BusinessLogic.Repositories
{
	internal class DetailRepository : BaseRepository<Detail>
	{
		internal void SaveDetails(Move move, Move oldMove)
		{
			foreach (var detail in move.DetailList)
			{
				detail.Move = move;
			}

			saveDetails(move);

			//TODO: DELETE ORPHAN MANUALLY, MAKE NH DO IT - RIGHT
			if (oldMove != null)
			{
				foreach (var detail in oldMove.DetailList)
				{
					if (move.DetailList.All(d => d.ID != detail.ID))
					{
						Delete(detail.ID);
					}
				}
			}
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
				throw DFMCoreException.WithMessage(DfMError.DetailWithoutParent);

			if (String.IsNullOrEmpty(detail.Description))
				throw DFMCoreException.WithMessage(DfMError.MoveDetailDescriptionRequired);

			if (detail.Description.Length > MaxLen.Detail_Description)
				throw DFMCoreException.WithMessage(DfMError.TooLargeDetailDescription);

			if (detail.Amount == 0)
				throw DFMCoreException.WithMessage(DfMError.MoveDetailAmountRequired);

			if (detail.ValueCents == 0)
				throw DFMCoreException.WithMessage(DfMError.MoveDetailValueRequired);
		}
	}
}
