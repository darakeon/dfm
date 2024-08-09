using System;
using System.Threading.Tasks;
using DFM.Entities.Enums;
using DFM.MVC.Helpers.Authorize;
using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace DFM.MVC.Controllers;

public class ArchivesController : BaseController
{
	[HttpGetAndHead]
	public IActionResult Index()
	{
		var model = new ArchivesIndexModel();

		return View(model);
	}

	[HttpGetAndHead]
	public IActionResult Upload()
	{
		var model = new ArchivesUploadModel();

		return View(model);
	}

	[HttpPost, ValidateAntiForgeryToken]
	public async Task<IActionResult> Upload(ArchivesUploadModel model)
	{
		if (ModelState.IsValid)
			addErrors(await model.SaveArchive());

		if (!ModelState.IsValid)
			return View(model);

		return RedirectToAction("Index", "Archives");
	}

	[HttpGetAndHead]
	public IActionResult Lines(Guid id)
	{
		var model = new ArchivesLinesModel(id);

		return View(model);
	}

	[JsonAuth, HttpPost, ValidateAntiForgeryToken, Wizard.Avoid]
	public IActionResult RetryLine(Guid id, Int16 position)
	{
		var model = new ArchivesLinesModel(id, false);

		model.Retry(position);

		return PartialView("ImportStatus", ImportStatus.Pending);
	}
}
