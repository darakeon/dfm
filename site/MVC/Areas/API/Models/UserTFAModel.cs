using System;

namespace DFM.MVC.Areas.Api.Models
{
	public class UserTFAModel : BaseApiModel
	{
		public UserTFAModel(String code)
		{
			Code = code;
		}

		public String Code { get; }

		internal void Validate()
		{
			auth.ValidateTicketTFA(Code);
		}
	}
}
