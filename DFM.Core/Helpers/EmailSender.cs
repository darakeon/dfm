using System;
using System.Net.Mail;
using System.Net;
using DFM.Core.Exceptions;

namespace DFM.Core.Helpers
{
    public class EmailSender
    {
        private const String domain = "dontflymoney.com";

        public static String Sender = "no-reply@" + domain;



        private String to, subject, body;

        public EmailSender To(String email)
        {
            to = email;
            return this;
        }

        public EmailSender Subject(String text)
        {
            subject = text;
            return this;
        }

        public EmailSender Body(String html)
        {
            body = html;
            return this;
        }



        internal void Send()
        {
            var credentials = new NetworkCredential("no-reply@" + domain, "[some-awful-password]");

            var smtp = new SmtpClient("smtp." + domain, 587)
                            {
                                EnableSsl = false,
                                Timeout = 10000,
                                DeliveryMethod = SmtpDeliveryMethod.Network,
                                UseDefaultCredentials = false,
                                Credentials = credentials,
                            };

            var message = new MailMessage(Sender, to, subject, body)
                            {
                                IsBodyHtml = true
                            };

            try
            {
                smtp.Send(message);
            }
            catch (Exception e)
            {
                throw DFMCoreException.WithMessage(ExceptionPossibilities.FailOnEmailSend);
            }

        }

    }
}
