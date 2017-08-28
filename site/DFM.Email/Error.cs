using System;
using System.Collections.Generic;
using System.Linq;
using DK.Generic.Exceptions;

namespace DFM.Email
{
    public class Error
    {
	    /// <summary>
	    /// Send a report e-mail with errors occured
	    /// </summary>
	    /// <param name="exceptions">Errors occured</param>
	    /// <param name="url">Current url</param>
	    /// <param name="urlReferrer">Previous url</param>
	    /// <param name="parameters">Parameters of url (post / get)</param>
	    /// <param name="user">Name of current user logged</param>
	    /// <returns>Status of e-mail</returns>
		public static Status SendReport(Exception[] exceptions, String url, String urlReferrer, IDictionary<String, String> parameters, String user)
        {
            if (exceptions == null)
                return Status.Empty;

            try
            {
                var parametersFormatted = String.Join("; ", parameters.Select(format));
                var exceptionsFormatted = String.Join("<br /><br />", exceptions.Select(format));

	            if (!String.IsNullOrEmpty(urlReferrer))
		            urlReferrer = "origin: " + urlReferrer;

                var body = $@"
					<h4>{user} at {url}</h4>
					<h5>{parametersFormatted}</h5>
					<h6>{urlReferrer}</h6>
					{exceptionsFormatted}";

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

        private static String format(KeyValuePair<String, String> pair)
        {
            return $"{pair.Key}: {pair.Value}";
        }

		private static String format(Exception exception)
        {
            var realException = exception.MostInner();
            var stackTrace = realException.StackTrace
                .Replace("\n", "<br style='border-top: 1px solid #AAA' />");

            return $@"<h3>{realException.GetType()}</h3>
                      <h2>{realException.Message}</h2>
					  <div style='background:#ffd;padding:20px 7px; white-space: nowrap;'>
						  {stackTrace}
					  </div>";
        }

        private static String subject => DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss´fff");


	    public enum Status
        {
            Sent = 0,
            Error = 1,
            Empty = 2,
        }

    }
}
