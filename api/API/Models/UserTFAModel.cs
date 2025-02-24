using System;

namespace DFM.API.Models
{
	public class UserTFAModel : BaseApiModel
	{
		public PatchTFAOperation Operation { get; set; } = PatchTFAOperation.Validate;

		public String? Code { get; set; }
		public String? Password { get; set; }

		internal void Execute()
		{
			if (Operation == PatchTFAOperation.Validate)
				auth.ValidateTicketTFA(Code);
			else
				auth.AskRemoveTFA(Password);
		}

		public enum PatchTFAOperation
		{
			Validate = 0,
			AskRemove = 1,
		}
	}
}
