using System;
using System.Collections.Generic;
using DFM.Core.Database.Base;
using DFM.Core.Entities;
using DFM.Core.Entities.Extensions;
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
            var security = new Security { Action = action, User = user };//, Subject = subject, Layout = layout };

            security = SaveOrUpdate(security);

            sendEmail(security, subject, layout);
        }

        internal static Security SaveOrUpdate(Security security)
        {
            return SaveOrUpdate(security, complete, validate);
        }



        private static void complete(Security security)
        {
            if (security.ID != 0) return;
            
            security.Active = true;
            security.Expire = DateTime.Now.AddMonths(1);
            security.CreateToken();
        }


        private static void validate(Security security)
        {
            var currentUser = UserData.SelectById(security.User.ID);

            if (currentUser == null || currentUser.Email != security.User.Email)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.WrongUserEmail);
        }



        private static void sendEmail(Security security, String subject, String layout)
        {
            var dic = new Dictionary<String, String>
                            {
                                { "Url", Dfm.Url },
                                { "Token", security.Token },
                                { "Date", security.Expire.AddDays(-1).ToShortDateString() }
                            };

            var fileContent =
                layout.Format(dic);
                    
            new EmailSender()
                .To(security.User.Email)
                .Subject(subject)
                .Body(fileContent)
                .Send();

            security.Sent = true;
            SaveOrUpdate(security);
        }



        private static Security selectByToken(String token)
        {
            var criteria = CreateSimpleCriteria(
                s => s.Token == token 
                    && s.Active 
                    && s.Expire >= DateTime.Now);

            return criteria.UniqueResult<Security>();
        }


        
        public static Boolean TokenExist(String token)
        {
            return selectByToken(token) != null;
        }



        public static void Deactivate(String token)
        {
            var security = selectByToken(token);

            security.Active = false;

            SaveOrUpdate(security);
        }



        public static void UserActivate(String token)
        {

            Deactivate(token);
            throw new NotImplementedException();
        }


        public static void PasswordReset(String token, String password)
        {
            var security = selectByToken(token);

            security.User.Password = password;

            UserData.ChangePassword(security.User);

            Deactivate(token);
        }
    }
}
