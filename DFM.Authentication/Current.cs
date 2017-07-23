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


        private User userApp;

        private Boolean isWeb
        {
            get { return HttpContext.Current != null; }
        }

        public User User
        {
            get
            {
                if (!isWeb)
                    return userApp;

                if (!Authenticate.IsAuthenticated)
                    return null;
                
                var username = Authenticate.Username;

                return userService.SelectUserByTicket(username);
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



        public User Set(String username, String password)
        {
            if (isWeb)
                throw DFMAuthException.IsWeb();

            var ticket = userService.ValidateUserAndGetTicket(username, password);

            userApp = userService.SelectUserByTicket(ticket);

            return userApp;
        }

        public User Set(String username, String password, HttpResponseBase response, Boolean isPersistent)
        {
            if (!isWeb)
                throw DFMAuthException.NotWeb();

            userService.ValidateUserAndGetTicket(username, password);

            Authenticate.Set(username, response, isPersistent);

            return userService.SelectUserByTicket(username);
        }



        public void Clean()
        {
            if (isWeb)
                throw DFMAuthException.IsWeb();

            userApp = null;
        }

        public void Clean(HttpRequestBase request)
        {
            if (!isWeb)
                throw DFMAuthException.NotWeb();

            Authenticate.Clean(request);
        }








    }
}