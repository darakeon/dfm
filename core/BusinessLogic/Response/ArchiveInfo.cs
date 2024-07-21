using System;
using DFM.Entities;
using DFM.Entities.Enums;

namespace DFM.BusinessLogic.Response;

public class ArchiveInfo
{
	public Guid Guid { get; set; }
	public String Filename { get; set; }
	public ImportStatus Status { get; set; }

	public Int64 LineCount { get; set; }

	public Archive Archive
	{
		get => new()
		{
			Guid = Guid,
			Filename = Filename,
			Status = Status,
		};
		set
		{
			Guid = value.Guid;
			Filename = value.Filename;
			Status = value.Status;
		}
	}
}
