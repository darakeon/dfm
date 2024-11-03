using System.Collections.Generic;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities;
using DFM.Entities.Bases;
using DFM.Entities.Enums;

namespace DFM.BusinessLogic.Validators;

public class ScheduleValidator() : GenericMoveValidator<Schedule>(
	MaxLen.ScheduleDescription,
	Error.TooLargeScheduleDescription
)
{
	private static readonly IDictionary<Error, ScheduleStatus> errorToStatus =
		new Dictionary<Error, ScheduleStatus>
		{
			{ Error.NotSignedLastContract, ScheduleStatus.UserNoSignContract },
			{ Error.DisabledUser, ScheduleStatus.UserInactive },
			{ Error.UserDeleted, ScheduleStatus.UserMarkedDelete },
			{ Error.UserAskedWipe, ScheduleStatus.UserRequestedWipe },
			{ Error.ClosedAccount, ScheduleStatus.AccountClosed },
			{ Error.DisabledCategory, ScheduleStatus.CategoryDisabled },
			{ Error.InvalidCategory, ScheduleStatus.CategoryInvalid },
			{ Error.CategoriesDisabled, ScheduleStatus.CategoriesDisabled },
			{ Error.AccountsSameCurrencyConversion, ScheduleStatus.CurrencyChange },
			{ Error.AccountsDifferentCurrencyNoConversion, ScheduleStatus.CurrencyChange },
			{ Error.PlanLimitMoveByAccountByMonthAchieved, ScheduleStatus.MoveOutOfLimit },
		};

	public ScheduleStatus Convert(Error error)
	{
		return !errorToStatus.ContainsKey(error)
			? ScheduleStatus.Error
			: errorToStatus[error];
	}
}
