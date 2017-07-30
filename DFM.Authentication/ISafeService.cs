using System;
using DFM.Entities;

namespace DFM.Authentication
{
    public interface ISafeService
    {
        User SelectUserByTicket(String ticket);

        String ValidateUserAndGetTicket(String username, String password);

        void DisableTicket(String ticket);

    }
}
