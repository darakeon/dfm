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
        InvalidUser,
        MonthAmbiguousInYear,
        MoveCircularTransfer,
        MoveFutureNotScheduled,
        OutMoveWrong,
        ScheduleFrequencyNotRecognized,
        ScheduleNoMoves,
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
