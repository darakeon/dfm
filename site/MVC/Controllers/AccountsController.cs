using System;
using DFM.Entities.Enums;
using DFM.MVC.Helpers.Authorize;
using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace DFM.MVC.Controllers
{
	[Auth]
	public class AccountsController : BaseController
	{
		[HttpGetAndHead]
		public IActionResult Index()
		{
			var model = new AccountsIndexModel();

			return View(model);
		}

		[HttpGetAndHead]
		public IActionResult ListClosed()
		{
			var model = new AccountsIndexModel(false);

			return View(model);
		}

		[HttpGetAndHead]
		public IActionResult Create()
		{
			var model = new AccountsCreateEditModel();

			return View("CreateEdit", model);
		}

		[HttpPost, ValidateAntiForgeryToken]
		public IActionResult Create(AccountsCreateEditModel model)
		{
			model.Type = OperationType.Creation;

			return createEdit(model);
		}

		[HttpGetAndHead]
		public IActionResult Edit(String id)
		{
			if (String.IsNullOrEmpty(id))
				return RedirectToAction("Create");

			var model = new AccountsCreateEditModel(id);

			if (model.Account == null)
				return RedirectToAction("Create");

			return View("CreateEdit", model);
		}

		[HttpPost, ValidateAntiForgeryToken]
		public IActionResult Edit(String id, AccountsCreateEditModel model)
		{
			return createEdit(model);
		}

		private IActionResult createEdit(AccountsCreateEditModel model)
		{
			if (ModelState.IsValid)
			{
				var errors = model.CreateOrUpdate();

				addErrors(errors);
			}

			if (ModelState.IsValid)
				return RedirectToAction("Index");

			return View("CreateEdit", model);
		}

		[HttpPost, ValidateAntiForgeryToken, NoWizard]
		public IActionResult Close(String id)
		{
			var model = new AdminModel();

			model.CloseAccount(id);

			return RedirectToAction("Index");
		}

		[HttpPost, ValidateAntiForgeryToken, NoWizard]
		public IActionResult Delete(String id)
		{
			var model = new AdminModel();

			model.DeleteAccount(id);

			return RedirectToAction("Index");
		}

		[HttpPost, ValidateAntiForgeryToken, NoWizard]
		public IActionResult Reopen(String id)
		{
			var model = new AdminModel();

			model.ReopenAccount(id);

			return RedirectToAction("Index");
		}
	}
}
