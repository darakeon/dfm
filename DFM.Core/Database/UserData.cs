using System;
using System.Linq;
using DFM.Core.Entities;

namespace DFM.Core.Database
{
    public class UserData : BaseData<User>
    {
        public User SelectByLogin(string login)
        {
            return Session
                .CreateCriteria(typeof(User))
                .List<User>()
                .Where(u => u.Login == login)
                .SingleOrDefault();
        }

        public User Validate(string login, string password)
        {
            var user = SelectByLogin(login);

            if (user == null || user.Password != password)
                throw new ApplicationException("Wrong.");

            return user;
        }

        public override User SaveOrUpdate(User user)
        {
            Validate(user);

            return base.SaveOrUpdate(user);
        }

        private void Validate(User user)
        {
            if (SelectByLogin(user.Login) != null)
                throw new ApplicationException("Already Exists.");
        }
    }
}
