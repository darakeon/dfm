using System;
using System.Linq;
using Keon.Util.DB;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using DFM.Entities.Extensions;
using Keon.NHibernate.Base;

namespace DFM.BusinessLogic.Bases
{
	public abstract class GenericMoveRepository<T> : BaseRepository<T>
		where T : class, IEntity, IMove, new()
	{
		#region Validate
		protected void Validate(
			T move,
			DateTime now,
			Int32 descriptionMaxSize,
			DfMError descriptionError,
			Boolean validateParents = true
		)
		{
			testDescription(move, descriptionMaxSize, descriptionError);
			testDate(move, now);
			testValue(move);
			testNature(move);

			if (validateParents)
			{
				testAccounts(move);
				testCategory(move);
			}
		}

		// ReSharper disable once UnusedParameter.Local
		private static void testDescription(
			T move,
			Int32 descriptionMaxSize,
			DfMError descriptionError
		)
		{
			if (String.IsNullOrEmpty(move.Description))
				throw DFMCoreException.WithMessage(DfMError.MoveDescriptionRequired);

			if (move.Description.Length > descriptionMaxSize)
				throw DFMCoreException.WithMessage(descriptionError);
		}

		private void testDate(T entity, DateTime now)
		{
			if (entity.Date == DateTime.MinValue)
				throw DFMCoreException.WithMessage(DfMError.MoveDateRequired);

			if (typeof(T) != typeof(Schedule) && entity.Date > now)
				throw DFMCoreException.WithMessage(DfMError.MoveDateInvalid);
		}

		private static void testValue(T move)
		{
			switch (move.ValueType())
			{
				case MoveValueType.Empty:
					throw DFMCoreException.WithMessage(
						DfMError.MoveValueOrDetailRequired
					);
				case MoveValueType.Both:
					throw DFMCoreException.WithMessage(
						DfMError.MoveValueAndDetailNotAllowed
					);
			}
		}

		private static void testNature(T move)
		{
			var hasIn = move.AccIn() != null;
			var hasOut = move.AccOut() != null;

			switch (move.Nature)
			{
				case MoveNature.In:
					if (!hasIn || hasOut)
						throw DFMCoreException.WithMessage(DfMError.InMoveWrong);
					break;

				case MoveNature.Out:
					if (hasIn || !hasOut)
						throw DFMCoreException.WithMessage(DfMError.OutMoveWrong);
					break;

				case MoveNature.Transfer:
					if (!hasIn || !hasOut)
						throw DFMCoreException.WithMessage(DfMError.TransferMoveWrong);
					break;

			}
		}

		private static void testAccounts(T move)
		{
			var moveInClosed = move.AccIn() != null && !move.AccIn().IsOpen();
			var moveOutClosed = move.AccOut() != null && !move.AccOut().IsOpen();

			if (moveInClosed || moveOutClosed)
				throw DFMCoreException.WithMessage(DfMError.ClosedAccount);

			if (move.AccIn() != null && move.AccOut() != null && move.AccIn().ID == move.AccOut().ID)
				throw DFMCoreException.WithMessage(DfMError.MoveCircularTransfer);
		}

		private void testCategory(T move)
		{
			if (GetUser(move).Config.UseCategories)
			{
				if (move.Category == null)
					throw DFMCoreException.WithMessage(DfMError.InvalidCategory);

				if (!move.Category.Active)
					throw DFMCoreException.WithMessage(DfMError.DisabledCategory);
			}
			else
			{
				if (move.Category != null)
					throw DFMCoreException.WithMessage(DfMError.CategoriesDisabled);
			}
		}

		internal abstract User GetUser(T entity);
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
