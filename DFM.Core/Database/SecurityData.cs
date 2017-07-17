using System;
using DFM.Core.Database.Base;
using DFM.Core.Entities;
using DFM.Core.Enums;
using DFM.Core.Exceptions;
using DFM.Core.Helpers;

namespace DFM.Core.Database
{
    public class SecurityData : BaseData<Security>
    {
        private SecurityData() { }

        public static void CreateAndSend(String email, SecurityAction action, String subject, String layout)
        {
            var user = UserData.SelectByEmail(email);

            if (user == null)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.WrongUserEmail);

            CreateAndSend(user, action, subject, layout);
        }

        internal static void CreateAndSend(User user, SecurityAction action, String subject, String layout)
        {
            var security = new Security { Action = action, User = user, Subject = subject, Layout = layout };

            security = SaveOrUpdate(security);

            sendEmail(security);
        }

        internal static Security SaveOrUpdate(Security security)
        {
            return SaveOrUpdate(security, complete, validate);
        }



        private static void complete(Security security)
        {
            security.Active = true;
            security.Expire = DateTime.Now.AddDays(30);
            security.Guid = Guid.NewGuid().ToString();
        }



        private static void validate(Security security)
        {
            var currentUser = UserData.SelectById(security.User.ID);

            if (currentUser == null || currentUser.Email != security.User.Email)
            {
                throw DFMCoreException.WithMessage(ExceptionPossibilities.WrongUserEmail);
            }
        }



        private static void sendEmail(Security security)
        {
            try
            {
                var fileContent =
                    String.Format(security.Layout, security.Guid);
                    

                new EmailSender()
                    .To(security.User.Email)
                    .Subject("Envio de Senha")
                    .Body(fileContent)
                    .Send();
            }
            catch (Exception e)
            {
                throw DFMCoreException.WithMessage(ExceptionPossibilities.FailOnEmailSend);
            }

            security.Sent = true;
            SaveOrUpdate(security);
        }



        public static void Use(Security security)
        {
            throw new NotImplementedException();
        }

    }
}
