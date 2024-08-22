using System;
using System.Collections.Generic;
using DFM.BusinessLogic.Response;

namespace DFM.MVC.Models;

public class ArchivesIndexModel : BaseSiteModel
{
	public IList<ArchiveInfo> ArchiveList { get; set; }
		= service.Robot.GetArchiveList();

	public void Cancel(Guid id)
	{
		var archive = service.Robot.CancelArchive(id);
		Archive = new ArchiveRowModel(archive);
	}

	public ArchiveRowModel Archive { get; private set; }
}
