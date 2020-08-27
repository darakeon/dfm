using System;
using Newtonsoft.Json;

namespace DFM.Generic
{
	public class ErrorLog
	{
		public String ID { get; set; }
		public DateTime Date { get; set; }
		public Exception Exception { get; set; }

		public ErrorLog(Exception exception)
		{
			Date = DateTime.Now;
			ID = Date.ToString("yyyyMMddHHmmssffffff");
			Exception = exception;
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
	}
}
