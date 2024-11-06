using System;
using DFM.BusinessLogic.Exceptions;
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

	public void ValidatePlanLimit(User user)
	{
		var count = Count(
			a => a.User == user
				&& a.Uploaded >= firstDayThisMonth
				&& a.Uploaded <= lastDayThisMonth
		);

		if (count >= user.Control.Plan.ArchiveMonthUpload)
			throw Error.PlanLimitArchiveMonthUploadAchieved.Throw();
	}
}
