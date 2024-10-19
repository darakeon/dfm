using System;
using System.Collections.Generic;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Repositories;
using DFM.BusinessLogic.Response;
using DFM.BusinessLogic.Validators;
using DFM.Entities;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using DFM.Entities.Extensions;
using DFM.Exchange.Importer;
using DFM.Queue;

namespace DFM.BusinessLogic.Services;

public class AttendantService : Service
{
	internal AttendantService(ServiceAccess serviceAccess, Repos repos, Valids valids, IQueueService queueService)
		: base(serviceAccess, repos, valids)
	{
		this.queueService = queueService;
	}


	private readonly IQueueService queueService;

	private readonly IDictionary<ImporterError, Error> importErrors =
		new Dictionary<ImporterError, Error>
		{
			{ ImporterError.Size, Error.PlanLimitSizeByArchiveAchieved },
			{ ImporterError.Lines, Error.PlanLimitLineByArchiveAchieved },
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
		var user = parent.Auth.GetCurrent();

		repos.Schedule.ValidatePlanLimit(user);

		var accountOut = parent.BaseMove.GetAccount(info.OutUrl);
		var accountIn = parent.BaseMove.GetAccount(info.InUrl);

		var category = parent.BaseMove.GetCategory(info.CategoryName);

		var schedule = new Schedule
		{
			Out = accountOut,
			In = accountIn,
			Category = category,
			User = user,
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


	public void ImportMovesFile(String filename, String csv)
	{
		var user = parent.Auth.VerifyUser();

		repos.Archive.ValidatePlanLimit(user);

		var importer = validateArchive(filename, csv, user);

		var archive = new Archive
		{
			Guid = Guid.NewGuid(),
			Filename = filename,
			Uploaded = DateTime.UtcNow,
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

	private CSVImporter validateArchive(String filename, String csv, User user)
	{
		using var error = new CoreError();
		var importer = new CSVImporter(user.Control.Plan, csv);

		if (!filename.EndsWith(".csv"))
		{
			error.AddError(Error.InvalidArchiveType);
			return importer;
		}

		if (filename.Length > MaxLen.ArchiveFileName)
		{
			error.AddError(Error.InvalidArchiveName);
			return importer;
		}

		if (importer.ErrorList.Any())
		{
			importer.ErrorList
				.ToList()
				.ForEach(e => error.AddError(importErrors[e.Value], e.Key));
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


	public LineInfo RetryLine(Guid archiveGuid, Int16 linePosition)
	{
		var user = parent.Auth.VerifyUser();
		var line = repos.Line.Get(archiveGuid, linePosition);

		if (line == null || line.Archive.User.ID != user.ID)
			throw Error.LineNotFound.Throw();

		if (line.Status != ImportStatus.Error && line.Status != ImportStatus.Canceled)
			throw Error.LineRetryOnlyErrorOrCanceled.Throw();

		inTransaction("RetryLine", () =>
		{
			line.Scheduled = DateTime.UtcNow;
			line.Status = ImportStatus.Pending;

			repos.Line.SaveOrUpdate(line);

			line.Archive.Status = ImportStatus.Pending;

			repos.Archive.SaveOrUpdate(line.Archive);
		});

		return LineInfo.Convert(line);
	}


	public LineInfo CancelLine(Guid archiveGuid, Int16 linePosition)
	{
		var user = parent.Auth.VerifyUser();
		var line = repos.Line.Get(archiveGuid, linePosition);

		if (line == null || line.Archive.User.ID != user.ID)
			throw Error.LineNotFound.Throw();

		if (line.Status == ImportStatus.Success)
			throw Error.LineCancelNoSuccess.Throw();

		inTransaction("CancelLine", () =>
		{
			line.Status = ImportStatus.Canceled;
			repos.Line.SaveOrUpdate(line);
		});

		return LineInfo.Convert(line);
	}


	public ArchiveInfo CancelArchive(Guid archiveGuid)
	{
		var user = parent.Auth.VerifyUser();
		var archive = repos.Archive.Get(archiveGuid);

		if (archive == null || archive.User.ID != user.ID)
			throw Error.ArchiveNotFound.Throw();

		if (archive.Status == ImportStatus.Success)
			throw Error.ArchiveCancelNoSuccess.Throw();

		inTransaction("CancelArchive", () =>
		{
			var success =
				archive.LineList
					.Any(l => l.Status == ImportStatus.Success);

			archive.Status = success
				? ImportStatus.Success
				: ImportStatus.Canceled;

			archive.LineList
				.Where(l => l.Status != ImportStatus.Success)
				.ToList()
				.ForEach(l => l.Status = ImportStatus.Canceled);

			repos.Archive.SaveOrUpdate(archive);
		});

		return new ArchiveInfo { Archive = archive };
	}


	public void OrderExport(OrderInfo orderInfo)
	{
		var user = parent.Auth.VerifyUser();

		repos.Order.ValidatePlanLimit(user);

		var order = orderInfo.Create(user);

		foreach (var accountUrl in orderInfo.AccountList)
		{
			var account = repos.Account.GetByUrl(accountUrl, user);

			if (account == null)
				throw Error.InvalidAccount.Throw();

			order.AccountList.Add(account);
		}

		foreach (var categoryName in orderInfo.CategoryList)
		{
			var category = repos.Category.GetByName(categoryName, user);

			if (category == null)
				throw Error.InvalidCategory.Throw();

			order.CategoryList.Add(category);
		}

		repos.Move.ValidatePlanLimit(user, order);

		inTransaction("OrderExport", () =>
		{
			repos.Order.SaveOrUpdate(order);
		});
	}


	public IList<OrderItem> GetOrderList()
	{
		var user = parent.Auth.VerifyUser();

		return repos.Order.ByUser(user)
			.Select(o => new OrderItem(o))
			.ToList();
	}


	public OrderItem RetryOrder(Guid orderGuid)
	{
		var order = validateOrder(orderGuid);

		if (order.Status != ExportStatus.Error && order.Status != ExportStatus.Canceled)
			throw Error.OrderRetryOnlyErrorOrCanceled.Throw();

		inTransaction("RetryOrder", () =>
		{
			order = repos.Order.Retry(order);
		});

		return new OrderItem(order);
	}

	public OrderItem CancelOrder(Guid orderGuid)
	{
		var order = validateOrder(orderGuid);

		if (order.Status == ExportStatus.Success || order.Status == ExportStatus.Expired)
			throw Error.OrderCancelNoSuccessExpired.Throw();

		inTransaction("CancelOrder", () =>
		{
			order = repos.Order.Cancel(order);
		});

		return new OrderItem(order);
	}

	public OrderFile DownloadOrder(Guid orderGuid)
	{
		var order = validateOrder(orderGuid);

		if (order.Status != ExportStatus.Success)
			throw Error.OrderDownloadOnlySuccess.Throw();

		var content = repos.Order.GetFile(order);

		if (content == null)
			throw Error.OrderFileDeleted.Throw();

		return new OrderFile(order.Path, content);
	}

	private Order validateOrder(Guid orderGuid)
	{
		var user = parent.Auth.VerifyUser();

		var order = repos.Order.Get(orderGuid);

		if (order == null || order.User.ID != user.ID)
			throw Error.OrderNotFound.Throw();

		return order;
	}
}
