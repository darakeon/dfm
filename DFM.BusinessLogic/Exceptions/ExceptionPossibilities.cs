namespace DFM.BusinessLogic.Exceptions
{
    public enum ExceptionPossibilities
    {
        FailOnEmailSend,
        TooLargeData,

        InvalidToken,
        InvalidUser,
        UserAlreadyExists,
        UserEmailInvalid,
        UserPasswordRequired,

        AccountNameRequired,
        AccountAlreadyExists,
        CantCloseEmptyAccount,
        CantDeleteAccountWithMoves,
        ClosedAccount,
        DuplicatedAccountName,
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

        Unauthorized,
    }

}
