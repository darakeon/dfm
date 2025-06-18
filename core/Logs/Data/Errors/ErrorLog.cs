﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DFM.Generic;
using DFM.Generic.Datetime;
using Newtonsoft.Json;

namespace DFM.Logs.Data.Errors
{
	internal class ErrorLog
	{
		public String ID { get; set; }
		public DateTime Date { get; set; }
		public Boolean Handled { get; set; }
		public ExceptionData Exception { get; set; }
		public Int32 Count { get; set; }
		public Int32 Hash => GetHashCode();
		private IList<ErrorLog> children;

		// this is for serialization
		public ErrorLog() { }

		public ErrorLog(Exception exception)
			: this(exception, false) { }

		public ErrorLog(Exception exception, String handledMessage)
			: this(new SystemError(handledMessage, exception), true) { }

		private ErrorLog(Exception exception, Boolean handled)
		{
			Date = DateTime.UtcNow;
			ID = Date.UntilMicrosecond();
			Exception = new ExceptionData(exception);
			Handled = handled;
		}

		public void Archive(String newLogs, String readLogs)
		{
			foreach (var child in children)
			{
				var file = Path.Combine(newLogs, $"{child.ID}.log");

				if (!File.Exists(file)) return;

				File.Move(file, file.Replace(newLogs, readLogs));
			}
		}

		public static ErrorLog FromJson(String json)
		{
			return JsonConvert.DeserializeObject<ErrorLog>(json);
		}

		public static ErrorLog Combine(IGrouping<ErrorLog, ErrorLog> list)
		{
			var log = list.First();
			log.Count = list.Count();
			log.children = list.ToList();
			return log;
		}

		public override String ToString()
		{
			return JsonConvert.SerializeObject(
				this,
				Formatting.Indented
			);
		}

		public override Boolean Equals(Object obj)
		{
			return obj is ErrorLog log
				&& log.Exception.Equals(Exception);
		}

		// ReSharper disable once NonReadonlyMemberInGetHashCode
		public override Int32 GetHashCode()
		{
			return Exception.GetHashCode();
		}
	}
}
