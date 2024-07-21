using System;
using DFM.Entities;

namespace DFM.BusinessLogic.Repositories;

internal class ArchiveRepository : Repo<Archive>
{
	internal Archive Get(Guid guid)
	{
		return SingleOrDefault(
			m => m.ExternalId == guid.ToByteArray()
		);
	}
}
