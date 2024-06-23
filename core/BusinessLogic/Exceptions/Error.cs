using System;

namespace DFM.BusinessLogic.Exceptions
{
	public enum Error
	{
		Unknown = 0,

		UpdateApp = 27,

		Uninvited = 207,
		LoginRequested = 224,
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
		WipeInvalid = 220,
		WipeUserAsked = 221,
		WipeNoMoves = 222,
		CSVNotFound = 223,
		TermsNotFound = 225,

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
		AccountNotFound = 312,
		UseAccountsSignsDisabled = 313,
		UseCurrencyDisabled = 314,

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
		MoveNatureRequired = 524,
		MoveNatureInvalid = 525,
		MoveValueOrDetailRequired = 508,
		MoveValueInvalid = 508,
		MoveNotFound = 509,
		DetailWithoutParent = 510,
		MoveDetailDescriptionRequired = 511,
		MoveDetailAmountRequired = 512,
		MoveDetailAmountInvalid = 512,
		MoveDetailValueRequired = 527,
		MoveDetailValueInvalid = 526,
		MoveDetailConversionInvalid = 528,
		MoveAlreadyChecked = 514,
		MoveAlreadyUnchecked = 515,
		MoveCheckDisabled = 516,
		TooLargeMoveDescription = 517,
		TooLargeDetailDescription = 518,
		MoveValueAndDetailNotAllowed = 519,
		MoveCheckWrongNature = 520,
		AccountsSameCurrencyConversion = 521,
		AccountsDifferentCurrencyNoConversion = 522,
		CurrencyInOutValueWithoutTransfer = 523,

		ScheduleRequired = 601,
		ScheduleTimesCantBeZero = 602,
		InvalidSchedule = 603,
		DisabledSchedule = 604,
		TooLargeScheduleDescription = 605,

		InvalidYear = 701,
		InvalidMonth = 702,

		LanguageUnknown = 801,
		TimeZoneUnknown = 802,

		LogNotFound = 900,

		InvalidArchive = 1000,
		InvalidArchiveSize = 1002,
		InvalidArchiveLines = 1003,
		InvalidArchiveColumn = 1001,
	}

	public static class ErrorX
	{
		public static CoreError Throw(this Error error, Exception inner = null)
		{
			return new(error, inner);
		}
	}
}
