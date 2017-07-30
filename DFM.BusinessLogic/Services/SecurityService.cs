using System;
using System.Collections.Generic;
using Ak.Generic.Extensions;
using DFM.BusinessLogic.Bases;
using DFM.Email;
using DFM.Email.Exceptions;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Entities.Extensions;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Helpers;
using ExceptionPossibilities = DFM.BusinessLogic.Exceptions.ExceptionPossibilities;

namespace DFM.BusinessLogic.Services
{
    internal class SecurityService : BaseService<Security>
    {
        internal SecurityService(IRepository<Security> repository) : base(repository) { }



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



        internal void SendEmail(Security security)
        {
            var dic = new Dictionary<String, String>
                            {
                                { "Url", Dfm.Url },
                                { "Token", security.Token },
                                { "Date", security.Expire.AddDays(-1).ToShortDateString() }
                            };

            var format = new Format(security.User.Language, security.Action);
            

            var fileContent = format.Layout.Format(dic);

            try
            {
                new Sender()
                    .To(security.User.Email)
                    .Subject(format.Subject)
                    .Body(fileContent)
                    .Send();
            }
            catch (DFMEmailException)
            {
                throw DFMCoreException.WithMessage(ExceptionPossibilities.FailOnEmailSend);
            }

            security.Sent = true;
            SaveOrUpdate(security);
        }



        internal Security SelectByToken(String token)
        {
            // TODO: Try refactor here
            return SingleOrDefault(
                s => s.Token == token 
                    && s.Active == true
                    && s.Expire >= DateTime.Now);
        }


        
        internal Boolean TokenExist(String token)
        {
            return SelectByToken(token) != null;
        }



        internal void Disable(String token)
        {
            var security = SelectByToken(token);

            if (security == null)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidToken);

            security.Active = false;

            SaveOrUpdate(security);
        }




        internal Security ValidateAndGet(String token, SecurityAction securityAction)
        {
            var securityToken = SelectByToken(token);

            if (securityToken == null || securityToken.Action != securityAction)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidToken);

            return securityToken;
        }

    }
}
