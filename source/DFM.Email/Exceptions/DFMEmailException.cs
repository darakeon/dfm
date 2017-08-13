using System;

namespace DFM.Email.Exceptions
{
    public class DFMEmailException : Exception
    {
        public static Int32 ErrorCounter { get; private set; }
        public ExceptionPossibilities Type { get; private set; }

        public static DFMEmailException WithMessage(ExceptionPossibilities type)
        {
            throw new DFMEmailException(type);
        }


        private DFMEmailException(ExceptionPossibilities type)
            : base(type.ToString())
        {
            ErrorCounter++;
            Type = type;
        }

        public DFMEmailException(Exception e)
            : base("Exception on sending e-mail", e) { }
    }



}
