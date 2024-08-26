using System.Collections.Generic;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Exchange.Exporter;
using DFM.Files;

namespace DFM.BusinessLogic.Repositories;

internal class OrderRepository(Repos repos, IFileService fileService) : Repo<Order>
{
	public Order GetNext()
	{
		return NewQuery()
			.Where(o => o.Status == ExportStatus.Pending)
			.OrderBy(o => o.ID)
			.FirstOrDefault;
	}

	public void Cancel(Order order)
	{
		order.Status = ExportStatus.Canceled;
		SaveOrUpdate(order);
	}

	public void ExtractToFile(Order order)
	{
		var moves = repos.Move.Filter(order);

		var csv = new CSVExporter();
		csv.Add(moves);
		csv.Create(order);

		if (csv.Path == null)
		{
			order.Status = ExportStatus.Error;
		}
		else
		{
			fileService.Upload(csv.Path);
			order.Status = ExportStatus.Success;
		}

		SaveOrUpdate(order);
	}

	public void Error(Order order)
	{
		order.Status = ExportStatus.Error;
		SaveOrUpdate(order);
	}
}
