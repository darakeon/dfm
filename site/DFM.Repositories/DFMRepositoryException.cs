using System;
using DFM.Generic;

namespace DFM.Repositories
{
	public class DFMRepositoryException : DFMException
	{
		public DFMRepositoryException(String message)
			: base(message) { }
	}
}
