using System;

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
		}

		public String ClassName { get; set; }
		public String Message { get; set; }
		public String StackTrace { get; set; }
		public String Source { get; set; }

		public ExceptionData InnerException { get; set; }

		public override Boolean Equals(object obj)
		{
			return obj?.GetHashCode() == GetHashCode();
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(
				ClassName,
				Message,
				StackTrace,
				Source,
				InnerException
			);
		}
	}
}
