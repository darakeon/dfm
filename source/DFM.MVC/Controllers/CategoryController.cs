using System;
using System.Web.Mvc;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities;
using DFM.Generic;
using DFM.MVC.Helpers;
using DFM.MVC.Helpers.Authorize;
using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Models;
using DFM.Repositories;

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
            try
            {
                model.Type = OperationType.Creation;

                var category = createEditForAll(model);
                var json = new { name = category.Name };

                return new JsonResult { Data = json };
            }
            catch (DFMCoreException e)
            {
                throw new Exception(MultiLanguage.Dictionary[e]);
            }
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
            try
            {
                createEditForAll(model);

                if (ModelState.IsValid)
                    return RedirectToAction("Index");
            }
            catch (DFMCoreException e)
            {
                ModelState.AddModelError("Category.Name", MultiLanguage.Dictionary[e]);
            }

            model.DefineAction(Request);

            return View("CreateEdit", model);
        }

        private Category createEditForAll(CategoryCreateEditModel model)
        {
            model.Category.User = Current.User;

            model.CreateEdit();

            return model.Category;
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
