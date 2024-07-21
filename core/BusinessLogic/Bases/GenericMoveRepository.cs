using System;
using System.Linq;
using DFM.BusinessLogic.Repositories;
using DFM.BusinessLogic.Validators;
using DFM.Entities;
using DFM.Entities.Bases;

namespace DFM.BusinessLogic.Bases
{
	public abstract class GenericMoveRepository<T>(
		GenericMoveValidator<T> validator
	) : Repo<T>
		where T : class, IMove, new()
	{
		internal abstract User GetUser(T entity);

		protected void validate(T move)
		{
			var user = GetUser(move);
			validator.Validate(move, user);
		}

		#region Complete
		protected static void complete(T move)
		{
			if (move.Guid == Guid.Empty)
				move.Guid = Guid.NewGuid();

			var detailList = move.DetailList.Where(
				d => d.Guid == Guid.Empty
			);

			foreach (var detail in detailList)
			{
				detail.Guid = Guid.NewGuid();
			}

			adjustDetailList(move);
			adjustValue(move);
		}

		private static void adjustValue(T move)
		{
			if (move.DetailList.Any())
			{
				move.Value = move.DetailList.Sum(d => d.GetTotalValue());

				var detailList = move.DetailList
					.Where(d => d.Conversion != null)
					.ToList();

				move.Conversion =
					detailList.Any()
						? detailList.Sum(d => d.GetTotalConversion())
						: null;
			}
			else
			{
				if (move.Value < 0)
					move.Value = -move.Value;
				if (move.Conversion < 0)
					move.Conversion = -move.Conversion;
			}
		}

		private static void adjustDetailList(T move)
		{
			var wrongDetails = move.DetailList
				.Where(detail => detail.Value < 0);

			foreach (var detail in wrongDetails)
			{
				detail.Value = -detail.Value;
			}

			wrongDetails = move.DetailList
				.Where(detail => detail.Conversion < 0);

			foreach (var detail in wrongDetails)
			{
				detail.Conversion = -detail.Conversion;
			}
		}
		#endregion
	}
}
