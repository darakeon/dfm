using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DFM.Generic;

namespace DFM.MVC.Areas.Api.Models
{
	public class StatusLogModel
	{
		public List<ErrorLog> Logs { get; }

		private readonly String newLogs;
		private readonly String readLogs;

		public StatusLogModel(String id)
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

				var files = Directory.GetFiles(newLogs);

				files = archiveLog(files, id);

				Logs = files
					.Select(File.ReadAllText)
					.Select(ErrorLog.FromJson)
					.ToList();
			}
			catch (Exception e)
			{
				Logs.Add(new ErrorLog(e));
			}
		}

		private String[] archiveLog(String[] files, String id)
		{
			if (id == null) return files;

			var move = files.FirstOrDefault(
				f => f.EndsWith($"{id}.log")
			);

			if (move == null) return files;

			File.Move(move, move.Replace(newLogs, readLogs));

			return files.Where(
				f => !f.EndsWith($"{id}.log")
			).ToArray();
		}
	}
}
