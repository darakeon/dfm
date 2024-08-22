using System;
using DFM.BusinessLogic.Response;
using DFM.MVC.Helpers.Views;

namespace DFM.MVC.Models;

public class LineRowModel
{
	public LineRowModel(
		Guid archiveGuid,
		LineInfo line,
		String language
	)
	{
		ArchiveGuid = archiveGuid;
		Line = line;
		Language = language;
	}

	public Guid ArchiveGuid { get; }
	public LineInfo Line { get; }
	public String Language { get; }

	# nullable enable
	public WizardHL? WizardHL { get; set; }
}