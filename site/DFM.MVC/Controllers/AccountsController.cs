using System;
using System.Web.Mvc;
using DFM.Generic;
using DFM.MVC.Helpers.Authorize;
using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Models;

namespace DFM.MVC.Controllers
{
    [DFMAuthorize]
    public class AccountsController : BaseController
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

            var model = new AccountCreateEditModel(OperationType.Edit, id);

            if (model.Account == null)
                return RedirectToAction("Create");
            
            return View("CreateEdit", model);
        }

        [HttpPost]
        public ActionResult Edit(String id, AccountCreateEditModel model)
        {
            model.ResetAccountUrl(OperationType.Edit, id);

            return createEdit(model);
        }

        private ActionResult createEdit(AccountCreateEditModel model)
        {
            if (ModelState.IsValid)
            {
                var errors = model.CreateOrUpdate();

                AddErrors(errors);
            }

            if (ModelState.IsValid)
                return RedirectToAction("Index");

            return View("CreateEdit", model);
        }



        public ActionResult Close(String id)
        {
            var model = new AdminModel();

            model.CloseAccount(id);

            return RedirectToAction("Index");
        }



        public ActionResult Delete(String id)
        {
            var model = new AdminModel();

            model.Delete(id);

            return RedirectToAction("Index");
        }



    }
}
