using System;
using System.Web.Mvc;
using Ak.MVC.Authentication;
using DFM.Entities;
using DFM.Extensions;
using DFM.MVC.Authentication;
using DFM.MVC.Models;
using DFM.Repositories;

namespace DFM.MVC.Controllers
{
    [AjaxAuthorize]
    public class CategoryController : Controller
    {
        public ActionResult Index()
        {
            var model = new CategoryIndexModel();

            return View(model);
        }


        
        public ActionResult Create()
        {
            var model = new CategoryCreateEditModel();

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
            var category = createEditForAll(model);
            var json = new { id = category.ID, name = category.Name };

            return new JsonResult { Data = json };
        }


        public ActionResult Edit(Int32? id)
        {
            if (!id.HasValue) return RedirectToAction("Create");

            var model = new CategoryCreateEditModel
            {
                Category = Service.Access.Admin.SelectCategoryById(id.Value)
            };

            if (isUnauthorized(model.Category))
                return RedirectToAction("Create");
            
            return View("CreateEdit", model);
        }

        [HttpPost]
        public ActionResult Edit(Int32 id, CategoryCreateEditModel model)
        {
            model.Category.ID = id;

            var oldCategory = Service.Access.Admin.SelectCategoryById(id);

            return isUnauthorized(oldCategory)
                ? RedirectToAction("Create")
                : createEditForHtmlForm(model);
        }



        private ActionResult createEditForHtmlForm(CategoryCreateEditModel model)
        {
            var category = createEditForAll(model);

            if (category.ID != 0)
                return RedirectToAction("Index");


            model.DefineAction(Request);

            return View("CreateEdit", model);
        }

        private Category createEditForAll(CategoryCreateEditModel model)
        {
            var categoryIdEmpty = model.Category == null
                                  || String.IsNullOrEmpty(model.Category.Name);

            if (categoryIdEmpty)
            {
                ModelState.AddModelError("Category.Name", "");
                return new Category {ID = 0};
            }


            model.Category.User = Current.User;

            Service.Access.Admin.SaveOrUpdateCategory(model.Category);

            return model.Category;
        }



        public ActionResult Disable(Int32 id)
        {
            var category = Service.Access.Admin.SelectCategoryById(id);

            if (!isUnauthorized(category))
                Service.Access.Admin.DisableCategory(category);
            //else
            //    category = null;

            // TODO: implement messages on page head
            //var message = category == null
            //    ? PlainText.Dictionary["CategoryNotFound"]
            //    : String.Format(PlainText.Dictionary["CategoryDisabled"], category.Name);

            return RedirectToAction("Index");
        }



        public ActionResult Enable(Int32 id)
        {
            var category =  Service.Access.Admin.SelectCategoryById(id);

            if (!isUnauthorized(category))
                Service.Access.Admin.EnableCategory(category);
            //else
            //    category = null;

            // TODO: implement messages on page head
            //var message = category == null
            //    ? PlainText.Dictionary["CategoryNotFound"]
            //    : String.Format(PlainText.Dictionary["CategoryEnabled"], category.Name);

            return RedirectToAction("Index");
        }



        private static Boolean isUnauthorized(Category category)
        {
            return category == null
                   || !category.AuthorizeCRUD(Current.User);
        }

    }
}
