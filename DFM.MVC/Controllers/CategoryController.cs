using System;
using System.Web.Mvc;
using Ak.MVC.Authentication;
using DFM.MVC.Areas.Accounts.Models;
using DFM.MVC.Authentication;
using DFM.Core.Database;

namespace DFM.MVC.Areas.Accounts.Controllers
{
    [AjaxAuthorize]
    public class CategoryController : Controller
    {
        readonly CategoryData categoryData = new CategoryData();


        // It's called on page, by Html.Action.
        // This causes the call method depend on the call method of mother page
        // So, it has to have different names for Get and Post
        public ActionResult CreateClean()
        {
            var model = new CategoryCreateModel();

            return View("Create", model);
        }

        [HttpPost]
        public JsonResult Create(CategoryCreateModel model)
        {
            var result = new JsonResult();

            var categoryIdEmpty = model.Category == null
                || String.IsNullOrEmpty(model.Category.Name);


            if (categoryIdEmpty)
            {
                result.Data = new { id = 0 };
            }
            else
            {
                var category = model.Category;

                Current.User.AddCategory(category);

                categoryData.SaveOrUpdate(category);

                result.Data = new { id = category.ID, name = category.Name };
            }


            return result;
        }
    }
}
