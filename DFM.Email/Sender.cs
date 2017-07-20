using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net;

namespace DFM.Email
{
    public class Sender
    {
        private const String domain = "dontflymoney.com";

        public const String SenderAddress = "no-reply@" + domain;
        private const String @default = "[some default sender here]";


        private String to, subject, body;
        private readonly IList<String> files;


        public Sender()
        {
            files = new List<String>();
        }


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

        public Sender Attach(String fileFullName)
        {
            files.Add(fileFullName);
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


            foreach (var fileFullName in files)
            {
                var attachment = new Attachment(fileFullName);
                message.Attachments.Add(attachment);
            }
            

            try
            {
                smtp.Send(message);
            }
            catch (Exception e)
            {
                throw new SendException(e);
            }

        }



        public class SendException : Exception
        {
            public SendException(Exception e) 
                : base("Exception on sending e-mail", e) { }
        }


    }
}
