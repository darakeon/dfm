using System;
using DFM.Service.Entities;

namespace DFM.Service
{
    public interface IUserService
    {
        User SelectByLogin(String login);
        User ValidateAndGet(String login, String password);
        User SaveOrUpdate(User entity);
        User SelectById(Int32 id);
    }
}
