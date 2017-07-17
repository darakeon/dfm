using System;
using DFM.Core.Database.Base;
using DFM.Core.Entities;
using DFM.Core.Helpers;

namespace DFM.Core.Database
{
    public class UserData : BaseData<User>
    {
		private UserData() { }

        public static User SelectByLogin(String login)
        {
            return SelectSingle(u => u.Login == login);
        }

        public static User ValidateAndGet(String login, String password)
        {
            var user = SelectByLogin(login);

            if (user == null || user.Password != password)
                throw new DFMCoreException("InvalidUser");

            return user;
        }



        public static User SaveOrUpdate(User user)
        {
            return SaveOrUpdate(user, validate, null);
        }

        private static void validate(User user)
        {
            if (SelectByLogin(user.Login) != null)
                throw new DFMCoreException("AlreadyExists");
        }
    }
}
