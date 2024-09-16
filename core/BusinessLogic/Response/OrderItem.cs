using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities;
using DFM.Entities.Enums;

namespace DFM.BusinessLogic.Response;

public class OrderItem
{
	public OrderItem()
	{
		AccountList = new List<String>();
		CategoryList = new List<String>();
	}

	public OrderItem(Order order)
	{
		Guid = order.Guid;

		Status = order.Status;

		Creation = order.Creation;
		Exportation = order.Exportation;
		Expiration = order.Expiration;
		Sent = order.Sent;

		Start = order.Start;
		End = order.End;
		AccountList = order.AccountList.Select(a => a.Name).ToList();
		CategoryList = order.CategoryList.Select(a => a.Name).ToList();

		Path = order.Path;
	}


	public Guid Guid { get; set; }

	public ExportStatus Status { get; set; }

	public DateTime? Creation { get; set; }
	public DateTime? Exportation { get; set; }
	public DateTime? Expiration { get; set; }
	public Boolean? Sent { get; set; }

	public DateTime Start { get; set; }
	public DateTime End { get; set; }
	public IList<String> AccountList { get; set; }
	public IList<String> CategoryList { get; set; }

	public String Path { get; set; }
}
