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
        DetailWithoutMove,
        MoveDetailDescriptionRequired,
        MoveDetailAmountRequired,
        MoveDetailValueRequired,

        ScheduleRequired,
        ScheduleFrequencyNotRecognized,
        ScheduleWithNoMoves,

        SummaryNatureNotFound,
    }

}
