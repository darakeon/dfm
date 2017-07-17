using System;
using System.Text.RegularExpressions;
using DFM.Core.Database.Base;
using DFM.Core.Entities;
using DFM.Core.Helpers;

namespace DFM.Core.Database
{
    public class UserData : BaseData<User>
    {
		private UserData() { }

        public static User SelectByLogin(String email)
        {
            var criteria = CreateSimpleCriteria(u => u.Email == email);

            return criteria.UniqueResult<User>();
        }

        public static User ValidateAndGet(String email, String password)
        {
            var user = SelectByLogin(email);

            if (user == null || user.Password != password)
                throw DFMCoreException.WithMessage(DFMCoreException.Possibilities.InvalidUser);

            return user;
        }



        public static User SaveOrUpdate(User user)
        {
            return SaveOrUpdate(user, validate, complete);
        }

        private static void validate(User user)
        {
            var regex = new Regex(@"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$", RegexOptions.IgnoreCase);

            if (!regex.Match(user.Email).Success)
                throw DFMCoreException.WithMessage(DFMCoreException.Possibilities.UserInvalidEmail);

            if (SelectByLogin(user.Email) != null)
                throw DFMCoreException.WithMessage(DFMCoreException.Possibilities.UserAlreadyExists);
        }

        private static void complete(User user)
        {
            user.Language = "pt-BR";
        }

    }
}
