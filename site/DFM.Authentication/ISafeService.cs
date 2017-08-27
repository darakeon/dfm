using System;
using DFM.Entities;
using DFM.Generic;

namespace DFM.Authentication
{
    public interface ISafeService
    {
        User GetUserByTicket(String ticket);

        String ValidateUserAndCreateTicket(String username, String password, PseudoTicket ticket);

        void DisableTicket(String ticket);

    }
}
