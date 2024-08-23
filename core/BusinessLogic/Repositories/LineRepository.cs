using System;
using System.Collections.Generic;
using DFM.BusinessLogic.Repositories.DataObjects;
using DFM.BusinessLogic.Response;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Generic;

namespace DFM.BusinessLogic.Repositories;

internal class LineRepository : Repo<Line>
{
	public IList<ArchiveLineStati> GetArchivesPending()
	{
		return NewQuery()
			.LeftJoin(l => l.Archive)
			.Where(l => l.Archive.Status == ImportStatus.Pending)
			.TransformResult<ArchiveLineStati>()
			.GroupBy(l => l.Archive, als => als.Archive)
			.Max(l => l.Status, als => als.MaxLineStati)
			.Min(l => l.Status, als => als.MinLineStati)
			.List;
	}

	public IList<ArchiveInfo> GetArchives(User user)
	{
		return NewQuery()
			.LeftJoin(l => l.Archive)
			.Where(l => l.Archive.User == user)
			.OrderBy(l => l.Archive, false)
			.TransformResult<ArchiveInfo>()
			.GroupBy(l => l.Archive, ai => ai.Archive)
			.Count(l => l.ID, ai => ai.LineCount)
			.List;
	}

	public Line Get(Guid archiveGuid, Int16 linePosition)
	{
		return NewQuery()
			.LeftJoin(l => l.Archive)
			.Where(
				l => l.Archive.ExternalId == archiveGuid.ToByteArray()
					&& l.Position == linePosition
			)
			.SingleOrDefault;
	}

	public IList<Line> GetToRequeue()
	{
		return NewQuery()
			.Where(
				l => l.Status == ImportStatus.Pending
				     && l.Scheduled < DateTime.Now.AddDays(-1)
			)
			.Take(Cfg.SQS.QueueLimit)
			.List;
	}
}
