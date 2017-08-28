namespace DFM.BusinessLogic.Exceptions
{
	public enum ExceptionPossibilities
	{
		Unauthorized = 0,

		FailOnEmailSend = 101,
		TooLargeData = 102,

		InvalidToken = 201,
		InvalidUser = 202,
		UserAlreadyExists = 203,
		UserEmailInvalid = 204,
		UserPasswordRequired = 205,
		DisabledUser = 206,
		Uninvited = 207,
		RetypeWrong = 208,
		WrongPassword = 209,

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
		CategoriesDisabled = 313,
		MoveCheckDisabled = 314,

		CategoryNameRequired = 401,
		CategoryAlreadyExists = 402,
		DisabledCategory = 403,
		EnabledCategory = 404,
		DuplicatedCategoryName = 405,
		InvalidCategory = 406,

		InMoveWrong = 501,
		OutMoveWrong = 502,
		TransferMoveWrong = 503,
		MoveCircularTransfer = 504,
		MoveDescriptionRequired = 505,
		MoveDateRequired = 506,
		MoveDateInvalid = 507,
		MoveValueOrDetailRequired = 508,
		InvalidMove = 509,
		DetailWithoutParent = 510,
		MoveDetailDescriptionRequired = 511,
		MoveDetailAmountRequired = 512,
		MoveDetailValueRequired = 513,
		InvalidDetail = 514,
		MoveAlreadyChecked = 515,
		MoveAlreadyUnchecked = 516,

		ScheduleRequired = 601,
		ScheduleFrequencyNotRecognized = 602,
		//ScheduleWithNoMoves = 603,
		ScheduleTimesCantBeZero = 604,
		InvalidSchedule = 605,
		DisabledSchedule = 606,

		SummaryNatureNotFound = 701,

		InvalidYear = 801,
		InvalidMonth = 802,

		LanguageUnknown = 901,
		TimezoneUnknown = 902,
	}

}
