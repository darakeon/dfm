using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Generic;

namespace DFM.BusinessLogic.Exceptions
{
	public class CoreError : SystemError, IDisposable
	{
		public Error Type => Types.First().Error;
		public List<ErrorWithMetadata> Types { get; }

		internal CoreError()
			: base("Multiple")
		{
			Types = new();
		}

		internal CoreError(Error type, Exception inner = null)
			: base(type.ToString(), inner)
		{
			Types = new() { new ErrorWithMetadata(type) };
		}

		internal void AddError(Error error, object metadata = null)
		{
			Types.Add(new ErrorWithMetadata(error, metadata));
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
