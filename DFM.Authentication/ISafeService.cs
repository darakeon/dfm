using System;
using DFM.Entities;

namespace DFM.Authentication
{
    public interface ISafeService
    {
        User SelectUserByTicket(String username);

        String ValidateUserAndGetTicket(String username, String password);


    }
}
