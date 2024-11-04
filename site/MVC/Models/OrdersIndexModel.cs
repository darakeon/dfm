using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DFM.BusinessLogic.Response;

namespace DFM.MVC.Models;

public class OrdersIndexModel : BaseSiteModel, IDisposable
{
	public OrdersIndexModel()
	{
		OrderList = attendant.GetOrderList();
	}

	public IList<OrderItem> OrderList { get; set; }

	public OrderRowModel Order { get; private set; }
	
	internal String FileName { get; private set; }
	internal Stream FileContent { get; private set; }


	public void Cancel(Guid id)
	{
		var order = attendant.CancelOrder(id);
		Order = new OrderRowModel(order, IsUsingCategories);
	}

	public void Retry(Guid id)
	{
		var order = attendant.RetryOrder(id);
		Order = new OrderRowModel(order, IsUsingCategories);
	}

	public void Download(Guid id)
	{
		var order = attendant.DownloadOrder(id);
		FileName = order.Path;
		FileContent = new MemoryStream(
			Encoding.UTF8.GetBytes(order.Content)
		);
	}


	public void Dispose()
	{
		FileContent?.Dispose();
	}
}
