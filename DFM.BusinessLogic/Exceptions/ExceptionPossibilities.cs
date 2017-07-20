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
        WrongUserEmail,

        AccountAlreadyExists,
        CantCloseEmptyAccount,
        CantDeleteAccountWithMoves,
        ClosedAccount,
        DuplicatedAccountName,
        RedLimitAboveYellowLimit,

        CategoryAlreadyExists,
        DisabledCategory,
        InvalidCategory,
        DuplicatedCategoryName,

        InMoveWrong,
        OutMoveWrong,
        TransferMoveWrong,
        MoveCircularTransfer,

        DetailRequired,
        DetailWithoutMove,

        ScheduleRequired,
        ScheduleFrequencyNotRecognized,
        ScheduleWithNoMoves,

        SummaryNatureNotFound,
    }

}
