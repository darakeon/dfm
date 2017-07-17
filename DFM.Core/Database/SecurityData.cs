using System;
using System.Collections.Generic;
using System.Linq;
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

        public static void PasswordReset(String email, String subject, String layout)
        {
            createAndSend(email, SecurityAction.PasswordReset, subject, layout);
        }



        public static void SendUserVerify(User user, String subject, String layout)
        {
            createAndSend(user, SecurityAction.UserVerify, subject, layout);
        }



        private static void createAndSend(String email, SecurityAction action, String subject, String layout)
        {
            var user = UserData.SelectByEmail(email);

            if (user == null)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.WrongUserEmail);

            createAndSend(user, action, subject, layout);
        }

        private static void createAndSend(User user, SecurityAction action, String subject, String layout)
        {
            var security = new Security { Action = action, User = user };

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
            var security = selectByToken(token);

            if (security.Action != SecurityAction.UserVerify)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidToken);

            UserData.Activate(security.User);

            Deactivate(token);
        }


        public static void PasswordReset(String token, String password)
        {
            var security = selectByToken(token);

            if (security.Action != SecurityAction.PasswordReset)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidToken);

            security.User.Password = password;

            UserData.ChangePassword(security.User);

            Deactivate(token);
        }



        public static SecurityAction GetTokenAction(String token)
        {
            var security = selectByToken(token);

            if (security == null)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidToken);

            return security.Action;
        }



        internal static Security GetUserActivation(User user)
        {
            var criteria = 
                CreateSimpleCriteria(
                    s => s.User.ID == user.ID 
                        && s.Action == SecurityAction.UserVerify
                        && s.Active
                        && s.Expire >= DateTime.Now);

            return criteria
                .List<Security>()
                .LastOrDefault();
        }

    }
}
