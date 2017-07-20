using System;
using System.Collections.Generic;
using Ak.Generic.Extensions;
using DFM.Email;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Extensions;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Helpers;

namespace DFM.BusinessLogic.Services
{
    internal class SecurityService : BaseService<Security>
    {
        internal SecurityService(IRepository repository) : base(repository) { }




        internal void SendUserVerify(User user, Format format)
        {
            CreateAndSend(user, SecurityAction.UserVerification, format);
        }


        internal void CreateAndSend(User user, SecurityAction action, Format format)
        {
            var security = new Security { Action = action, User = user };

            security = SaveOrUpdate(security);

            sendEmail(security, format);
        }



        internal Security SaveOrUpdate(Security security)
        {
            return SaveOrUpdate(security, complete);
        }



        private static void complete(Security security)
        {
            if (security.ID != 0) return;
            
            security.Active = true;
            security.Expire = DateTime.Now.AddMonths(1);
            security.CreateToken();
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


        internal Security SelectByToken(String token)
        {
            return SingleOrDefault(
                s => s.Token == token 
                    && s.Active 
                    && s.Expire >= DateTime.Now);
        }


        
        internal Boolean TokenExist(String token)
        {
            return SelectByToken(token) != null;
        }



        internal void Deactivate(String token)
        {
            var security = SelectByToken(token);

            security.Active = false;

            SaveOrUpdate(security);
        }






        internal SecurityAction GetTokenAction(String token)
        {
            var security = SelectByToken(token);

            if (security == null)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidToken);

            return security.Action;
        }



    }
}
