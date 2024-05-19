using System;
using System.Collections.Generic;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Helpers;
using DFM.BusinessLogic.Repositories;
using DFM.BusinessLogic.Response;
using DFM.BusinessLogic.Validators;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Entities.Extensions;
using DFM.Generic;
using Error = DFM.BusinessLogic.Exceptions.Error;

namespace DFM.BusinessLogic.Services
{
	public class RobotService : Service
	{
		internal RobotService(ServiceAccess serviceAccess, Repos repos, Valids valids)
			: base(serviceAccess, repos, valids) { }

		public IList<ScheduleInfo> GetScheduleList()
		{
			parent.Auth.VerifyUser();

			var user = parent.Auth.GetCurrent();

			return repos.Schedule
				.Where(
					s => s.Active && s.User.ID == user.ID
				)
				.Select(ScheduleInfo.Convert)
				.ToList();
		}

		public Boolean HasSchedule()
		{
			var user = parent.Auth.VerifyUser();
			var x = repos.Schedule.Where(
				s => s.User.ID == user.ID
						&& s.Active
			);
			return repos.Schedule.Any(
				s => s.User.ID == user.ID
						&& s.Active
			);
		}

		public ScheduleResult SaveSchedule(ScheduleInfo info)
		{
			parent.Auth.VerifyUser();

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
			var accountOut = parent.BaseMove.GetAccount(info.OutUrl);
			var accountIn = parent.BaseMove.GetAccount(info.InUrl);

			var category = parent.BaseMove.GetCategory(info.CategoryName);

			var user = parent.Auth.GetCurrent();

			var schedule = new Schedule
			{
				Out = accountOut,
				In = accountIn,
				Category = category,
				User = user
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
				parent.Auth.VerifyUser(user);
			}
			catch (CoreError e)
			{
				if (e.Type != Error.NotSignedLastContract)
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

		public void DisableSchedule(Guid guid)
		{
			parent.Auth.VerifyUser();

			var user = parent.Auth.GetCurrent();

			inTransaction("DisableSchedule", () =>
				repos.Schedule.Disable(guid, user)
			);
		}

		public void AskWipe(String password)
		{
			parent.Auth.VerifyUser();

			inTransaction("AskWipe", () =>
			{
				var user = parent.Auth.GetCurrent();

				var validPassword =
					repos.User.VerifyPassword(user, password);

				if (!validPassword)
					throw Error.WrongPassword.Throw();

				repos.Control.RequestWipe(user);
			});
		}

		public void WipeUsers()
		{
			if (!parent.Current.IsRobot)
				throw Error.Uninvited.Throw();

			var ignoreUserIDs = new List<Int64>();

			wipeBecauseNoInteraction(ignoreUserIDs);
			wipeBecauseNotSignedContract(ignoreUserIDs);
			wipeBecausePersonAsked(ignoreUserIDs);
		}

		private void wipeBecauseNoInteraction(IList<Int64> ignoreUserIDs)
		{
			var users = repos.User.NewQuery()
				.Where(
					u => u.Control,
					c => c.LastAccess == null
						|| c.LastAccess < WarnHelper.Limit1()
				)
				.NotIn(u => u.ID, ignoreUserIDs)
				.List;

			foreach (var user in users)
			{
				if (user.Control.IsRobot)
					continue;

				var control = user.Control;
				var date = control.LastInteraction();
				var didSomething = warnOrDelete(
					user, date,
					RemovalReason.NoInteraction
				);

				if (didSomething)
				{
					ignoreUserIDs.Add(user.ID);
				}
			}
		}

		private void wipeBecauseNotSignedContract(IList<Int64> ignoreUserIDs)
		{
			var contract = repos.Contract.GetContract();

			if (contract == null || !contract.BeginDate.PassedWarn1())
				return;

			var accepted = repos.Acceptance
				.Where(a => a.Contract.ID == contract.ID)
				.Select(a => a.User.ID)
				.ToArray();

			var notAccepted = repos.User.NewQuery()
				.NotIn(u => u.ID, accepted)
				.NotIn(u => u.ID, ignoreUserIDs)
				.List;

			foreach (var user in notAccepted)
			{
				if (user.Control.IsRobot)
					continue;

				var userDate = user.Control.Creation;
				var contractDate = contract.BeginDate;

				var newestDate =
					userDate > contractDate
						? userDate
						: contractDate;

				var didSomething = warnOrDelete(
					user, newestDate,
					RemovalReason.NotSignedContract
				);

				if (didSomething)
				{
					ignoreUserIDs.Add(user.ID);
				}
			}
		}

		private void wipeBecausePersonAsked(IList<Int64> ignoreUserIDs)
		{
			var users = repos.User.NewQuery()
				.NotIn(u => u.ID, ignoreUserIDs)
				.Where(u => u.Control, c => c.WipeRequest != null)
				.List;

			foreach (var user in users)
			{
				if (user.Control.IsRobot)
					continue;

				// ReSharper disable once PossibleInvalidOperationException
				var date = user.Control.WipeRequest.Value;

				delete(user, date, RemovalReason.PersonAsked);

				ignoreUserIDs.Add(user.ID);
			}
		}

		private Boolean warnOrDelete(
			User user,
			DateTime date,
			RemovalReason reason
		)
		{
			var sent = user.Control.RemovalWarningSent;

			var shouldWarn1 = date.PassedWarn1() && sent < 1;
			var shouldWarn2 = date.PassedWarn2() && sent < 2;
			var shouldRemove = date.PassedRemoval() && sent >= 2;

			if (shouldRemove)
			{
				delete(user, date, reason);
				return true;
			}

			if (shouldWarn1 || shouldWarn2)
			{
				return warn(user, date, reason);
			}

			return false;
		}

		private Boolean warn(User user, DateTime date, RemovalReason reason)
		{
			inTransaction(
				"SaveWarning",
				() => repos.Control.WarnRemoval(user, date, reason)
			);

			return true;
		}

		private void delete(
			User user,
			DateTime date,
			RemovalReason reason
		)
		{
			inTransaction(
				"MarkUserDeletion",
				() => repos.Control.MarkDeletion(user)
			);

			inTransaction(
				"DeleteUser",
				() => repos.Wipe.Execute(user, date, reason)
			);
		}
	}
}
