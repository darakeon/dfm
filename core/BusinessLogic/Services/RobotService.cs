using System;
using System.Collections.Generic;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Helpers;
using DFM.BusinessLogic.Repositories;
using DFM.BusinessLogic.Response;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Entities.Extensions;
using DFM.Generic;
using Error = DFM.BusinessLogic.Exceptions.Error;

namespace DFM.BusinessLogic.Services
{
	public class RobotService : Service
	{
		internal RobotService(ServiceAccess serviceAccess, Repos repos)
			: base(serviceAccess, repos) { }

		public DicList<CoreError> RunSchedule()
		{
			var errors = new DicList<CoreError>();

			if (!parent.Current.IsRobot)
				throw Error.Uninvited.Throw();

			var users = repos.User
				.NewQuery()
				.Where(u => u.Control, c => c.Active)
				.Where(u => u.Control, c => c.RobotCheck <= DateTime.UtcNow)
				.Where(u => u.Control, c => !c.IsRobot)
				.List;

			foreach (var user in users)
			{
				runSchedule(user, errors);
			}

			return errors;
		}

		private void runSchedule(User user, DicList<CoreError> errors)
		{
			try
			{
				parent.Safe.VerifyUser(user);
			}
			catch (CoreError e)
			{
				errors.Add(user.Email, e);
				return;
			}

			try
			{
				runSchedule(
					repos.Schedule.GetRunnable(user),
					errors
				);

				parent.BaseMove.FixSummaries(user);

				user.SetRobotCheckDay();
			}
			catch (CoreError e)
			{
				errors.Add(user.Email, e);
			}
		}

		private void runSchedule(IList<Schedule> scheduleList, DicList<CoreError> errors)
		{
			foreach (var schedule in scheduleList)
			{
				try
				{
					inTransaction(
						"RunSchedule",
						() => addNewMoves(schedule)
					);
				}
				catch (CoreError e)
				{
					errors.Add(schedule.User.Email, e);
				}
			}
		}

		private void addNewMoves(Schedule schedule)
		{
			while (schedule.CanRunNow())
			{
				var newMove = schedule.CreateMove();

				var result = parent.BaseMove.SaveMove(
					newMove, OperationType.Scheduling
				);

				var move = repos.Move.Get(result.Guid);

				schedule.MoveList.Add(move);
			}

			repos.Schedule.UpdateState(schedule);
		}

		public ScheduleResult SaveSchedule(ScheduleInfo info)
		{
			parent.Safe.VerifyUser();

			if (info == null)
				throw Error.ScheduleRequired.Throw();

			var result = inTransaction(
				"SaveSchedule",
				() => save(info)
			);

			return result;
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

			repos.Control.AnticipateRobotCheck(schedule.User.Control);

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

		public void CleanupAbandonedUsers(Action<String> upload)
		{
			if (!parent.Current.IsRobot)
				throw Error.Uninvited.Throw();

			var ignoreUsers = new List<User>();

			cleanupByLastAccess(ignoreUsers, upload);
			cleanupByNotSignedContract(ignoreUsers, upload);
		}

		private void cleanupByLastAccess(IList<User> ignoreUsers, Action<String> upload)
		{
			var users = repos.User.NewQuery()
				.Where(
					u => u.Control,
					c => c.LastAccess == null
						|| c.LastAccess < WarnHelper.Limit1()
				)
				.List;

			foreach (var user in users)
			{
				var control = user.Control;
				var date = control.LastInteraction();
				var didSomething = warnOrDelete(
					user, date, upload,
					RemovalReason.NoInteraction
				);

				if (didSomething)
				{
					ignoreUsers.Add(user);
				}
			}
		}

		private void cleanupByNotSignedContract(IList<User> ignoreUsers, Action<String> upload)
		{
			var contract = repos.Contract.GetContract();

			if (contract == null || !contract.BeginDate.PassedWarn1())
				return;

			var accepted = repos.Acceptance
				.Where(a => a.Contract.ID == contract.ID)
				.Select(a => a.User.ID)
				.ToArray();

			var alreadyDidSomething = ignoreUsers
				.Select(s => s.ID).ToArray();

			var notAccepted = repos.User.NewQuery()
				.NotIn(c => c.ID, accepted)
				.NotIn(c => c.ID, alreadyDidSomething)
				.List;

			foreach (var user in notAccepted)
			{
				var userDate = user.Control.Creation;
				var contractDate = contract.BeginDate;

				var newestDate =
					userDate > contractDate
						? userDate
						: contractDate;

				var didSomething = warnOrDelete(
					user, newestDate, upload,
					RemovalReason.NotSignedContract
				);

				if (didSomething)
				{
					ignoreUsers.Add(user);
				}
			}
		}

		private Boolean warnOrDelete(
			User user,
			DateTime date,
			Action<String> upload,
			RemovalReason reason
		)
		{
			var sent = user.Control.RemovalWarningSent;

			var shouldWarn1 = date.PassedWarn1() && sent < 1;
			var shouldWarn2 = date.PassedWarn2() && sent < 2;
			var shouldRemove = date.PassedRemoval() && sent >= 2;

			if (shouldRemove)
				return delete(user, reason, upload);

			if (shouldWarn1 || shouldWarn2)
				return warn(date, user, reason);

			return false;
		}

		private Boolean delete(User user, RemovalReason reason, Action<String> upload)
		{
			inTransaction(
				"MarkUserDeletion",
				() => repos.Control.MarkDeletion(user)
			);

			inTransaction(
				"DeleteUser",
				() => repos.Purge.Execute(user, reason, upload)
			);

			return true;
		}

		private Boolean warn(DateTime date, User user, RemovalReason reason)
		{
			inTransaction(
				"SaveWarning",
				() => repos.Control.WarnRemoval(user, date, reason)
			);

			return true;
		}
	}
}
