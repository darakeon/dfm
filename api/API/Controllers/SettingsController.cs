using DFM.API.Helpers.Authorize;
using DFM.API.Helpers.Controllers;
using DFM.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DFM.API.Controllers;

public class SettingsController : BaseApiController
{
	[HttpGetAndHead, Auth]
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