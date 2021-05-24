using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities;
using DFM.Entities.Bases;
using DFM.Entities.Enums;

namespace DFM.BusinessLogic.Repositories
{
	internal class DetailRepository : Repo<Detail>
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
				if (detail.Guid == Guid.Empty)
					detail.Guid = Guid.NewGuid();

				SaveOrUpdate(detail, validate);
			}
		}

		private static void validate(Detail detail)
		{
			if (detail.Move == null && detail.Schedule == null)
				throw Error.DetailWithoutParent.Throw();

			if (String.IsNullOrEmpty(detail.Description))
				throw Error.MoveDetailDescriptionRequired.Throw();

			if (detail.Description.Length > MaxLen.DetailDescription)
				throw Error.TooLargeDetailDescription.Throw();

			if (detail.Amount == 0)
				throw Error.MoveDetailAmountRequired.Throw();

			if (detail.ValueCents == 0)
				throw Error.MoveDetailValueRequired.Throw();
		}

		public IList<Detail> ByDescription(String userEmail, params String[] terms)
		{
			return byDescription(userEmail, PrimalMoveNature.In, terms)
				.Union(byDescription(userEmail, PrimalMoveNature.Out, terms))
				.ToList();
		}

		private IList<Detail> byDescription(String userEmail, PrimalMoveNature nature, params String[] terms)
		{
			var userRelation = getUserRelation(nature);

			var query = NewQuery()
				.Where(d => d.Move != null)
				.Where(userRelation, User.Compare(userEmail));

			terms.ToList().ForEach(
				term => query = query.Like(d => d.Description, term)
			);

			return query.List;
		}

		private static Expression<Func<Detail, User>> getUserRelation(PrimalMoveNature nature)
		{
			switch (nature)
			{
				case PrimalMoveNature.In:
					return d => d.Move.In.User;
				case PrimalMoveNature.Out:
					return d => d.Move.Out.User;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}
