using System;
using System.ComponentModel.DataAnnotations;

namespace DFM.MVC.Areas.Api.Models
{
	internal class UsersLoginModel : BaseApiModel
	{
		[Required(ErrorMessage = "*")]
		public String Email { get; set; }

		[Required(ErrorMessage = "*")]
		public String Password { get; set; }

		public Boolean RememberMe { get; set; }

		internal String LogOn()
		{
			return login(Email, Password, RememberMe);
		}
	}
}
