using System;

namespace DFM.Email.Exceptions
{
    [Flags]
    public enum EmailStatus
    {
        EmailDisabled = 0,
        Ok = 1,
        InvalidSubject = 2,
        InvalidBody = 4,
        InvalidAddress = 8,
        EmailNotSent = 16,
    }

}
