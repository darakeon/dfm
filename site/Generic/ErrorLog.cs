using System;
using Newtonsoft.Json;

namespace DFM.Generic
{
	public class ErrorLog
	{
		public String ID { get; set; }
		public DateTime Date { get; set; }
		public ExceptionData Exception { get; set; }

		// this is for serialization
		public ErrorLog() { }

		public ErrorLog(Exception exception)
		{
			Date = DateTime.Now;
			ID = Date.ToString("yyyyMMddHHmmssffffff");
			Exception = new ExceptionData(exception);
		}

		public static ErrorLog FromJson(String json)
		{
			return JsonConvert.DeserializeObject<ErrorLog>(json);
		}

		public override String ToString()
		{
			return JsonConvert.SerializeObject(
				this,
				Formatting.Indented
			);
		}

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
		}
	}
}
