using System;

namespace DFM.Email
{
    [Flags]
    public enum EmailStatus
    {
        EmailDisabled = 0,
        EmailSent = 1,
        InvalidSubject = 2,
        InvalidBody = 4,
        InvalidAddress = 8,
        EmailNotSent = 16,
    }

}
