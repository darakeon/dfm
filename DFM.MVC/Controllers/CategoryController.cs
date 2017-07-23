using System;
using System.Web.Mvc;
using Ak.MVC.Authentication;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities;
using DFM.Entities.Extensions;
using DFM.Generic;
using DFM.MVC.Helpers;
using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Models;
using DFM.Repositories;

namespace DFM.MVC.Controllers
{
    [AjaxAuthorize]
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
            return createEditForHtmlForm(model);
        }

        [HttpPost]
        public JsonResult CreateAjax(CategoryCreateEditModel model)
        {
            try
            {
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

            var model = new CategoryCreateEditModel(OperationType.Update)
            {
                Category = Services.Admin.SelectCategoryByName(id)
            };

            if (isUnauthorized(model.Category))
                return RedirectToAction("Create");
            
            return View("CreateEdit", model);
        }

        [HttpPost]
        public ActionResult Edit(String id, CategoryCreateEditModel model)
        {
            model.Category.Name = id;

            var oldCategory = Services.Admin.SelectCategoryByName(id);

            return isUnauthorized(oldCategory)
                ? RedirectToAction("Create")
                : createEditForHtmlForm(model);
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
            try
            {

            }
            catch (Exception)
            {
                
                throw;
            }
            model.Category.User = Current.User;

            if (model.Type == OperationType.Creation)
                Services.Admin.CreateCategory(model.Category);
            else
                Services.Admin.UpdateCategory(model.Category, model.Name);

            return model.Category;
        }



        public ActionResult Disable(String id)
        {
            var category = Services.Admin.SelectCategoryByName(id);

            if (!isUnauthorized(category))
                Services.Admin.DisableCategory(id);
            //else
            //    category = null;

            // TODO: implement messages on page head
            //var message = category == null
            //    ? MultiLanguage.Dictionary["CategoryNotFound"]
            //    : String.Format(MultiLanguage.Dictionary["CategoryDisabled"], category.Name);

            return RedirectToAction("Index");
        }



        public ActionResult Enable(String id)
        {
            var category =  Services.Admin.SelectCategoryByName(id);

            if (!isUnauthorized(category))
                Services.Admin.EnableCategory(id);
            //else
            //    category = null;

            // TODO: implement messages on page head
            //var message = category == null
            //    ? MultiLanguage.Dictionary["CategoryNotFound"]
            //    : String.Format(MultiLanguage.Dictionary["CategoryEnabled"], category.Name);

            return RedirectToAction("Index");
        }



        private Boolean isUnauthorized(Category category)
        {
            return category == null
                   || !category.AuthorizeCRUD(Current.User);
        }

    }
}
