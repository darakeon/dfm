﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Helpers;
using DFM.BusinessLogic.Repositories;
using DFM.BusinessLogic.Response;
using DFM.BusinessLogic.Validators;
using DFM.Entities;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using DFM.Generic;
using DFM.Queue;

namespace DFM.BusinessLogic.Services
{
	public class ExecutorService : Service
	{
		internal ExecutorService(ServiceAccess serviceAccess, Repos repos, Valids valids, IQueueService queueService)
			: base(serviceAccess, repos, valids)
		{
			this.queueService = queueService;
		}


		private readonly IQueueService queueService;


		public DicList<CoreError> RunSchedule()
		{
			if (!parent.Current.IsRobot)
				throw Error.Uninvited.Throw();

			var errors = new DicList<CoreError>();

			var usersToRun = repos.User.GetForRunSchedule();

			foreach (var user in usersToRun)
			{
				runSchedule(user, errors);
			}

			repos.Schedule.GetForRobot().ToList()
				.ForEach(s => setFailure(s, ScheduleStatus.UserRobot));

			repos.Schedule.GetForInactive().ToList()
				.ForEach(s => setFailure(s, ScheduleStatus.UserInactive));

			return errors;
		}

		private void runSchedule(User user, DicList<CoreError> errors)
		{
			var scheduleList = repos.Schedule.GetRunnable(user);

			if (!scheduleList.Any())
				return;

			try
			{
				parent.Auth.VerifyUser(user);
			}
			catch (CoreError e)
			{
				scheduleList.ToList()
					.ForEach(s => setFailure(s, e));

				errors.Add(user.Email, e);
				return;
			}

			try
			{
				runSchedule(scheduleList, errors);

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
				while (schedule.CanRunNow())
				{
					tryAddNewMove(errors, schedule);
				}
			}
		}

		private void tryAddNewMove(DicList<CoreError> errors, Schedule schedule)
		{
			try
			{
				inTransaction(
					"RunSchedule",
					() => addNewMove(schedule)
				);
			}
			catch (CoreError e)
			{
				setFailure(schedule, e);
				errors.Add(schedule.User.Email, e);
			}
		}

		private void addNewMove(Schedule schedule)
		{
			var newMove = schedule.CreateMove();

			var result = parent.BaseMove.SaveMove(
				newMove, OperationType.Scheduling
			);

			var move = repos.Move.Get(result.Guid);

			schedule.MoveList.Add(move);

			repos.Schedule.UpdateState(schedule);
		}

		private void setFailure(Schedule schedule, CoreError error)
		{
			setFailure(
				schedule,
				s => repos.Schedule.SetFailure(s, error.Type)
			);
		}

		private void setFailure(Schedule schedule, ScheduleStatus status)
		{
			setFailure(
				schedule,
				s => repos.Schedule.SetFailure(s, status)
			);
		}

