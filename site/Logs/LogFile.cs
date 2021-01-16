using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DFM.Generic;

namespace DfM.Logs
{
	public class LogFile
	{
		public Int32 Count { get; }
		public List<ErrorLog> Logs { get; }

		private readonly String newLogs;
		private readonly String readLogs;
		private readonly String[] files;

		public LogFile(Boolean list)
		{
			Logs = new List<ErrorLog>();

			try
			{
				newLogs = new FileInfo(Cfg.LogPathErrors).DirectoryName;
				readLogs = Path.Combine(newLogs, "read");

				if (newLogs == null)
					throw new Exception("No log path configured");

				if (!Directory.Exists(newLogs))
					Directory.CreateDirectory(newLogs);

				if (!Directory.Exists(readLogs))
					Directory.CreateDirectory(readLogs);

				files = Directory.GetFiles(newLogs, "*.log");

				Count = files.Length;

				if (list)
				{
					Logs = files
						.Select(File.ReadAllText)
						.Select(ErrorLog.FromJson)
						.GroupBy(l => l)
						.Select(ErrorLog.Combine)
						.ToList();
				}
			}
			catch (Exception e)
			{
				Count = -1;

				if (list)
				{
					Logs.Add(new ErrorLog(e, false));
				}
			}
		}

		public void Archive(Int32 id)
		{
			Logs.FirstOrDefault(l => l.Hash == id)
				?.Archive(newLogs, readLogs);
		}
	}
}
