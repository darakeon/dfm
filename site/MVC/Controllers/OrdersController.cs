using System;
using DFM.MVC.Authorize;
using DFM.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace DFM.MVC.Controllers;

public class OrdersController : BaseController
{
	[HttpGetAndHead]
	public IActionResult Index()
	{
		var model = new OrdersIndexModel();

		return View(model);
	}


	[JsonAuth, HttpPost, ValidateAntiForgeryToken, Wizard.Avoid]
	public IActionResult Cancel(Guid id)
	{
		var model = new OrdersIndexModel();

		model.Cancel(id);

		return PartialView("OrderRow", model.Order);
	}


	[JsonAuth, HttpPost, ValidateAntiForgeryToken, Wizard.Avoid]
	public IActionResult Retry(Guid id)
	{
		var model = new OrdersIndexModel();

		model.Retry(id);

		return PartialView("OrderRow", model.Order);
	}


	[HttpGet, Wizard.Avoid]
	public IActionResult Download(Guid id)
	{
		var model = new OrdersIndexModel();

		model.Download(id);

		return File(model.FileContent, "text/csv", model.FileName);
	}


	[HttpGetAndHead]
	public IActionResult Create()
	{
		var model = new OrdersCreateModel();

		return View(model);
	}

	[HttpPost, ValidateAntiForgeryToken]
	public IActionResult Create(OrdersCreateModel model)
	{
		if (ModelState.IsValid)
			addErrors(model.SaveOrder());

		if (!ModelState.IsValid)
			return View(model);

		return RedirectToAction("Index", "Orders");
	}
}
