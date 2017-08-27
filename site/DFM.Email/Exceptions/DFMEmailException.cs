using System;

namespace DFM.Email.Exceptions
{
    public class DFMEmailException : Exception
    {
        public static Int32 ErrorCounter { get; private set; }
        public EmailStatus Type { get; private set; }

        public static DFMEmailException WithMessage(EmailStatus type)
        {
            throw new DFMEmailException(type);
        }

        public static DFMEmailException WithMessage(Exception exception)
        {
            throw new DFMEmailException(exception);
        }


        private DFMEmailException(EmailStatus type)
            : base(type.ToString())
        {
            ErrorCounter++;
            Type = type;
        }

        public DFMEmailException(Exception e)
            : base("Exception on sending e-mail", e)
        {
            ErrorCounter++;
            Type = EmailStatus.EmailNotSent;
        }
    }



}
