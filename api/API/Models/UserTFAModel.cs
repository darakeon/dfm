using System;

namespace DFM.API.Models
{
	public class UserTFAModel : BaseApiModel
	{
		public String Code { get; set; }

		internal void Validate()
		{
			auth.ValidateTicketTFA(Code);
		}
	}
}
