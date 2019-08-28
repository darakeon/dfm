using System;
using DFM.Generic;

namespace DFM.BusinessLogic.Repositories
{
	public class DFMRepositoryException : DFMException
	{
		public DFMRepositoryException(String message)
			: base(message) { }
	}
}
