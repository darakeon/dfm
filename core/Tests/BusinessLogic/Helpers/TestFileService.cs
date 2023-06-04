using System;
using System.Collections.Generic;
using System.IO;
using DFM.Exchange;

namespace DFM.BusinessLogic.Tests.Helpers;

public class TestFileService : IFileService
{
	private readonly IDictionary<String, String> csvs
		= new Dictionary<String, String>();

	public String[] LastCsv { get; private set; }

	public void Upload(String path)
	{
		LastCsv = File.ReadAllLines(path);
		csvs.Add(path, File.ReadAllText(path));
	}

	public void Download(String path)
	{
		File.WriteAllText(path, csvs[path]);
	}
}
