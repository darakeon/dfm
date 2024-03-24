using DFM.API.Helpers.Authorize;
using DFM.API.Models;
using DFM.BusinessLogic.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace DFM.API.Controllers
{
	public class UsersController : BaseApiController
	{
		[HttpPost]
		public IActionResult Login([FromBody] UsersLoginModel model)
		{
			model.LogOn();
			return json(() => model);
		}

		[HttpPost]
		public IActionResult SignUp([FromBody] UsersSignUpModel model)
		{
			return json(model.SignUp);
		}

		[HttpPatch]
		public IActionResult Logout()
		{
			return json(new SafeModel().LogOff);
		}

		[HttpPatch, Auth(AuthParams.IgnoreTFA)]
		public IActionResult TFA([FromBody] UserTFAModel model)
		{
			return json(model.Validate);
		}

		[HttpPatch, Auth]
		public IActionResult Wipe([FromBody] UserWipeModel model)
		{
			return json(model.AskWipe);
		}

		[HttpGet]
		public IActionResult Terms()
		{
			var model = new UserTermsModel();

			if (model.Content == null)
			{
				return error(Error.TermsNotFound);
			}

			return json(() => model);
		}
	}
}
