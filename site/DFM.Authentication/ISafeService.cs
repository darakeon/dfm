using System;
using DK.MVC.Cookies;
using DFM.Entities;

namespace DFM.Authentication
{
    public interface ISafeService
    {
        User GetUserByTicket(String ticket);

        String ValidateUserAndCreateTicket(String username, String password, PseudoTicket ticket);

        void DisableTicket(String ticket);

    }
}
