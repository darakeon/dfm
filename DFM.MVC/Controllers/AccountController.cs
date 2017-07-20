using System;
using System.Web.Mvc;
using DFM.Entities;
using DFM.Extensions;
using DFM.BusinessLogic.Exceptions;
using DFM.MVC.Authentication;
using DFM.MVC.Models;
using DFM.MVC.MultiLanguage;
using DFM.Repositories;

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
                Account = Service.Access.Admin.SelectAccountById(id.Value)
            };

            if (isUnauthorized(model.Account))
                return RedirectToAction("Create");

            return View("CreateEdit", model);
        }

        [HttpPost]
        public ActionResult Edit(Int32 id, AccountCreateEditModel model)
        {
            model.Account.ID = id;


            var oldAccount = Service.Access.Admin.SelectAccountById(id);

            return isUnauthorized(oldAccount)
                ? RedirectToAction("Create")
                : createEdit(model);
        }

        private ActionResult createEdit(AccountCreateEditModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.Account.User = Current.User;
                    Service.Access.Admin.SaveOrUpdateAccount(model.Account);
                }
                catch (DFMCoreException e)
                {
                    ModelState.AddModelError("", PlainText.Dictionary[e.Message]);
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
            var account =  Service.Access.Admin.SelectAccountById(id);

            // TODO: implement messages on page head
            //String message;

            try
            {
                if (!isUnauthorized(account))
                    Service.Access.Admin.CloseAccount(account);
                //else
                //    account = null;

                //message = account == null
                //    ? PlainText.Dictionary["AccountNotFound"]
                //    : String.Format(PlainText.Dictionary["AccountClosed"], account.Name);
            }
            catch (DFMCoreException)// e)
            {
                //message = PlainText.Dictionary[e.Message];
            }

            return RedirectToAction("Index");
        }



        public ActionResult Delete(Int32 id)
        {
            var account =  Service.Access.Admin.SelectAccountById(id);

            // TODO: implement messages on page head
            //String message;

            try
            {
                if (!isUnauthorized(account))
                    Service.Access.Admin.DeleteAccount(account);
                //else
                //    account = null;

                //message = account == null
                //    ? PlainText.Dictionary["AccountNotFound"]
                //    : String.Format(PlainText.Dictionary["AccountDeleted"], account.Name);
            }
            catch (DFMCoreException)// e)
            {
                //message = PlainText.Dictionary[e.Message];
            }


            return RedirectToAction("Index");
        }



        private static Boolean isUnauthorized(Account account)
        {
            return account == null
                   || !account.AuthorizeCRUD(Current.User);
        }

    }
}
