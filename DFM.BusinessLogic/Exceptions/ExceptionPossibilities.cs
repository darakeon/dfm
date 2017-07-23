namespace DFM.BusinessLogic.Exceptions
{
    public enum ExceptionPossibilities
    {
        Unauthorized = 1,

        FailOnEmailSend = 101,
        TooLargeData = 102,

        InvalidToken = 201,
        InvalidUser = 202,
        UserAlreadyExists = 203,
        UserEmailInvalid = 204,
        UserPasswordRequired = 205,
        DisabledUser = 206,

        AccountNameRequired = 301,
        AccountUrlRequired = 302,
        AccountUrlInvalid = 303,
        AccountAlreadyExists = 304,
        AccountUrlAlreadyExists = 305,
        CantCloseEmptyAccount = 306,
        CantDeleteAccountWithMoves = 307,
        ClosedAccount = 308,
        DuplicatedAccountName = 309,
        DuplicatedAccountUrl = 310,
        RedLimitAboveYellowLimit = 311,
        InvalidAccount = 312,

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
        DetailWithoutMove = 510,
        MoveDetailDescriptionRequired = 511,
        MoveDetailAmountRequired = 512,
        MoveDetailValueRequired = 513,
        InvalidDetail = 514,

        ScheduleRequired = 601,
        ScheduleFrequencyNotRecognized = 602,
        ScheduleWithNoMoves = 603,
        ScheduleTimesCantBeZero = 604,

        SummaryNatureNotFound = 701,

        InvalidYear = 801,
        InvalidMonth = 802,
    }

}
