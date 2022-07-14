using System;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Repositories;
using DFM.Entities;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using DFM.Entities.Extensions;

namespace DFM.BusinessLogic.Bases
{
	public abstract class GenericMoveRepository<T> : Repo<T>
		where T : class, IMove, new()
	{
		protected abstract Int32 descriptionMaxSize { get; }
		protected abstract Error descriptionError { get; }

		#region Validate
		protected void validate(T move, DateTime now)
		{
			testDescription(move);
			testDate(move, now);

			testValue(move);
			testNature(move);

			testAccounts(move);
			testCategory(move);
		}

		private void testDescription(T move)
		{
			if (String.IsNullOrEmpty(move.Description))
				throw Error.MoveDescriptionRequired.Throw();

			if (move.Description.Length > descriptionMaxSize)
				throw descriptionError.Throw();
		}

		private void testDate(T entity, DateTime now)
		{
			if (entity.GetDate() == DateTime.MinValue)
				throw Error.MoveDateRequired.Throw();

			if (typeof(T) != typeof(Schedule) && entity.GetDate() > now)
				throw Error.MoveDateInvalid.Throw();
		}

		private static void testValue(T move)
		{
			switch (move.ValueType())
			{
				case MoveValueType.Empty:
					throw Error.MoveValueOrDetailRequired.Throw();
				case MoveValueType.Both:
					throw Error.MoveValueAndDetailNotAllowed.Throw();
				case MoveValueType.Unique:
				case MoveValueType.Detailed:
					break;
				default:
					throw new NotImplementedException();
			}
		}

		private static void testNature(T move)
		{
			var hasIn = move.In != null;
			var hasOut = move.Out != null;

			switch (move.Nature)
			{
				case MoveNature.In:
					if (!hasIn || hasOut)
						throw Error.InMoveWrong.Throw();
					break;

				case MoveNature.Out:
					if (hasIn || !hasOut)
						throw Error.OutMoveWrong.Throw();
					break;

				case MoveNature.Transfer:
					if (!hasIn || !hasOut)
						throw Error.TransferMoveWrong.Throw();
					break;

				default:
					throw new NotImplementedException();
			}
		}

		private static void testAccounts(T move)
		{
			var moveInClosed = move.In is {Open: false};
			var moveOutClosed = move.Out is {Open: false};

			if (moveInClosed || moveOutClosed)
				throw Error.ClosedAccount.Throw();

			if (move.In != null && move.Out != null && move.In.ID == move.Out.ID)
				throw Error.CircularTransfer.Throw();
		}

		private void testCategory(T move)
		{
			if (GetUser(move).Settings.UseCategories)
			{
				if (move.Category == null)
					throw Error.InvalidCategory.Throw();

				if (!move.Category.Active)
					throw Error.DisabledCategory.Throw();
			}
			else
			{
				if (move.Category != null)
					throw Error.CategoriesDisabled.Throw();
			}
		}

		internal abstract User GetUser(T entity);
		#endregion

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
				move.Value = move.DetailList.Sum(d => d.GetTotal());
			else if (move.Value < 0)
				move.Value = -move.Value;
		}

		private static void adjustDetailList(T move)
		{
			var wrongDetails = move.DetailList
				.Where(detail => detail.Value < 0);

			foreach (var detail in wrongDetails)
			{
				detail.Value = -detail.Value;
			}
		}
		#endregion
	}
}
