namespace DFM.BusinessLogic.Exceptions
{
	public enum Error
	{
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

		AccountNameRequired = 301,
		AccountUrlRequired = 302,
		AccountUrlInvalid = 303,
		AccountNameAlreadyExists = 304,
		AccountUrlAlreadyExists = 305,
		CantCloseEmptyAccount = 306,
		CantDeleteAccountWithMoves = 307,
		ClosedAccount = 308,
		DuplicatedAccountName = 309,
		DuplicatedAccountUrl = 310,
		RedLimitAboveYellowLimit = 311,
		InvalidAccount = 312,
		CantDeleteAccountWithSchedules = 313,
		TooLargeAccountName = 314,
		TooLargeAccountUrl = 315,

		CategoryNameRequired = 401,
		CategoryAlreadyExists = 402,
		DisabledCategory = 403,
		EnabledCategory = 404,
		DuplicatedCategoryName = 405,
		InvalidCategory = 406,
		CategoriesDisabled = 407,
		TooLargeCategoryName = 408,

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
		public static CoreError Throw(this Error error)
		{
			return new CoreError(error);
		}
	}
}
