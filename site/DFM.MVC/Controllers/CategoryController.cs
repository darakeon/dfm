using System;
using System.Web.Mvc;
using DFM.Generic;
using DFM.MVC.Helpers.Authorize;
using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Helpers.Global;
using DFM.MVC.Models;

namespace DFM.MVC.Controllers
{
    [DFMAjaxAuthorize]
    public class CategoryController : BaseController
    {
        public ActionResult Index()
        {
            var model = new CategoryIndexModel();

            return View(model);
        }



        public ActionResult Create()
        {
            var model = new CategoryCreateEditModel(OperationType.Creation);

            model.DefineAction(Request);

            return View("CreateEdit", model);
        }

        [HttpPost]
        public ActionResult Create(CategoryCreateEditModel model)
        {
            model.Type = OperationType.Creation;

            return createEditForHtmlForm(model);
        }

        [HttpPost]
        public JsonResult CreateAjax(CategoryCreateEditModel model)
        {
            model.Type = OperationType.Creation;

            var error = model.CreateEdit();

            if (error != null)
                throw new Exception(MultiLanguage.Dictionary[error]);

            var json = new { name = model.Category.Name };

            return new JsonResult { Data = json };
        }


        public ActionResult Edit(String id)
        {
            if (String.IsNullOrEmpty(id)) 
                return RedirectToAction("Create");

            var model = new CategoryCreateEditModel(OperationType.Edit, id);

            return View("CreateEdit", model);
        }

        [HttpPost]
        public ActionResult Edit(String id, CategoryCreateEditModel model)
        {
            model.Type = OperationType.Edit;
            model.Category.Name = id;

            return createEditForHtmlForm(model);
        }



        private ActionResult createEditForHtmlForm(CategoryCreateEditModel model)
        {
            if (ModelState.IsValid)
            {
                var error = model.CreateEdit();

                if (error != null)
                    ModelState.AddModelError("Category.Name", MultiLanguage.Dictionary[error]);
            }

            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            model.DefineAction(Request);

            return View("CreateEdit", model);
        }



        public ActionResult Disable(String id)
        {
            var model = new AdminModel();

            model.Disable(id);

            return RedirectToAction("Index");
        }



        public ActionResult Enable(String id)
        {
            var model = new AdminModel();

            model.Enable(id);
            
            return RedirectToAction("Index");
        }


    }
}
