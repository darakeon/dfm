using System;
using System.Collections.Generic;
using System.Linq;
using Ak.Generic.Extensions;
using DFM.Email;
using DFM.Entities;
using DFM.Extensions;
using DFM.Core.Enums;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Helpers;

namespace DFM.BusinessLogic.Services
{
    public class SecurityService : BaseService<Security>
    {
        internal SecurityService(DataAccess father, IRepository repository) : base(father, repository) { }

        public void PasswordReset(String email, Format format)
        {
            createAndSend(email, SecurityAction.PasswordReset, format);
        }



        public void SendUserVerify(User user, Format format)
        {
            createAndSend(user, SecurityAction.UserVerification, format);
        }



        private void createAndSend(String email, SecurityAction action, Format format)
        {
            var user = Father.User.SelectByEmail(email);

            if (user == null)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.WrongUserEmail);

            createAndSend(user, action, format);
        }

        private void createAndSend(User user, SecurityAction action, Format format)
        {
            var security = new Security { Action = action, User = user };

            security = SaveOrUpdate(security);

            sendEmail(security, format);
        }



        internal Security SaveOrUpdate(Security security)
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


        private void validate(Security security)
        {
            var currentUser = Father.User.SelectById(security.User.ID);

            if (currentUser == null || currentUser.Email != security.User.Email)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.WrongUserEmail);
        }



        private void sendEmail(Security security, Format format)
        {
            var dic = new Dictionary<String, String>
                            {
                                { "Url", Dfm.Url },
                                { "Token", security.Token },
                                { "Date", security.Expire.AddDays(-1).ToShortDateString() }
                            };

            var fileContent = format.Layout.Format(dic);

            try
            {
                new Sender()
                    .To(security.User.Email)
                    .Subject(format.Subject)
                    .Body(fileContent)
                    .Send();
            }
            catch (Exception)
            {
                throw DFMCoreException.WithMessage(ExceptionPossibilities.FailOnEmailSend);
            }

            security.Sent = true;
            SaveOrUpdate(security);
        }



        private Security selectByToken(String token)
        {
            return SingleOrDefault(
                s => s.Token == token 
                    && s.Active 
                    && s.Expire >= DateTime.Now);
        }


        
        public Boolean TokenExist(String token)
        {
            return selectByToken(token) != null;
        }



        public void Deactivate(String token)
        {
            var security = selectByToken(token);

            security.Active = false;

            SaveOrUpdate(security);
        }



        public void UserActivate(String token)
        {
            var security = selectByToken(token);

            if (security.Action != SecurityAction.UserVerification)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidToken);

            Father.User.Activate(security.User);

            Deactivate(token);
        }


        public void PasswordReset(String token, String password)
        {
            var security = selectByToken(token);

            if (security.Action != SecurityAction.PasswordReset)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidToken);

            security.User.Password = password;

            Father.User.ChangePassword(security.User);

            Deactivate(token);
        }



        public SecurityAction GetTokenAction(String token)
        {
            var security = selectByToken(token);

            if (security == null)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidToken);

            return security.Action;
        }



        internal Security GetUserActivation(User user)
        {
            var securityList = 
                List(s => s.User.ID == user.ID 
                        && s.Action == SecurityAction.UserVerification
                        && s.Active
                        && s.Expire >= DateTime.Now);

            return securityList.LastOrDefault();
        }

    }
}
