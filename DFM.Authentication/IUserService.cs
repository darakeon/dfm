using System;
using DFM.Entities;

namespace DFM.Authentication
{
    public interface IUserService
    {
        User SelectUserByEmail(String username);

        User ValidateAndGet(String username, String password);


    }
}
