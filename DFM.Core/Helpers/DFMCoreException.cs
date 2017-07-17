using System;
using Ak.Generic.Enums;
using DFM.Core.Database.Base;

namespace DFM.Core.Helpers
{
    public class DFMCoreException : Exception
    {
        internal static DFMCoreException WithMessage(Possibilities message)
        {
            return new DFMCoreException(message);
        }

        internal static DFMCoreException WithMessage(String format, params object[] args)
        {
            var errorMessage = String.Format(format, args);
            var errorEnumValue = Str2Enum.Cast<Possibilities>(errorMessage);

            return WithMessage(errorEnumValue);
        }


        private DFMCoreException(Possibilities message)
            : base(message.ToString())
        {
            NHManager.Error();
        }


        internal enum Possibilities
        {
            AccountAlreadyExists,
            CantCloseEmptyAccount,
            CantDeleteAccountWithMoves,
            ClosedAccount,
            ConnectionError,
            DetailRequired,
            DisabledCategory,
            DuplicatedAccountName,
            InMoveWrong,
            InvalidUser,
            MonthAmbiguousInYear,
            MoveCircularTransfer,
            MoveFutureNotScheduled,
            OutMoveWrong,
            ScheduleFrequencyNotRecognized,
            SummaryNatureNotFound,
            TransferMoveWrong,
            UserAlreadyExists,
            UserInvalidEmail,
            YearAmbiguousInAccount,
            ScheduleNoMoves,
        }

    }
}