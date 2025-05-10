using DFM.BusinessLogic.Response;
using DFM.MVC.Models.Views;

namespace DFM.MVC.Models;

public class ArchiveRowModel(
	ArchiveInfo archive,
	WizardHL wizardHL = null
)
{
	public ArchiveInfo Archive { get; } = archive;

	# nullable enable
	public WizardHL? WizardHL { get; } = wizardHL;
}