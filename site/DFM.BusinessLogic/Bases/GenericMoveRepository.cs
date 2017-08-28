using System;
using System.Linq;
using DK.Generic.DB;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Repositories;
using DFM.Entities;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using DFM.Entities.Extensions;

namespace DFM.BusinessLogic.Bases
{
	public class GenericMoveRepository<T> : BaseRepository<T>
		where T : class, IEntity, IMove, new()
	{
		#region Validate
		protected static void Validate(T move, Boolean validateParents = true)
		{
			testDescription(move);
			testDate(move);
			testValue(move);
			testNature(move);

			if (validateParents)
			{
				testAccounts(move);
				testCategory(move);
			}
		}

		// ReSharper disable once UnusedParameter.Local
		private static void testDescription(T move)
		{
			if (String.IsNullOrEmpty(move.Description))
				throw DFMCoreException.WithMessage(ExceptionPossibilities.MoveDescriptionRequired);
		}

		private static void testDate(T move)
		{
			if (move.Date == DateTime.MinValue)
				throw DFMCoreException.WithMessage(ExceptionPossibilities.MoveDateRequired);

			var now = move.User?.Now() ?? DateTime.UtcNow;

			if (typeof(T) != typeof(Schedule) && move.Date > now)
				throw DFMCoreException.WithMessage(ExceptionPossibilities.MoveDateInvalid);
		}

		private static void testValue(T move)
		{
			if (!move.HasValue())
				throw DFMCoreException.WithMessage(ExceptionPossibilities.MoveValueOrDetailRequired);
		}

		private static void testNature(T move)
		{
			var hasIn = move.AccIn() != null;
			var hasOut = move.AccOut() != null;

			switch (move.Nature)
			{
				case MoveNature.In:
					if (!hasIn || hasOut)
						throw DFMCoreException.WithMessage(ExceptionPossibilities.InMoveWrong);
					break;

				case MoveNature.Out:
					if (hasIn || !hasOut)
						throw DFMCoreException.WithMessage(ExceptionPossibilities.OutMoveWrong);
					break;

				case MoveNature.Transfer:
					if (!hasIn || !hasOut)
						throw DFMCoreException.WithMessage(ExceptionPossibilities.TransferMoveWrong);
					break;

			}
		}

		private static void testAccounts(T move)
		{
			var moveInClosed = move.AccIn() != null && !move.AccIn().IsOpen();
			var moveOutClosed = move.AccOut() != null && !move.AccOut().IsOpen();

			if (moveInClosed || moveOutClosed)
				throw DFMCoreException.WithMessage(ExceptionPossibilities.ClosedAccount);

			if (move.AccIn() != null && move.AccOut() != null && move.AccIn().ID == move.AccOut().ID)
				throw DFMCoreException.WithMessage(ExceptionPossibilities.MoveCircularTransfer);
		}

		private static void testCategory(T move)
		{
			if (move.User.Config.UseCategories)
			{
				if (move.Category == null)
					throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidCategory);

				if (!move.Category.Active)
					throw DFMCoreException.WithMessage(ExceptionPossibilities.DisabledCategory);
			}
			else
			{
				if (move.Category != null)
					throw DFMCoreException.WithMessage(ExceptionPossibilities.CategoriesDisabled);
			}
		}
		#endregion

		#region Complete
		protected static void Complete(T move)
		{
			adjustValue(move);
			adjustDetailList(move);
		}

		private static void adjustValue(T move)
		{
			if (move.Value == 0)
				move.Value = null;
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
