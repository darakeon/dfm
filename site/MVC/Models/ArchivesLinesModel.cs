using System;
using System.Collections.Generic;
using DFM.BusinessLogic.Response;

namespace DFM.MVC.Models;

public class ArchivesLinesModel : BaseSiteModel
{
    public ArchivesLinesModel(Guid guid, Boolean loadArchive = true)
    {
	    ArchiveGuid = guid;

	    if (loadArchive)
	    {
		    var archiveInfo = service.Robot.GetLineList(guid);
		    ArchiveName = archiveInfo.Filename;
		    LineList = archiveInfo.LineList;
	    }
	    else
	    {
		    LineList = new List<LineInfo>();
	    }
    }

    public Guid ArchiveGuid { get; set; }
    public String ArchiveName { get; set; }
    public LineRowModel Line { get; set; }
    public IList<LineInfo> LineList { get; set; }

    public void Retry(Int16 position)
    {
		var line = service.Robot.RetryLine(ArchiveGuid, position);
		Line = new LineRowModel(ArchiveGuid, line, Language);
    }
}
