using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace DFM.Core.Helpers
{
    public class EmailSender
    {

        internal static void Send()
        {
            //var objEmail = new MailMessage
            //                   {
            //                       IsBodyHtml = true,
            //                       Priority = MailPriority.Normal,
            //                       Subject = "Assunto",
            //                       Body = "Conteúdo do email. Se ativar html, pode utilizar cores, fontes, etc.",
            //                       SubjectEncoding = Encoding.UTF8,
            //                       BodyEncoding = Encoding.UTF8,
            //                   };

            //var objSmtp = new SmtpClient
            //                  {
            //                      Host = "smtp.gmail.com",
            //                  };

            //objSmtp.Send(objEmail);

            var ss = new SmtpClient("smtp.gmail.com", 587)
                         {
                             EnableSsl = true,
                             Timeout = 10000,
                             DeliveryMethod = SmtpDeliveryMethod.Network,
                             UseDefaultCredentials = false,
                         };

            var mm = new MailMessage("no-reply@dontflymoney.com", "destination@dontflymoney.com", "Assunto", "Corpo")
                         {
                             BodyEncoding = Encoding.UTF8,
                             DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure
                         };
            ss.Send(mm);


        }
    }
}
