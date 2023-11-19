using System;

namespace DFM.MVC.Areas.Api.Models
{
	public class UserWipeModel : BaseApiModel
	{
		public String Password { get; set; }

		internal void AskWipe()
		{
			robot.AskWipe(Password);
		}
	}
}
