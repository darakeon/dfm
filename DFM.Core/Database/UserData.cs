using System;
using DFM.Core.Entities;
using NHibernate.Criterion;

namespace DFM.Core.Database
{
    public class UserData : BaseData<User>
    {
        public UserData()
        {
            Validate += validate;
        }

        public User SelectByLogin(string login)
        {
            return Session
                .CreateCriteria(typeof(User))
                .Add(Restrictions.Eq("Login", login))
                .UniqueResult<User>();
        }

        public User GetAndValidate(string login, string password)
        {
            var user = SelectByLogin(login);

            if (user == null || user.Password != password)
                throw new ApplicationException("Wrong.");

            return user;
        }


        private void validate(User user)
        {
            if (SelectByLogin(user.Login) != null)
                throw new ApplicationException("Already Exists.");
        }
    }
}
