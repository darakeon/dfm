using System;
using DFM.Entities.Enums;
using DFM.MVC.Helpers.Authorize;
using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Helpers.Extensions;
using DFM.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace DFM.MVC.Controllers
{
	[Auth]
	public class CategoriesController : BaseController
	{
		[HttpGetAndHead]
		public IActionResult Index()
		{
			var model = new CategoriesIndexModel();

			return View(model);
		}

		[HttpGetAndHead]
		public IActionResult Create()
		{
			var model = new CategoriesCreateEditModel();

			return View("CreateEdit", model);
		}

		[HttpPost, ValidateAntiForgeryToken]
		public IActionResult Create(CategoriesCreateEditModel model)
		{
			model.Type = OperationType.Creation;

			return createEditForHtmlForm(model);
		}

		[HttpGetAndHead]
		public IActionResult Edit(String id)
		{
			if (String.IsNullOrEmpty(id))
				return RedirectToAction("Create");

			var model = new CategoriesCreateEditModel(id);

			return View("CreateEdit", model);
		}

		[HttpPost, ValidateAntiForgeryToken]
		public IActionResult Edit(String id, CategoriesCreateEditModel model)
		{
			model.Type = OperationType.Edition;
			return createEditForHtmlForm(model);
		}

		private IActionResult createEditForHtmlForm(CategoriesCreateEditModel model)
		{
			if (ModelState.IsValid)
			{
				var error = model.CreateEdit();

				if (error != null)
					ModelState.AddModelError("Category.Name", HttpContext.Translate(error));
			}

			if (ModelState.IsValid)
			{
				return RedirectToAction("Index");
			}

			return View("CreateEdit", model);
		}

		[HttpPost, ValidateAntiForgeryToken]
		public IActionResult Disable(String id)
		{
			var model = new AdminModel();

			model.DisableCategory(id);

			return RedirectToAction("Index");
		}

		[HttpPost, ValidateAntiForgeryToken]
		public IActionResult Enable(String id)
		{
			var model = new AdminModel();

			model.EnableCategory(id);

			return RedirectToAction("Index");
		}
	}
}
