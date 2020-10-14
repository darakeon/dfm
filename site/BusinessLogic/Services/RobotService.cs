using System;
using System.Collections.Generic;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Repositories;
using DFM.BusinessLogic.Response;
using DFM.Email;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Entities.Extensions;
using Error = DFM.BusinessLogic.Exceptions.Error;

namespace DFM.BusinessLogic.Services
{
	public class RobotService : Service
	{
		internal RobotService(ServiceAccess serviceAccess, Repos repos)
			: base(serviceAccess, repos) { }

		public EmailStatus RunSchedule()
		{
			parent.Safe.VerifyUser();

			try
			{
				var user = parent.Safe.GetCurrent();
				var scheduleList = repos.Schedule.GetRunnable(user);

				var result = scheduleList.Any()
					? runSchedule(scheduleList)
					: EmailStatus.EmailSent;

				parent.BaseMove.FixSummaries();

				return result;
			}
			catch (Exception e)
			{
				throw Error.ErrorRunningSchedules.Throw(e);
			}
		}

		private EmailStatus runSchedule(IEnumerable<Schedule> scheduleList)
		{
			var emailsStati = new List<EmailStatus>();
			var exceptions = new List<Exception>();

			foreach (var schedule in scheduleList)
			{
				try
				{
					var result = inTransaction("RunSchedule", () =>
						addNewMoves(schedule)
					);

					emailsStati.AddRange(result);
				}
				catch (Exception e)
				{
					exceptions.Add(e);
				}
			}

			if (exceptions.Any())
			{
				throw new AggregateException(exceptions);
			}

			return emailsStati.Max();
		}

		private IEnumerable<EmailStatus> addNewMoves(Schedule schedule)
		{
			var emailsStati = new List<EmailStatus>();

			while (schedule.CanRunNow())
			{
				var newMove = schedule.CreateMove();

				schedule.LastRun++;

				var result = parent.BaseMove.SaveMove(
					newMove, OperationType.Scheduling
				);

				var move = repos.Move.Get(result.Guid);

				schedule.MoveList.Add(move);
				emailsStati.Add(result.Email);
			}

			repos.Schedule.UpdateState(schedule);

			return emailsStati;
		}

		public ScheduleResult SaveSchedule(ScheduleInfo info)
		{
			parent.Safe.VerifyUser();

			if (info == null)
				throw Error.ScheduleRequired.Throw();

			return inTransaction("SaveSchedule",
				() => save(info)
			);
		}

		private ScheduleResult save(ScheduleInfo info)
		{
			var schedule = new Schedule
			{
				Out = parent.BaseMove.GetAccount(info.OutUrl),
				In = parent.BaseMove.GetAccount(info.InUrl),
				Category = parent.BaseMove.GetCategory(info.CategoryName),
				User = parent.Safe.GetCurrent()
			};

			info.Update(schedule);

			if (schedule.ID == 0 || !schedule.IsDetailed())
			{
				repos.Schedule.Save(schedule);
				repos.Detail.SaveDetails(schedule);
			}
			else
			{
				repos.Detail.SaveDetails(schedule);
				repos.Schedule.Save(schedule);
			}

			return new ScheduleResult(schedule.Guid);
		}

		public void DisableSchedule(Guid guid)
		{
			parent.Safe.VerifyUser();

			var user = parent.Safe.GetCurrent();

			inTransaction("DisableSchedule", () => 
				repos.Schedule.Disable(guid, user)
			);
		}

		public IList<ScheduleInfo> GetScheduleList()
		{
			parent.Safe.VerifyUser();

			var user = parent.Safe.GetCurrent();

			return repos.Schedule
				.Where(
					s => s.Active && s.User.ID == user.ID
				)
				.Select(ScheduleInfo.Convert)
				.ToList();
		}
	}
}
