using System;
using System.Linq;
using Ak.Generic.Exceptions;

namespace DFM.Email
{
    public class Error
    {
        /// <summary>
        /// Send a report e-mail with errors occured
        /// </summary>
        /// <param name="exceptions">Errors occured</param>
        /// <param name="url">Current url</param>
        /// <param name="user">Name of current user logged</param>
        /// <returns>Status of e-mail</returns>
        public static Status SendReport(Exception[] exceptions, String url, String user)
        {
            if (exceptions == null)
                return Status.Empty;

            try
            {
                var errors = String.Join("<br /><br />", exceptions.Select(format));
                var body = String.Format("<h4>{0} at {1}</h4>{2}", user, url, errors);

                new Sender()
                    .ToDefault()
                    .Subject(subject)
                    .Body(body)
                    .Send();

                return Status.Sent;
            }
            catch
            {
                return Status.Error;
            }
        }

        private static String format(Exception exception)
        {
            var realException = exception.MostInner();
            var stackTrace = realException.StackTrace
                .Replace("\n", "<br style='border-top: 1px solid #AAA' />");

            return String.Format(
                    @"<h3>{0}</h3>
                      <h2>{1}</h2>
                        <div style='background:#ffd;padding:20px 7px; white-space: nowrap;'>
                            {2}
                        </div>"
                        , realException.GetType()
                        , realException.Message
                        , stackTrace
                );
        }

        private static String subject
        {
            get { return DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss´fff"); }
        }



        public enum Status
        {
            Sent,
            Error,
            Empty
        }

    }
}
