using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using DFM.Generic;
using CM = System.Configuration.ConfigurationManager;
using Smtp = System.Net.Configuration.SmtpSection;

namespace DFM.Email
{
    public class Sender
    {
        private readonly String from;
        private String to, subject, body;
        private readonly IList<String> files;
        private readonly String @default;
        
        public static String SenderAddress
        {
            get { return new Sender().from; }
        }
        

        public Sender()
        {
            files = new List<String>();

            var mailSettings = (Smtp)CM.GetSection("system.net/mailSettings/smtp");
            
            if (mailSettings != null)
                from = mailSettings.From;

            @default = Cfg.Email;
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
            var emailSender = Cfg.EmailSender;

            if (emailSender == "DontSend")
                return;

            if (emailSender == "MakeError")
                DFMEmailException.WithMessage(EmailStatus.EmailNotSent);

            if (String.IsNullOrEmpty(subject))
                DFMEmailException.WithMessage(EmailStatus.InvalidSubject);

            if (String.IsNullOrEmpty(body))
                DFMEmailException.WithMessage(EmailStatus.InvalidBody);

            if (String.IsNullOrEmpty(to))
                DFMEmailException.WithMessage(EmailStatus.InvalidAddress);

            using (var smtp = new SmtpClient {Timeout = 60000})
            {
                try
                {
                    var message =
                        new MailMessage(from, to, subject, body)
                        {IsBodyHtml = true};

                    var attachments = files.Select(
                        fileFullName => new Attachment(fileFullName));

                    foreach (var attachment in attachments)
                    {
                        message.Attachments.Add(attachment);
                    }

                    smtp.Send(message);
                }
                catch (Exception e)
                {
                    DFMEmailException.WithMessage(e);
                }
            }

        }


    }
}
