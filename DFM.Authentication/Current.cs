using System;
using System.Web;
using Ak.MVC.Authentication;
using DFM.Entities;
using DFM.Entities.Extensions;

namespace DFM.Authentication
{
    public class Current
    {
        private IUserService userService { get; set; }

        public Current(IUserService userService)
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

                return userService.SelectUserByEmail(username);
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

            var user = userService.ValidateAndGet(username, password);

            if (user.Active)
                userApp = user;

            return user;
        }

        public User Set(String username, String password, HttpResponseBase response, Boolean isPersistent)
        {
            if (!isWeb)
                throw DFMAuthException.NotWeb();

            var user = userService.ValidateAndGet(username, password);

            if (user.Active)
                Authenticate.Set(username, response, isPersistent);

            return user;
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