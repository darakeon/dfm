namespace DFM.Core.Exceptions
{
    internal enum ExceptionPossibilities
    {
        AccountAlreadyExists,
        CantCloseEmptyAccount,
        CantDeleteAccountWithMoves,
        CategoryAlreadyExists,
        ClosedAccount,
        ConnectionError,
        DetailRequired,
        DisabledCategory,
        DuplicatedAccountName,
        FailOnEmailSend,
        InMoveWrong,
        InvalidCategory,
        InvalidToken,
        InvalidUser,
        MoveCircularTransfer,
        MoveFutureNotScheduled,
        OutMoveWrong,
        ScheduleFrequencyNotRecognized,
        SummaryNatureNotFound,
        TooLargeData,
        TransferMoveWrong,
        UserAlreadyExists,
        UserInvalidEmail,
        WrongUserEmail,
        YellowLimitUnderRedLimit,
    }

}