		private void setFailure(Schedule schedule, Action<Schedule> setFailure)
		{
			var user = schedule.User;
			var control = user.Control;

			user.SetRobotCheckDay();

			inTransaction("RunScheduleNot", () =>
			{
				repos.Control.SaveOrUpdate(control);
				setFailure(schedule);
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

		private Boolean warnOrDelete(User user, DateTime date, RemovalReason reason)
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
		
		private void delete(User user, DateTime date, RemovalReason reason)
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

		
		public async Task<ExecutorResult<MoveResult>> MakeMoveFromImported()
		{
			if (!parent.Current.IsRobot)
				throw Error.Uninvited.Throw();

			var dequeued = await queueService.Dequeue();

			if (!dequeued.HasValue)
				return null;

			var key = dequeued.Value.Key;
			var line = repos.Line.Get(dequeued.Value.Value.ID);
			var user = line.Archive.User;

			try
			{
				parent.Auth.VerifyUser(user);

				return makeMove(user, line);
			}
			catch (CoreError e)
			{
				inTransaction("MakeMoveFromImported", () =>
				{
					line.Status =
						e.Type == Error.PlanLimitAccountMonthMoveAchieved
							? ImportStatus.OutOfLimit
							: ImportStatus.Error;

					repos.Line.SaveOrUpdate(line);
				});

				return new ExecutorResult<MoveResult>(user, e);
			}
			finally
			{
				queueService.Delete(key);
			}
		}

		private ExecutorResult<MoveResult> makeMove(User user, Line line)
		{
			if (line.Status != ImportStatus.Pending)
				return null;

			var newMove = createMove(line);

			var move = inTransaction("MakeMoveFromImported", () =>
			{
				var result = parent.BaseMove.SaveMove(
					newMove, OperationType.Importing
				);

				line.Status = ImportStatus.Success;
				repos.Line.SaveOrUpdate(line);

				return result;
			});

			parent.BaseMove.FixSummaries(user);

			return new ExecutorResult<MoveResult>(user, move);
		}

		private Move createMove(Line line)
		{
			var move =
				new Move
				{
					Description = line.Description,
					Nature = line.GetNature(),
					In = getAccountOrThrow(line.In, line.Archive.User),
					Out = getAccountOrThrow(line.Out, line.Archive.User),
					Category = getCategoryOrThrow(line.Category, line.Archive.User),
					Value = line.DetailList.Any() ? 0 : line.Value ?? 0,
					Conversion = line.DetailList.Any() ? null : line.Conversion,
				};

			foreach (var detail in line.DetailList)
			{
				var newDetail = detail.Clone();
				newDetail.Move = move;
				move.DetailList.Add(newDetail);
			}

			move.SetDate(line.Date);

			move.SetPositionInSchedule();

			return move;
		}

		private Account getAccountOrThrow(String name, User user)
		{
			if (String.IsNullOrEmpty(name))
				return null;

			var account = repos.Account.GetByName(name, user);

			if (account == null)
				throw Error.InvalidAccount.Throw();

			return account;
		}

		private Category getCategoryOrThrow(String name, User user)
		{
			if (String.IsNullOrEmpty(name))
				return null;

			var category = repos.Category.GetByName(name, user);

			if (category == null)
				throw Error.InvalidCategory.Throw();

			return category;
		}


		public void FinishArchives()
		{
			if (!parent.Current.IsRobot)
				throw Error.Uninvited.Throw();

			var archivesLineStati = repos.Line.GetArchivesPending();

			foreach (var archiveLineStati in archivesLineStati)
			{
				if (archiveLineStati.MinLineStati == ImportStatus.Pending)
					continue;

				var archive = archiveLineStati.Archive;
				archive.Status = archiveLineStati.MaxLineStati;

				inTransaction("FinishArchives", () =>
				{
					repos.Archive.SaveOrUpdate(archive);
				});
			}
		}


		public async Task RequeueLines()
		{
			if (!parent.Current.IsRobot)
				throw Error.Uninvited.Throw();

			var lines = repos.Line.GetToRequeue();

			if (!lines.Any())
				return;

			await queueService.Enqueue(lines);

			inTransaction("RequeueLines", () =>
			{
				foreach (var line in lines)
				{
					line.Scheduled = DateTime.UtcNow;
					repos.Line.SaveOrUpdate(line);
				}
			});
		}


		public ExecutorResult ExportOrder()
		{
			if (!parent.Current.IsRobot)
				throw Error.Uninvited.Throw();

			var order = repos.Order.GetNext();

			try
			{
				parent.Auth.VerifyUser(order.User);

				inTransaction(
					"ExportOrder",
					() => repos.Order.ExtractToFileAndSend(order)
				);

				return new ExecutorResult(order.User);
			}
			catch (CoreError e)
			{
				if (order.Status != ExportStatus.Error)
				{
					inTransaction(
						"ExportOrder",
						() => repos.Order.SetFailure(order, e.Type)
					);
				}

				return new ExecutorResult(order.User, e);
			}
		}


		public IList<ExecutorResult> DeleteExpiredOrders()
		{
			if (!parent.Current.IsRobot)
				throw Error.Uninvited.Throw();

			var orders = repos.Order.GetExpired();
			var results = new List<ExecutorResult>();

			foreach (var order in orders)
			{
				var result = expire(order);
				results.Add(result);
			}

			return results;
		}

		private ExecutorResult expire(Order order)
		{
			try
			{
				parent.Auth.VerifyUserIgnoreContract(order.User);

				inTransaction(
					"DeleteExpiredOrders",
					() => repos.Order.Expire(order)
				);

				return new ExecutorResult(order.User);
			}
			catch (CoreError e)
			{
				return new ExecutorResult(order.User, e);
			}
		}


		public void ExpireTickets()
		{
			if (!parent.Current.IsRobot)
				throw Error.Uninvited.Throw();

			var lastAccessLimit = DateTime.UtcNow.AddDays(
				-Defaults.TicketExpirationDays
			);

			var tickets = repos.Ticket.Where(
				t => t.LastAccess <= lastAccessLimit
			);

			foreach (var ticket in tickets)
			{
				inTransaction(
					"ExpireTickets",
					() => repos.Ticket.Disable(ticket)
				);
			}
		}
	}
}
