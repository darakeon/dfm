using DFM.API.Helpers.Authorize;
using DFM.API.Models;
using DFM.API.Starters.Routes;
using Microsoft.AspNetCore.Mvc;

namespace DFM.API.Controllers;

[Route(Apis.Main.ObjectPath)]
[Route(Apis.Object.ObjectPath)]
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