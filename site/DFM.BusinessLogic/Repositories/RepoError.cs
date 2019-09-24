using System;
using DFM.Generic;

namespace DFM.BusinessLogic.Repositories
{
	public class RepoError : SystemError
	{
		public RepoError(String message)
			: base(message) { }
	}
}
