﻿namespace DFM.API.Models
{
	public class UserWipeModel : BaseApiModel
	{
		public string Password { get; set; }

		internal void AskWipe()
		{
			attendant.AskWipe(Password);
		}
	}
}
