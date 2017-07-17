using System;
using DFM.Core.Database;
using DFM.Service.Entities;

namespace DFM.Service.Services
{
    public class UserService : IUserService
    {
        private readonly UserData data = new UserData();

        public User SelectByLogin(String login)
        {
            return (User)data.SelectByLogin(login);
        }

        public User ValidateAndGet(String login, String password)
        {
            return (User)data.ValidateAndGet(login, password);
        }

        public User SaveOrUpdate(User user)
        {
            return (User)data.SaveOrUpdate(user);
        }

        public User SelectById(Int32 id)
        {
            return (User)data.SelectById(id);
        }
    }
}