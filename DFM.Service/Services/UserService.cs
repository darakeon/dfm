using System;
using DFM.Core.Database;
using DFM.Service.Entities;

namespace DFM.Service.Services
{
    public class UserService : IUserService
    {
        public User SelectByLogin(String login)
        {
            return (User)UserData.SelectByLogin(login);
        }

        public User ValidateAndGet(String login, String password)
        {
            return (User)UserData.ValidateAndGet(login, password);
        }

        public User SaveOrUpdate(User user)
        {
            return (User)UserData.SaveOrUpdate(user);
        }

        public User SelectById(Int32 id)
        {
            return (User)UserData.SelectById(id);
        }
    }
}