using System;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using DFM.Entities.Extensions;

namespace DFM.BusinessLogic.Validators;

public class GenericMoveValidator<T>(
	Int32 descriptionMaxSize,
	Error descriptionError
)
	where T : class, IMove
{
	private Int32 descriptionMaxSize { get; } = descriptionMaxSize;
	private Error descriptionError { get; } = descriptionError;

	public void Validate(T move, User user)
	{
		testAccounts(move);
		testCategory(user, move);

		testDescription(move);
		testDate(move, user.Now());

		testValue(move);
		testNature(move);

		testCurrency(
			move,
			user.Settings.UseCurrency,
			move.Out?.Currency,
			move.In?.Currency
		);
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
		var moveInClosed = move.In is { Open: false };
		var moveOutClosed = move.Out is { Open: false };

		if (moveInClosed || moveOutClosed)
			throw Error.ClosedAccount.Throw();

		if (move.In != null && move.Out != null && move.In.ID == move.Out.ID)
			throw Error.CircularTransfer.Throw();
	}

	private void testCategory(User user, T move)
	{
		if (user.Settings.UseCategories)
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

	private void testCurrency(T move, Boolean useCurrency, Currency? currencyOut, Currency? currencyIn)
	{
		var isTransfer = move.Nature == MoveNature.Transfer;
		var sameCurrency = currencyIn == currencyOut;

		var moveHasConversion =
			move.Conversion != null
			&& move.Conversion != 0;

		var detailHaveAnyConversion =
			move.DetailList.Any(
				d => d.Conversion != null
				     && move.Conversion != 0
			);

		var detailHaveAllConversion =
			move.DetailList.Any()
			&& move.DetailList.All(
				d => d.Conversion != null
				     && d.Conversion != 0
			);

		if (moveHasConversion || detailHaveAnyConversion)
		{
			if (!useCurrency)
				throw Error.UseCurrencyDisabled.Throw();

			if (!isTransfer)
				throw Error.CurrencyInOutValueWithoutTransfer.Throw();

			if (sameCurrency)
				throw Error.AccountsSameCurrencyConversion.Throw();
		}

		if (
			isTransfer
			&& !sameCurrency
			&& !moveHasConversion
			&& !detailHaveAllConversion
		)
		{
			throw Error.AccountsDifferentCurrencyNoConversion.Throw();
		}
	}
}
