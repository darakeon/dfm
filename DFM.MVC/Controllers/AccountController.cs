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
            var model = new AccountCreateEditModel();

            return View("CreateEdit", model);
        }

        [HttpPost]
        public ActionResult Create(AccountCreateEditModel model)
        {
            return createEdit(model);
        }

        public ActionResult Edit(Int32 id)
        {
            var model = new AccountCreateEditModel
                            {
                                Account = accountData.SelectById(id)
                            };

            return View("CreateEdit", model);
        }

        [HttpPost]
        public ActionResult Edit(Int32 id, AccountCreateEditModel model)
        {
            model.Account.ID = id;

            return createEdit(model);
        }

        private ActionResult createEdit(AccountCreateEditModel model)
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

            return View("CreateEdit", model);
        }



        public ActionResult Close(Int32 id)
        {
            throw new NotImplementedException();
        }
    }
}
