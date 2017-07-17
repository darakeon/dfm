using System;
using System.Web.Mvc;
using DFM.Core.Entities;
using DFM.Core.Entities.Extensions;
using DFM.Core.Helpers;
using DFM.MVC.Authentication;
using DFM.MVC.Models;
using DFM.Core.Database;
using DFM.MVC.MultiLanguage;

namespace DFM.MVC.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        public ActionResult Index()
        {
            var model = new AccountIndexModel();

            return View(model);
        }

        public ActionResult ListClosed()
        {
            var model = new AccountIndexModel(false);

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

        public ActionResult Edit(Int32? id)
        {
            if (!id.HasValue) return RedirectToAction("Create");

            var model = new AccountCreateEditModel
            {
                Account = AccountData.SelectById(id.Value)
            };

            if (isUnauthorized(model.Account))
                return RedirectToAction("Create");

            return View("CreateEdit", model);
        }

        [HttpPost]
        public ActionResult Edit(Int32 id, AccountCreateEditModel model)
        {
            model.Account.ID = id;


            var oldAccount = AccountData.SelectById(id);

            if (isUnauthorized(oldAccount))
                return RedirectToAction("Create");


            return createEdit(model);
        }

        private ActionResult createEdit(AccountCreateEditModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.Account.User = Current.User;
                    AccountData.SaveOrUpdate(model.Account);
                }
                catch (DFMCoreException e)
                {
                    ModelState.AddModelError("Name", PlainText.Dictionary[e.Message]);
                }

                if (ModelState.IsValid)
                {
                    return RedirectToAction("Index");
                }
            }

            return View("CreateEdit", model);
        }



        public JsonResult Close(Int32 id)
        {
            var account = AccountData.SelectById(id);

            String message;

            try
            {
                if (isUnauthorized(account))
                    account = null;
                else
                    AccountData.Close(account);

                message = account == null
                    ? PlainText.Dictionary["AccountNotFound"]
                    : String.Format(PlainText.Dictionary["AccountClosed"], account.Name);
            }
            catch (DFMCoreException e)
            {
                message = PlainText.Dictionary[e.Message];
            }


            return new JsonResult { Data = new { message } };
        }



        public JsonResult Delete(Int32 id)
        {
            var account = AccountData.SelectById(id);

            String message;

            try
            {
                if (isUnauthorized(account))
                    account = null;
                else
                    AccountData.Delete(account);

                message = account == null
                    ? PlainText.Dictionary["AccountNotFound"]
                    : String.Format(PlainText.Dictionary["AccountDeleted"], account.Name);
            }
            catch (DFMCoreException e)
            {
                message = PlainText.Dictionary[e.Message];
            }


            return new JsonResult { Data = new { message } };
        }



        private static Boolean isUnauthorized(Account account)
        {
            return account == null
                   || !account.AuthorizeCRUD(Current.User);
        }

    }
}
