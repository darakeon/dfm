using System;
using DFM.BusinessLogic.Response;
using DFM.MVC.Helpers.Views;

namespace DFM.MVC.Models;

public class LineRowModel(
	Guid archiveGuid,
	LineInfo line,
	String language,
	WizardHL wizardHL = null
)
{
	public Guid ArchiveGuid { get; } = archiveGuid;
	public LineInfo Line { get; } = line;
	public String Language { get; } = language;

	# nullable enable
	public WizardHL? WizardHL { get; } = wizardHL;
}