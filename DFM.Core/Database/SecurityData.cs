using System;
using System.Web.Mail;
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

        public static void CreateAndSend(String email, SecurityAction action)
        {
            var user = UserData.SelectByEmail(email);

            if (user == null)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.WrongUserEmail);

            CreateAndSend(user, action);
        }

        internal static void CreateAndSend(User user, SecurityAction action)
        {
            var security = new Security { Action = action, User = user };

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
                EmailSender.Send();
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
