using System;
using DFM.API.Helpers.Authorize;
using DFM.API.Helpers.Controllers;
using DFM.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DFM.API.Controllers
{
    public class UsersController : BaseApiController
    {
	    [HttpPost]
	    public IActionResult Login(String email, String password)
	    {
		    var model =
			    new UsersLoginModel
			    {
				    Email = email,
				    Password = password,
			    };

		    return json(() => new { ticket = model.LogOn() });
	    }

		[HttpPost]
		public IActionResult SignUp([FromBody] UsersSignUpModel model)
		{
			return json(model.SignUp);
		}

        [HttpPost]
        public IActionResult Logout()
        {
            return json(() =>
            {
                new SafeModel().LogOff();
            });
        }

        [HttpGetAndHead, Auth]
        public IActionResult GetSettings()
        {
            return json(() => new UserSettingsModel());
        }

        [HttpPost, Auth]
        public IActionResult SaveSettings()
        {
            var model = getFromBody<UserSettingsModel>();
            return json(model.Save);
        }

        [HttpPost, Auth(AuthParams.IgnoreTFA)]
        public IActionResult TFA(String code)
        {
            var model = new UserTFAModel(code);
            return json(model.Validate);
        }

        [HttpPost, Auth]
        public IActionResult Wipe()
        {
            var model = getFromBody<UserWipeModel>();
            return json(model.AskWipe);
        }

        [HttpGetAndHead]
        public IActionResult Terms()
        {
            return json(() => new UserTermsModel());
        }
    }
}
