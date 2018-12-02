using System;
using System.Web.Mvc;
using DFM.Entities.Enums;
using DFM.MVC.Helpers.Authorize;
using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Helpers.Global;
using DFM.MVC.Models;

namespace DFM.MVC.Controllers
{
	[DFMAuthorize]
	public class CategoriesController : BaseController
	{
		public ActionResult Index()
		{
			var model = new CategoriesIndexModel();

			return View(model);
		}



		public ActionResult Create()
		{
			var model = new CategoriesCreateEditModel(OperationType.Creation);

			return View("CreateEdit", model);
		}

		[HttpPost]
		public ActionResult Create(CategoriesCreateEditModel model)
		{
			model.Type = OperationType.Creation;

			return createEditForHtmlForm(model);
		}


		public ActionResult Edit(String id)
		{
			if (String.IsNullOrEmpty(id))
				return RedirectToAction("Create");

			var model = new CategoriesCreateEditModel(OperationType.Edition, id);

			return View("CreateEdit", model);
		}

		[HttpPost]
		public ActionResult Edit(String id, CategoriesCreateEditModel model)
		{
			model.Type = OperationType.Edition;
			model.Category.Name = id;

			return createEditForHtmlForm(model);
		}



		private ActionResult createEditForHtmlForm(CategoriesCreateEditModel model)
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
