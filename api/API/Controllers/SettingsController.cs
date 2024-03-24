using DFM.API.Helpers.Authorize;
using DFM.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DFM.API.Controllers;

public class SettingsController : BaseApiController
{
	[HttpGet, Auth]
	public IActionResult Index()
	{
		return json(() => new UserSettingsModel());
	}

	[HttpPatch, Auth]
	public IActionResult Index([FromBody] UserSettingsModel model)
	{
		return json(model.Save);
	}
}