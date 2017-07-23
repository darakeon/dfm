using System;
using System.Web.Mvc;
using DFM.Entities;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities.Extensions;
using DFM.MVC.Helpers;
using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Models;
using DFM.Repositories;

namespace DFM.MVC.Controllers
{
    [Authorize]
    public class AccountController : BaseController
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

        public ActionResult Edit(String id)
        {
            if (String.IsNullOrEmpty(id)) 
                return RedirectToAction("Create");

            var model = new AccountCreateEditModel
            {
                Account = Services.Admin.SelectAccountByName(id)
            };

            if (isUnauthorized(model.Account))
                return RedirectToAction("Create");

            return View("CreateEdit", model);
        }

        [HttpPost]
        public ActionResult Edit(String id, AccountCreateEditModel model)
        {
            model.Account.Name = id;

            var oldAccount = Services.Admin.SelectAccountByName(id);

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
                    Services.Admin.SaveOrUpdateAccount(model.Account);
                }
                catch (DFMCoreException e)
                {
                    ModelState.AddModelError("", MultiLanguage.Dictionary[e]);
                }

                if (ModelState.IsValid)
                {
                    return RedirectToAction("Index");
                }
            }

            return View("CreateEdit", model);
        }



        public ActionResult Close(String id)
        {
            var account =  Services.Admin.SelectAccountByName(id);

            // TODO: implement messages on page head
            //String message;

            try
            {
                if (!isUnauthorized(account))
                    Services.Admin.CloseAccount(id);
                //else
                //    account = null;

                //message = account == null
                //    ? MultiLanguage.Dictionary["AccountNotFound"]
                //    : String.Format(MultiLanguage.Dictionary["AccountClosed"], account.Name);
            }
            catch (DFMCoreException)// e)
            {
                //message = MultiLanguage.Dictionary[e];
            }

            return RedirectToAction("Index");
        }



        public ActionResult Delete(String id)
        {
            var account =  Services.Admin.SelectAccountByName(id);

            // TODO: implement messages on page head
            //String message;

            try
            {
                if (!isUnauthorized(account))
                    Services.Admin.DeleteAccount(id);
                //else
                //    account = null;

                //message = account == null
                //    ? MultiLanguage.Dictionary["AccountNotFound"]
                //    : String.Format(MultiLanguage.Dictionary["AccountDeleted"], account.Name);
            }
            catch (DFMCoreException)// e)
            {
                //message = MultiLanguage.Dictionary[e];
            }


            return RedirectToAction("Index");
        }



        private Boolean isUnauthorized(Account account)
        {
            return account == null
                   || !account.AuthorizeCRUD(Current.User);
        }

    }
}
