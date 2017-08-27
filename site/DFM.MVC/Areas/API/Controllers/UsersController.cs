using System;
using System.Web.Mvc;
using DFM.BusinessLogic.Exceptions;
using DFM.MVC.Areas.API.Models;
using DFM.MVC.Helpers;

namespace DFM.MVC.Areas.API.Controllers
{
    public class UsersController : BaseJsonController
    {
        public ActionResult Login(String email, String password)
        {
            var model =
                new UsersLoginModel
                {
                    Email = email,
                    Password = password,
                };

            var result = model.LogOn();

            if (result != null)
                return JsonGetError(MultiLanguage.Dictionary[result]);

            return JsonGet(Current.Ticket);
        }



        public ActionResult Uninvited()
        {
            return JsonGetError(MultiLanguage.Dictionary[ExceptionPossibilities.Uninvited]);
        }



    }
}
