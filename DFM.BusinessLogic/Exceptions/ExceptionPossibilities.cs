namespace DFM.BusinessLogic.Exceptions
{
    public enum ExceptionPossibilities
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
        RedLimitAboveYellowLimit,
        ScheduleFrequencyNotRecognized,
        SummaryNatureNotFound,
        TooLargeData,
        TransferMoveWrong,
        UserAlreadyExists,
        UserInvalidEmail,
        WrongUserEmail,
        DetailWithoutMove,
    }

}
