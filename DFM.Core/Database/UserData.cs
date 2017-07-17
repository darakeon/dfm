using System;
using DFM.Core.Database.Bases;
using DFM.Core.Entities;
using DFM.Core.Helpers;

namespace DFM.Core.Database
{
    public class UserData : BaseData<User>
    {
        public UserData()
        {
            Validate += validate;
        }

        public User SelectByLogin(String login)
        {
            return SelectSingle(u => u.Login == login);
        }

        public User ValidateAndGet(String login, String password)
        {
            var user = SelectByLogin(login);

            if (user == null || user.Password != password)
                throw new CoreValidationException("InvalidUser");

            return user;
        }


        private void validate(User user)
        {
            if (SelectByLogin(user.Login) != null)
                throw new CoreValidationException("AlreadyExists");
        }
    }
}
