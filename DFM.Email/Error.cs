using System;
using System.Linq;
using Ak.Generic.Exceptions;

namespace DFM.Email
{
    public class Error
    {
        public static Boolean Report(Exception[] exceptions)
        {
            try
            {
                var body = String.Join("<br />",
                        exceptions.Select(format)
                    );


                new Sender()
                    .ToDefault()
                    .Subject(DateTime.Now.ToString())
                    .Body(body)
                    .Send();

                return true;
            }
            catch
            {
                return false;
            }
        }

        private static String format(Exception exception)
        {
            var realException = exception.MostInner();

            return String.Format(
                    "{0}: {1}<br />{2}", realException.GetType(), realException.Message, realException.StackTrace
                );
        }


    }
}
