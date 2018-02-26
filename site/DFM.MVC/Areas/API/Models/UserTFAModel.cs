using System;

namespace DFM.MVC.Areas.API.Models
{
	public class UserTFAModel : BaseApiModel
	{
		public String Code { get; set; }

		internal void Validate()
		{
			Safe.ValidateTicketTFA(Code);
		}
	}
}