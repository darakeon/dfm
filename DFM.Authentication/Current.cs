using System;
using System.Web;
using Ak.MVC.Authentication;
using DFM.Entities;
using DFM.Entities.Extensions;

namespace DFM.Authentication
{
    public class Current
    {
        private ISafeService userService { get; set; }

        public Current(ISafeService userService)
        {
            this.userService = userService;
        }


        private String ticket;

        private Boolean isWeb
        {
            get { return HttpContext.Current != null; }
        }

        public User User
        {
            get
            {
                if (!isWeb)
                    return userService.SelectUserByTicket(ticket);

                if (!Authenticate.IsAuthenticated)
                    return null;
                
                ticket = Authenticate.Username;

                return userService.SelectUserByTicket(ticket);
            }
        }



        public Boolean IsAuthenticated
        {
            get { return User != null; }
        }


        public String Language {
            get { return User == null ? null : User.Language; }
        }

        
        public Boolean IsAdm
        {
            get { return IsAuthenticated && User.IsAdm(); }
        }



        public void Set(String username, String password)
        {
            if (isWeb)
                throw DFMAuthException.IsWeb();

            ticket = userService.ValidateUserAndGetTicket(username, password);
        }

        public void Set(String username, String password, HttpResponseBase response, Boolean isPersistent)
        {
            if (!isWeb)
                throw DFMAuthException.NotWeb();

            ticket = userService.ValidateUserAndGetTicket(username, password);

            Authenticate.Set(ticket, response, isPersistent);
        }



        public void Clean()
        {
            if (isWeb)
                throw DFMAuthException.IsWeb();

            userService.DisableTicket(ticket);

            ticket = null;
        }

        public void Clean(HttpRequestBase request)
        {
            if (!isWeb)
                throw DFMAuthException.NotWeb();

            userService.DisableTicket(Authenticate.Username);

            Authenticate.Clean(request);
        }



    }
}