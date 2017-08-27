using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using DFM.Email.Exceptions;
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

            @default = CM.AppSettings["email"];
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
            var emailSender = CM.AppSettings["EmailSender"];

            if (emailSender == "DontSend")
                return;

            if (String.IsNullOrEmpty(subject))
                DFMEmailException.WithMessage(ExceptionPossibilities.InvalidSubject);

            if (String.IsNullOrEmpty(body))
                DFMEmailException.WithMessage(ExceptionPossibilities.InvalidBody);

            if (String.IsNullOrEmpty(to))
                DFMEmailException.WithMessage(ExceptionPossibilities.InvalidAddressee);

            var all = CM.OpenExeConfiguration(ConfigurationUserLevel.None);
            var net = (Smtp)all.GetSection("system.net/mailSettings/smtp");
            var host = net.Network.Host;

            using (var smtp = new SmtpClient(host) {Timeout = 10000})
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
                    throw new DFMEmailException(e);
                }
            }

        }


    }
}
