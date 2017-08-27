using System;
using System.Web.Mvc;
using DFM.MVC.Areas.Json.Models;
using DFM.MVC.Helpers;

namespace DFM.MVC.Areas.Json.Controllers
{
    public class UserController : BaseJsonController
    {
        public JsonResult Index(String email, String password)
        {
            var model = new UserLogOnJsonModel { Email = email, Password = password };

            var result = model.LogOn();

            if (result != null)
                return JsonGetError(MultiLanguage.Dictionary[result]);

            return JsonGet(Current.Ticket);
        }

    }
}
