using System;
using DFM.Entities;

namespace DFM.Authentication
{
    public interface ISafeService
    {
        User GetUserByTicket(String ticket);

        String ValidateUserAndCreateTicket(String username, String password, String ticket);

        void DisableTicket(String ticket);

    }
}
