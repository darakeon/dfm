namespace DFM.BusinessLogic.Exceptions
{
    public enum ExceptionPossibilities
    {
        Unauthorized,

        FailOnEmailSend,
        TooLargeData,

        InvalidToken,
        InvalidUser,
        UserAlreadyExists,
        UserEmailInvalid,
        UserPasswordRequired,

        AccountNameRequired,
        AccountUrlRequired,
        AccountUrlInvalid,
        AccountAlreadyExists,
        AccountUrlAlreadyExists,
        CantCloseEmptyAccount,
        CantDeleteAccountWithMoves,
        ClosedAccount,
        DuplicatedAccountName,
        DuplicatedAccountUrl,
        RedLimitAboveYellowLimit,
        InvalidAccount,

        CategoryNameRequired,
        CategoryAlreadyExists,
        DisabledCategory,
        EnabledCategory,
        DuplicatedCategoryName,
        InvalidCategory,

        InMoveWrong,
        OutMoveWrong,
        TransferMoveWrong,
        MoveCircularTransfer,
        MoveDescriptionRequired,
        MoveDateRequired,
        MoveDateInvalid,
        MoveValueOrDetailRequired,
        InvalidMove,
        DetailWithoutMove,
        MoveDetailDescriptionRequired,
        MoveDetailAmountRequired,
        MoveDetailValueRequired,
        InvalidDetail,

        ScheduleRequired,
        ScheduleFrequencyNotRecognized,
        ScheduleWithNoMoves,
        ScheduleTimesCantBeZero,

        SummaryNatureNotFound,

        InvalidYear,
        InvalidMonth,
    }

}
