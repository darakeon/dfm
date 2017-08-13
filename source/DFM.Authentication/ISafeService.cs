using System;
using DFM.Entities;

namespace DFM.Authentication
{
    public interface ISafeService
    {
        User GetUserByTicket(String ticket);

        String ValidateUserAndCreateTicket(String username, String password);

        void DisableTicket(String ticket);

    }
}
