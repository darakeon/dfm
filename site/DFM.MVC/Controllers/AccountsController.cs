using System;
using System.Web.Mvc;
using DFM.Entities.Enums;
using DFM.MVC.Helpers.Authorize;
using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Models;

namespace DFM.MVC.Controllers
{
	[Auth]
	public class AccountsController : BaseController
	{
		[HttpGet]
		public ActionResult Index()
		{
			var model = new AccountsIndexModel();

			return View(model);
		}

		[HttpGet]
		public ActionResult ListClosed()
		{
			var model = new AccountsIndexModel(false);

			return View(model);
		}

		[HttpGet]
		public ActionResult Create()
		{
			var model = new AccountsCreateEditModel(OperationType.Creation);

			return View("CreateEdit", model);
		}

		[HttpPost, ValidateAntiForgeryToken]
		public ActionResult Create(AccountsCreateEditModel model)
		{
			model.Type = OperationType.Creation;

			return createEdit(model);
		}

		[HttpGet]
		public ActionResult Edit(String id)
		{
			if (String.IsNullOrEmpty(id))
				return RedirectToAction("Create");

			var model = new AccountsCreateEditModel(OperationType.Edition, id);

			if (model.Account == null)
				return RedirectToAction("Create");

			return View("CreateEdit", model);
		}

		[HttpPost, ValidateAntiForgeryToken]
		public ActionResult Edit(String id, AccountsCreateEditModel model)
		{
			return createEdit(model);
		}

		private ActionResult createEdit(AccountsCreateEditModel model)
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

		[HttpPost, ValidateAntiForgeryToken]
		public ActionResult Close(String id)
		{
			var model = new AdminModel();

			model.CloseAccount(id);

			return RedirectToAction("Index");
		}

		[HttpPost, ValidateAntiForgeryToken]
		public ActionResult Delete(String id)
		{
			var model = new AdminModel();

			model.Delete(id);

			return RedirectToAction("Index");
		}
	}
}
