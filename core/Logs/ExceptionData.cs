using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DfM.Logs
{
	public class ExceptionData
	{
		// ReSharper disable once UnusedMember.Global
		// this is for serialization
		public ExceptionData() { }

		public ExceptionData(Exception exception)
		{
			ClassName = exception.GetType().FullName;
			Message = exception.Message;
			StackTrace = exception.StackTrace;
			Source = exception.Source;

			if (exception.InnerException != null)
			{
				InnerException = new ExceptionData(
					exception.InnerException
				);
			}

			var prop = exception.GetType()
				.GetProperty("ValidationErrors");

			if (prop != null)
			{
				Details = prop.GetValue(exception)
					as ReadOnlyCollection<String>;
			}
		}

		public String ClassName { get; set; }
		public String Message { get; set; }
		public String StackTrace { get; set; }
		public String Source { get; set; }
		public IList<String> Details { get; set; }

		public ExceptionData InnerException { get; set; }

		public override Boolean Equals(Object obj)
		{
			return obj?.GetHashCode() == GetHashCode();
		}

		public override Int32 GetHashCode()
		{
			return HashCode.Combine(
				ClassName,
				StackTrace,
				Source,
				InnerException,
				Details
			);
		}
	}
}
