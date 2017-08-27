using System;
using System.Web.Mvc;
using DFM.BusinessLogic.Exceptions;
using DFM.MVC.Areas.API.Models;
using DFM.MVC.Helpers;

namespace DFM.MVC.Areas.API.Controllers
{
    public class UserController : BaseJsonController
    {
        public ActionResult Index(String id, String email, String password)
        {
            if (String.IsNullOrEmpty(id))
                return RedirectToAction("Uninvited");

            var model =
                new UserLogOnJsonModel
                {
                    Email = email,
                    Password = password,
                    MachineId = id
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
