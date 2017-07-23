using System;
using System.Web.Mvc;
using DFM.BusinessLogic.Exceptions;
using DFM.Generic;
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
            var model = new AccountCreateEditModel(OperationType.Creation);

            return View("CreateEdit", model);
        }

        [HttpPost]
        public ActionResult Create(AccountCreateEditModel model)
        {
            model.Type = OperationType.Creation;

            return createEdit(model);
        }

        public ActionResult Edit(String id)
        {
            if (String.IsNullOrEmpty(id)) 
                return RedirectToAction("Create");

            var model = new AccountCreateEditModel(OperationType.Edit)
            {
                Account = Services.Admin.SelectAccountByName(id)
            };

            if (model.Account == null)
                return RedirectToAction("Create");
            
            return View("CreateEdit", model);
        }

        [HttpPost]
        public ActionResult Edit(String id, AccountCreateEditModel model)
        {
            var oldAccount = Services.Admin.SelectAccountByName(id);

            model.Type = OperationType.Edit;
            model.Account.Name = oldAccount.Name;

            return createEdit(model);
        }

        private ActionResult createEdit(AccountCreateEditModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.Account.User = Current.User;

                    if (model.Type == OperationType.Creation)
                        Services.Admin.CreateAccount(model.Account);
                    else
                        Services.Admin.UpdateAccount(model.Account, model.Name);
                }
                catch (DFMCoreException e)
                {
                    ModelState.AddModelError("", MultiLanguage.Dictionary[e]);
                }

                if (ModelState.IsValid)
                    return RedirectToAction("Index");
            }

            return View("CreateEdit", model);
        }



        public ActionResult Close(String id)
        {
            // TODO: implement messages on page head
            //String message;

            try
            {
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
            // TODO: implement messages on page head
            //String message;

            try
            {
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



    }
}
