﻿using System;
using System.Collections.Generic;
using DFM.Entities;
using DFM.Entities.Enums;

namespace DFM.BusinessLogic.Response;

public class ArchiveInfo
{
	public Guid Guid { get; set; }
	public String Filename { get; set; }
	public DateTime Uploaded { get; set; }
	public ImportStatus Status { get; set; }

	public Int64 LineCount { get; set; }

	public Archive Archive
	{
		get => new()
		{
			Guid = Guid,
			Filename = Filename,
			Uploaded = Uploaded,
			Status = Status,
		};
		set
		{
			Guid = value.Guid;
			Filename = value.Filename;
			Uploaded = value.Uploaded;
			Status = value.Status;
		}
	}

	public IList<LineInfo> LineList { get; set; } = new List<LineInfo>();
}
