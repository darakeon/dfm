using System;
using System.IO;
using System.Threading.Tasks;
using DFM.Generic;
using DFM.Logs.Data;

namespace DFM.Logs;

public class LocalLogService : BaseLogService, ILogService
{
	public LocalLogService()
	{
		if (!Cfg.Log.LocalFilled)
			throw new SystemError("Must have section Log whole configured for local files");
	}

	private protected override async Task saveLog(Division division, Object content)
	{
		var text = content.ToString();

		try
		{
			var path = pathFor(division);

			await File.WriteAllTextAsync(path, text);
		}
		catch (Exception e)
		{
			Console.WriteLine($"Error while recording log: {e}");
		}
		finally
		{
			Console.WriteLine($"Error that happened: {text?.Replace("\\n", "\n")}");
		}
	}

	private String pathFor(Division division)
	{
		return Path.Combine(Cfg.Log.Path, division.ToString());
	}
}
