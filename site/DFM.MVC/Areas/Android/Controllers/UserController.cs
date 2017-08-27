using System;
using System.Web.Mvc;
using DFM.BusinessLogic.Exceptions;
using DFM.MVC.Areas.Android.Models;
using DFM.MVC.Helpers;

namespace DFM.MVC.Areas.Android.Controllers
{
    public class UserController : BaseJsonController
    {
        public ActionResult Index(String email, String password, String ticket)
        {
            var model = new UserLogOnJsonModel { Email = email, Password = password };

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
