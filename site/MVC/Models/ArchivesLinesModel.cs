using System;
using System.Collections.Generic;
using DFM.BusinessLogic.Response;

namespace DFM.MVC.Models;

public class ArchivesLinesModel : BaseSiteModel
{
    public ArchivesLinesModel(Guid guid, Boolean loadArchive = true)
    {
        ArchiveGuid = guid;

        LineList = loadArchive
	        ? service.Robot.GetLineList(guid).LineList
	        : new List<LineInfo>();
    }

    public IList<LineInfo> LineList { get; set; }
    public Guid ArchiveGuid { get; set; }

    public void Retry(Int16 position)
    {
        service.Robot.RetryLine(ArchiveGuid, position);
    }
}
