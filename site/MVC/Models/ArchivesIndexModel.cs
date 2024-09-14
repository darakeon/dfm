using System;
using System.Collections.Generic;
using DFM.BusinessLogic.Response;

namespace DFM.MVC.Models;

public class ArchivesIndexModel : BaseSiteModel
{
	public ArchivesIndexModel()
	{
		ArchiveList = attendant.GetArchiveList();
	}

	public IList<ArchiveInfo> ArchiveList { get; set; }

	public void Cancel(Guid id)
	{
		var archive = attendant.CancelArchive(id);
		Archive = new ArchiveRowModel(archive);
	}

	public ArchiveRowModel Archive { get; private set; }
}
