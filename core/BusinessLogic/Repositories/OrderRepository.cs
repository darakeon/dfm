using System;
using System.Collections.Generic;
using System.Linq;
using Keon.Util.Extensions;
using DFM.Email;
using DFM.Entities;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using DFM.Exchange.Exporter;
using DFM.Files;
using DFM.Generic.Datetime;

namespace DFM.BusinessLogic.Repositories;

internal class OrderRepository(Repos repos, Current.GetUrl getUrl, IFileService fileService) : Repo<Order>
{
	public Order GetNext()
	{
		return NewQuery()
			.Where(o => o.Status == ExportStatus.Pending)
			.OrderBy(o => o.ID)
			.FirstOrDefault;
	}

	public void ExtractToFileAndSend(Order order)
	{
		var moves = repos.Move.Filter(order);

		var csv = new CSVExporter();
		csv.Add(moves);
		csv.Create(order);

		if (csv.Path == null)
		{
			Error(order);
			return;
		}

		fileService.Upload(csv.Path);
		order.Path = csv.Path;
		order.Status = ExportStatus.Success;
		order.Creation = order.User.Now();

		sendEmail(order, csv.Path);

		SaveOrUpdate(order);
	}

	private void sendEmail(Order order, String filename)
	{
		var dic = new Dictionary<String, String>
		{
			{ "Url", getUrl() },
			{ "OrderDate", order.Creation?.UniversalWithTime() },
			{ "DateRangeStart", order.Start.ToShortDateString() },
			{ "DateRangeEnd", order.End.ToShortDateString() },
			{ "Accounts", String.Join('\n', order.AccountList) },
			{ "Categories", String.Join('\n', order.CategoryList) },
			{ "AvailableUntil", order.Expiration?.UniversalWithTime() },
		};

		var format = Format.ExportData(order.User);
		var fileContent = format.Layout.Format(dic);

		var sender = new Sender()
			.To(order.User.Email)
			.Subject(format.Subject)
			.Body(fileContent)
			.Attach(filename);

		try
		{
			sender.Send();
			order.Sent = true;
		}
		catch (MailError)
		{
			order.Sent = false;
		}
	}

	public void Error(Order order)
	{
		order.Status = ExportStatus.Error;
		order.Sent = false;
		SaveOrUpdate(order);
	}

	public IList<Order> GetExpired()
	{
		var limitDaysAgo = DateTime.Now.AddDays(-DayLimits.EXPORT_EXPIRATION);

		return Where(
			o => o.Status != ExportStatus.Expired
			    && o.Creation <= limitDaysAgo
				&& o.Path != null
		);
	}

	public void Expire(Order order)
	{
		fileService.Delete(order.Path);

		order.Status = ExportStatus.Expired;
		SaveOrUpdate(order);
	}

	public IList<String> GetFiles(User user)
	{
		return Where(
			o => o.User.ID == user.ID
				&& o.Path != null
				&& o.Status != ExportStatus.Expired
		)
			.Select(o => o.Path)
			.ToList();
	}

	public IList<Order> ByUser(User user)
	{
		return Where(o => o.User == user);
	}

	public Order Get(Guid guid)
	{
		return SingleOrDefault(
			m => m.ExternalId == guid.ToByteArray()
		);
	}

	public Order Retry(Order order)
	{
		order.Status = ExportStatus.Pending;
		return SaveOrUpdate(order);
	}

	public Order Cancel(Order order)
	{
		order.Status = ExportStatus.Canceled;
		return SaveOrUpdate(order);
	}
}
