using System;
using System.Web.Mvc;
using DFM.Core.Helpers;
using DFM.MVC.Authentication;
using DFM.MVC.Models;
using DFM.Core.Database;

namespace DFM.MVC.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        readonly AccountData accountData = new AccountData();


        public ActionResult Index()
        {
            var model = new AccountIndexModel();

            return View(model);
        }




        public ActionResult Create()
        {
            var model = new AccountCreateModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(AccountCreateModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.Account.User = Current.User;
                    accountData.SaveOrUpdate(model.Account);
                }
                catch (CoreValidationException e)
                {
                    ModelState.AddModelError("Name", e.Message);
                }

                if (ModelState.IsValid)
                {
                    return RedirectToAction("Index");
                }
            }

            return View(model);
        }

        public ActionResult Close(Int32 id)
        {
            throw new NotImplementedException();
        }
    }
}
