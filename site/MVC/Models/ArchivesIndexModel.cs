using System.Collections.Generic;
using DFM.BusinessLogic.Response;

namespace DFM.MVC.Models;

public class ArchivesIndexModel : BaseSiteModel
{
	public IList<ArchiveInfo> ArchiveList { get; set; }
		= service.Robot.GetArchiveList();
}
