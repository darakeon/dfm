using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Net;
using DFM.Email.Exceptions;

namespace DFM.Email
{
    public class Sender
    {
        private static readonly String domain = ConfigurationManager.AppSettings["smtp-domain"];
        private static readonly String subdomain = ConfigurationManager.AppSettings["smtp-subdomain"];

        private static readonly String sender = ConfigurationManager.AppSettings["email-sender"];
        private static readonly String receiver = ConfigurationManager.AppSettings["email-receiver"];
        private static readonly String senderPassword = ConfigurationManager.AppSettings["email-receiver-pass"];


        public static readonly String SenderAddress = sender + "@" + domain;
        private static readonly String @default = receiver + "@" + domain;
        private static readonly String smtpAddress = subdomain + "." + domain;



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
            var emailSender = ConfigurationManager.AppSettings["EmailSender"];

            if (emailSender == "DontSend")
                return;

            var credentials = new NetworkCredential(SenderAddress, senderPassword);

            if (String.IsNullOrEmpty(subject))
                DFMEmailException.WithMessage(ExceptionPossibilities.InvalidSubject);

            if (String.IsNullOrEmpty(body))
                DFMEmailException.WithMessage(ExceptionPossibilities.InvalidBody);

            if (String.IsNullOrEmpty(to))
                DFMEmailException.WithMessage(ExceptionPossibilities.InvalidAddressee);

            var smtp = new SmtpClient(smtpAddress, 587)
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


            var attachments = files.Select(
                fileFullName => new Attachment(fileFullName));

            foreach (var attachment in attachments)
            {
                message.Attachments.Add(attachment);
            }
            

            try
            {
                smtp.Send(message);
            }
            catch (Exception e)
            {
                throw new DFMEmailException(e);
            }

        }


    }
}
