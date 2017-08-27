using System;
using System.Web.Mvc;
using DFM.BusinessLogic.Exceptions;
using DFM.MVC.Areas.API.Models;
using DFM.MVC.Helpers.Authorize;
using DFM.MVC.Helpers.Global;
using DFM.MVC.Models;

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

            return JsonGet(new { ticket = Current.Ticket.Key });
        }



        public ActionResult Logout()
        {
            var model = new SafeModel();

            model.LogOff();

            return JsonGet(new { success = true });
        }



        public ActionResult Uninvited()
        {
            return JsonGetError(MultiLanguage.Dictionary[ExceptionPossibilities.Uninvited]);
        }


        [DFMApiAuthorize, HttpGet]
        public ActionResult GetConfig()
        {
            try
            {
                return JsonGet(new UserGetConfigModel());
            }
            catch (DFMCoreException e)
            {
                return JsonGetError(MultiLanguage.Dictionary[e]);
            }
        }

        [DFMApiAuthorize, HttpPost]
        public ActionResult SaveConfig(UserSaveConfigModel model)
        {
            try
            {
                model.Save();

                return JsonPostSuccess();
            }
            catch (DFMCoreException e)
            {
                return JsonPostError(MultiLanguage.Dictionary[e]);
            }
        }



    }
}
