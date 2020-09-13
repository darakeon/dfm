using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DFM.Generic;

namespace DFM.MVC.Areas.Api.Models
{
	public class LogModel
	{
		public Int32 Count { get; }
		public List<ErrorLog> Logs { get; }

		private readonly String newLogs;
		private readonly String readLogs;
		private readonly String[] files;

		public LogModel(Boolean list)
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
						.ToList();
				}
			}
			catch (Exception e)
			{
				Count = -1;

				if (list)
				{
					Logs.Add(new ErrorLog(e));
				}
			}
		}

		public void Archive(String id)
		{
			if (id == null) return;

			var move = files.FirstOrDefault(
				f => f.EndsWith($"{id}.log")
			);

			if (move == null) return;

			File.Move(move, move.Replace(newLogs, readLogs));
		}
	}
}
