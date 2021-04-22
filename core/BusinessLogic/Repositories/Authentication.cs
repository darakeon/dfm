using System;
using DFM.Entities;

namespace DFM.BusinessLogic.Repositories
{
	class Authentication
	{
		public Authentication(User user, Boolean usedTFAPassword)
		{
			User = user;
			UsedTFAPassword = usedTFAPassword;
		}

		public User User { get; }
		public Boolean UsedTFAPassword { get; }
	}
}
