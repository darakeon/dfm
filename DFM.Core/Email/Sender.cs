using System;
using System.Net.Mail;
using System.Net;
using DFM.Core.Exceptions;

namespace DFM.Core.Email
{
    public class Sender
    {
        private const String domain = "dontflymoney.com";

        public const String SenderAddress = "no-reply@" + domain;
        private const String @default = "[some default sender here]";


        private String to, subject, body;

        public Sender To(String email)
        {
            to = email;
            return this;
        }

        public Sender ToDefault()
        {
            to = @default;
            return this;
        }

        public Sender Subject(String text)
        {
            subject = text;
            return this;
        }

        public Sender Body(String html)
        {
            body = html;
            return this;
        }


        public void Send()
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

            var message = new MailMessage(SenderAddress, to, subject, body)
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
