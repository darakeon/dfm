﻿using System;

namespace DFM.BusinessLogic.Exceptions
{
	public enum Error
	{
		Unknown = 0,

		Uninvited = 207,
		NotSignedLastContract = 42,

		FailOnEmailSend = 101,

		InvalidToken = 201,
		InvalidUser = 202,
		UserAlreadyExists = 203,
		UserEmailInvalid = 204,
		UserPasswordRequired = 205,
		DisabledUser = 206,
		RetypeWrong = 208,
		WrongPassword = 209,
		InvalidTheme = 210,
		TFAEmptySecret = 211,
		TFAWrongCode = 212,
		TFAWrongPassword = 213,
		TFANotConfigured = 214,
		TFANotVerified = 215,
		TooLargeUserEmail = 216,
		UserEmailRequired = 217,
		UserDeleted = 218,
		UserAskedWipe = 219,

		AccountNameRequired = 301,
		AccountNameAlreadyExists = 302,
		CantCloseEmptyAccount = 303,
		CantDeleteAccountWithMoves = 304,
		ClosedAccount = 305,
		DuplicatedAccountName = 306,
		RedLimitAboveYellowLimit = 307,
		InvalidAccount = 308,
		CantDeleteAccountWithSchedules = 309,
		TooLargeAccountName = 310,
		OpenedAccount = 311,

		CategoryNameRequired = 401,
		CategoryAlreadyExists = 402,
		DisabledCategory = 403,
		EnabledCategory = 404,
		DuplicatedCategoryName = 405,
		InvalidCategory = 406,
		CategoriesDisabled = 407,
		TooLargeCategoryName = 408,
		CategoryUnifyFail = 409,
		CannotMergeSameCategory = 410,

		InMoveWrong = 501,
		OutMoveWrong = 502,
		TransferMoveWrong = 503,
		CircularTransfer = 504,
		MoveDescriptionRequired = 505,
		MoveDateRequired = 506,
		MoveDateInvalid = 507,
		MoveValueOrDetailRequired = 508,
		InvalidMove = 509,
		DetailWithoutParent = 510,
		MoveDetailDescriptionRequired = 511,
		MoveDetailAmountRequired = 512,
		MoveDetailValueRequired = 513,
		MoveAlreadyChecked = 514,
		MoveAlreadyUnchecked = 515,
		MoveCheckDisabled = 516,
		TooLargeMoveDescription = 517,
		TooLargeDetailDescription = 518,
		MoveValueAndDetailNotAllowed = 519,
		MoveCheckWrongNature = 520,

		ScheduleRequired = 601,
		ScheduleTimesCantBeZero = 602,
		InvalidSchedule = 603,
		DisabledSchedule = 604,
		TooLargeScheduleDescription = 605,

		InvalidYear = 701,
		InvalidMonth = 702,

		LanguageUnknown = 801,
		TimeZoneUnknown = 802,
	}

	public static class ErrorX
	{
		public static CoreError Throw(this Error error, Exception inner = null)
		{
			return new(error, inner);
		}
	}
}
