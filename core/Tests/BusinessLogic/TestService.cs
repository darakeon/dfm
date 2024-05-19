using System;
using DFM.BusinessLogic.Repositories;
using DFM.BusinessLogic.Services;
using DFM.BusinessLogic.Validators;

namespace DFM.BusinessLogic.Tests
{
	public class TestService : Service
	{
		internal TestService(
			ServiceAccess serviceAccess, Repos repos, Valids valids
		) : base(serviceAccess, repos, valids) { }

		public void Execute(Action action)
		{
			inTransaction("TESTS", action);
		}
	}
}
