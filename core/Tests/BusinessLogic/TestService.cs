using System;
using DFM.BusinessLogic.Repositories;
using DFM.BusinessLogic.Services;

namespace DFM.BusinessLogic.Tests
{
	public class TestService : Service
	{
		internal TestService(
			ServiceAccess serviceAccess, Repos repos
		) : base(serviceAccess, repos) { }

		public void Execute(Action action)
		{
			inTransaction("TESTS", action);
		}
	}
}
