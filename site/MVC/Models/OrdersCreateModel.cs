using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Response;

namespace DFM.MVC.Models;

public class OrdersCreateModel : BaseSiteModel
{
	public OrdersCreateModel()
	{
		Order = new()
		{
			Start = DateTime.Today,
			End = DateTime.Today,
		};

		AllAccountList = admin.GetAccountList(true)
			.Union(admin.GetAccountList(false))
			.ToDictionary(a => a.Url, a => a);

		AccountList = AllAccountList
			.ToDictionary(a => a.Key, _ => false);

		AllCategoryList = admin.GetCategoryList(true)
			.Union(admin.GetCategoryList(false))
			.ToDictionary(c => c.Name, c => c);

		CategoryList = AllCategoryList
			.ToDictionary(c => c.Key, _ => false);
	}

	public OrderInfo Order { get; set; }

	public String Start
	{
		get => Order.Start?.ToString("yyyy-MM-dd");
		set {
			DateTime.TryParse(value, out var start);
			Order.Start = start;
		}
	}

	public String End
	{
		get => Order.End?.ToString("yyyy-MM-dd");
		set {
			DateTime.TryParse(value, out var end);
			Order.End = end;
		}
	}

	public IDictionary<String, AccountListItem> AllAccountList { get; set; }
	public IDictionary<String, Boolean> AccountList { get; set; }

	public IDictionary<String, CategoryListItem> AllCategoryList { get; set; }
	public IDictionary<String, Boolean> CategoryList { get; set; }

	public List<String> SaveOrder()
	{
		var errors = new List<String>();

		AccountList
			.Where(a => a.Value)
			.Select(a => a.Key)
			.ToList()
			.ForEach(Order.AccountList.Add);

		CategoryList
			.Where(a => a.Value)
			.Select(a => a.Key)
			.ToList()
			.ForEach(Order.CategoryList.Add);

		try
		{
			attendant.OrderExport(Order);
		}
		catch (CoreError error)
		{
			errors.Add(translator[error]);
		}

		return errors;
	}
}
