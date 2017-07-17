namespace DFM.Core.Exceptions
{
    internal enum ExceptionPossibilities
    {
        AccountAlreadyExists,
        CantCloseEmptyAccount,
        CantDeleteAccountWithMoves,
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
        MonthAmbiguousInYear,
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
        YearAmbiguousInAccount,
        YellowLimitUnderRedLimit,
    }

}
