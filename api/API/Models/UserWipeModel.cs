using System;

namespace DFM.API.Models
{
	public class UserWipeModel : BaseApiModel
	{
		public String Password { get; set; }

		internal void AskWipe()
		{
			attendant.AskWipe(Password);
		}
	}
}
