using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Generic;

namespace DFM.BusinessLogic.Exceptions
{
	public class CoreError : SystemError, IDisposable
	{
		public Error Type => Types.First();
		public List<Error> Types { get; }

		internal CoreError()
			: base("Multiple")
		{
			Types = new();
		}

		internal CoreError(Error type, Exception inner = null)
			: base(type.ToString(), inner)
		{
			Types = new() { type };
		}

		internal void AddError(Error error)
		{
			Types.Add(error);
		}


		public void Dispose()
		{
			if (Types.Any())
			{
				throw this;
			}
		}
	}
}
