using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities.Enums;
using DFM.Generic;

namespace DFM.Entities.Extensions
{
    public static class UserExtension
    {
        public static Boolean IsAdm(this User user)
        {
            return user.Email == "[some admin e-mail here]";
        }

        public static DateTime Now(this User user)
        {
            return DateTimeGMT.Now(user.Config.TimeZone);
        }

        public static Boolean HasPendentActivation(this User user)
        {
            return user.SecurityList
                .Any(s => s.Action == SecurityAction.UserVerification
                     && s.Expire >= user.Now()
                     && s.Active);
        }

        public static IList<Category> VisibleCategoryList(this User user)
        {
            return user.CategoryList
                .Where(c => c.Active)
                .OrderBy(c => c.Name)
                .ToList();
        }

        public static IList<Account> VisibleAccountList(this User user)
        {
            return user.AccountList
                .Where(c => c.IsOpen())
                .OrderBy(a => a.Name)
                .ToList();
        }

    }
}
