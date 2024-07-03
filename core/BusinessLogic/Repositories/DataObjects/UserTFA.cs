using System;
using DFM.Entities;

namespace DFM.BusinessLogic.Repositories.DataObjects
{
	class UserTFA
	{
		public UserTFA(User user, Boolean usedTFAPassword)
		{
			User = user;
			UsedTFAPassword = usedTFAPassword;
		}

		public User User { get; }
		public Boolean UsedTFAPassword { get; }
	}
}
