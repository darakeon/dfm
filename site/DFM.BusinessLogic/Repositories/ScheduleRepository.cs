using System;
using System.Collections.Generic;
using System.Linq;
using DFM.BusinessLogic.Bases;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Helpers;
using DFM.Entities;
using DFM.Entities.Extensions;

namespace DFM.BusinessLogic.Repositories
{
	internal class ScheduleRepository : GenericMoveRepository<Schedule>
	{
		internal Schedule Save(Schedule schedule)
		{
			return SaveOrUpdate(schedule, complete, validate);
		}

		private static void complete(Schedule schedule)
		{
			Complete(schedule);

			if (schedule.ID == 0)
			{
				schedule.Active = true;
			}
		}

		private void validate(Schedule schedule)
		{
			var now = schedule.User.Now();
			Validate(
				schedule,
				now,
				MaxLen.Schedule_Description,
				Error.TooLargeScheduleDescription,
				schedule.Active
			);

			if (!schedule.Boundless && schedule.Times <= 0)
				throw Error.ScheduleTimesCantBeZero.Throw();
		}



		internal IList<Schedule> GetRunnable(User user, Boolean hasCategory)
		{
			return SimpleFilter(s => s.User == user && s.Active)
				.Where(
					s => s.HasCategory() == hasCategory
						&& s.CanRunNow()
				)
				.ToList();
		}



		internal void Disable(Int64 id, User loggedInUser)
		{
			var schedule = Get(id);

			if (schedule == null || schedule.User.ID != loggedInUser.ID)
				throw Error.InvalidSchedule.Throw();

			if (!schedule.Active)
				throw Error.DisabledSchedule.Throw();

			schedule.Active = false;
			Save(schedule);
		}


		public void DisableAll(Account account)
		{
			var scheduleList = SimpleFilter(
				s => s.Active
				&& (s.In.ID == account.ID
					|| s.Out.ID == account.ID)
			);

			foreach (var schedule in scheduleList)
			{
				schedule.Active = false;
				Save(schedule);
			}
		}


		public void DeleteAll(Account account)
		{
			var scheduleList = SimpleFilter(
				s => s.In.ID == account.ID
				|| s.Out.ID == account.ID
			);

			foreach (var schedule in scheduleList)
			{
				if (schedule.Active)
				{
					throw Error.CantDeleteAccountWithSchedules.Throw();
				}

				Delete(schedule);
			}
		}

		internal override User GetUser(Schedule entity)
		{
			return entity.User;
		}
	}
}
