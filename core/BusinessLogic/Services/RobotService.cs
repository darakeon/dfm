using System;
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
using DFM.Entities.Extensions;
using DFM.Exchange.Importer;
using DFM.Generic;
using DFM.Queue;
using Error = DFM.BusinessLogic.Exceptions.Error;

namespace DFM.BusinessLogic.Services
{
	public class RobotService : Service
	{
		private readonly IQueueService queueService;

		internal RobotService(ServiceAccess serviceAccess, Repos repos, Valids valids, IQueueService queueService)
			: base(serviceAccess, repos, valids)
		{
			this.queueService = queueService;
		}

		private readonly IDictionary<ImporterError, Error> errors =
			new Dictionary<ImporterError, Error>
			{
				{ ImporterError.Size, Error.InvalidArchiveSize },
				{ ImporterError.Lines, Error.InvalidArchiveLines },
				{ ImporterError.Header, Error.InvalidArchiveColumn },
				{ ImporterError.Empty, Error.EmptyArchive },
				{ ImporterError.DateRequired, Error.MoveDateRequired },
				{ ImporterError.DateInvalid, Error.MoveDateInvalid },
				{ ImporterError.NatureRequired, Error.MoveNatureRequired },
				{ ImporterError.NatureInvalid, Error.MoveNatureInvalid },
				{ ImporterError.ValueInvalid, Error.MoveValueInvalid },
				{ ImporterError.DetailAmountRequired, Error.MoveDetailAmountRequired },
				{ ImporterError.DetailAmountInvalid, Error.MoveDetailAmountInvalid },
				{ ImporterError.DetailValueRequired, Error.MoveDetailValueRequired },
				{ ImporterError.DetailValueInvalid, Error.MoveDetailValueInvalid },
				{ ImporterError.DetailConversionInvalid, Error.MoveDetailConversionInvalid },
			};

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

		public void ImportMovesFile(String filename, String csv)
		{
			var user = parent.Auth.VerifyUser();

			var importer = validateArchive(csv, user);

			var archive = new Archive
			{
				Guid = Guid.NewGuid(),
				Filename = filename,
				User = user,
			};

			archive.LineList =
				importer.MoveList
					.Select(m => m.ToLine(archive))
					.ToList();

			inTransaction(
				"ImportMovesFile",
				() => repos.Archive.SaveOrUpdate(archive)
			);

			queueService.Enqueue(archive.LineList);
		}

		private CSVImporter validateArchive(String csv, User user)
		{
			using var error = new CoreError();

			var importer = new CSVImporter(csv);

			if (importer.ErrorList.Any())
			{
				importer.ErrorList
					.ToList()
					.ForEach(e => error.AddError(errors[e.Value], e.Key));
			}

			var accountsIn = importer.MoveList.Select(m => m.In);
			var accountsOut = importer.MoveList.Select(m => m.Out);

			var accounts =
				accountsIn
					.Union(accountsOut)
					.Where(a => !String.IsNullOrEmpty(a))
					.Distinct()
					.ToDictionary(
						a => a,
						a => repos.Account.GetByName(a, user)
					);

			var categories =
				importer.MoveList
					.Where(m => !String.IsNullOrEmpty(m.Category))
					.Select(m => m.Category)
					.Distinct()
					.ToDictionary(
						name => name,
						name => repos.Category.GetByName(name, user)
					);

			foreach (var line in importer.MoveList)
			{
				try
				{
					var move = line.ToMove(accounts, categories);
					var valid = true;

					if (!String.IsNullOrEmpty(line.In) && move.In == null)
					{
						error.AddError(Error.InvalidAccount, line.Position);
						valid = false;
					}

					if (!String.IsNullOrEmpty(line.Out) && move.Out == null)
					{
						error.AddError(Error.InvalidAccount, line.Position);
						valid = false;
					}

					if (!String.IsNullOrEmpty(line.Category) && move.Category == null)
					{
						error.AddError(Error.InvalidCategory, line.Position);
						valid = false;
					}

					if (!valid)
						continue;

					valids.Move.Validate(move, user);

					move.DetailList
						.ToList()
						.ForEach(valids.Detail.Validate);
				}
				catch (CoreError moveError)
				{
					error.AddError(moveError.Type, line.Position);
				}
			}

			return importer;
		}

		public async Task<MoveResult> MakeMoveFromImported()
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

				return move;
			}
			catch (CoreError e)
			{
				inTransaction("MakeMoveFromImported", () =>
				{
					line.Status = ImportStatus.Error;
					repos.Line.SaveOrUpdate(line);
				});

				throw;
			}
			finally
			{
				queueService.Delete(key);
			}
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

		public void RequeueLines()
		{
			if (!parent.Current.IsRobot)
				throw Error.Uninvited.Throw();

			var lines = repos.Line.Where(
				l => l.Status == ImportStatus.Pending
					&& l.Scheduled < DateTime.Now.AddDays(-1)
			);

			queueService.Enqueue(lines);

			foreach (var line in lines)
			{
				line.Scheduled = DateTime.Now;
				repos.Line.SaveOrUpdate(line);
			}
		}

		public IList<ArchiveInfo> GetArchiveList()
		{
			var user = parent.Auth.VerifyUser();

			return repos.Line.GetArchives(user);
		}

		public ArchiveInfo GetLineList(Guid archiveGuid)
		{
			var user = parent.Auth.VerifyUser();
			var archive = repos.Archive.Get(archiveGuid);

			if (archive == null || archive.User.ID != user.ID)
				throw Error.ArchiveNotFound.Throw();

			return new ArchiveInfo
			{
				Archive = archive,
				LineList = archive.LineList
					.Select(LineInfo.Convert).ToList(),
			};
		}

		public void RetryLine(Guid archiveGuid, Int16 linePosition)
		{
			var user = parent.Auth.VerifyUser();
			var line = repos.Line.Get(archiveGuid, linePosition);

			if (line == null || line.Archive.User.ID != user.ID)
				throw Error.LineNotFound.Throw();

			if (line.Status != ImportStatus.Error)
				throw Error.LineRetryOnlyError.Throw();

			inTransaction("RetryLine", () =>
			{
				line.Scheduled = DateTime.Now;
				line.Status = ImportStatus.Pending;

				repos.Line.SaveOrUpdate(line);

				line.Archive.Status = ImportStatus.Pending;

				repos.Archive.SaveOrUpdate(line.Archive);
			});
		}
	}
}
