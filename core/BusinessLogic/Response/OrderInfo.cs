using System;
using System.Collections.Generic;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities;

namespace DFM.BusinessLogic.Response;

public class OrderInfo
{
	public virtual DateTime? Start { get; set; }
	public virtual DateTime? End { get; set; }

	public virtual IList<String> AccountList { get; } = new List<String>();
	public virtual IList<String> CategoryList { get; } = new List<String>();

	internal Order Create(User user)
	{
		if (Start == null || End == null || Start > End || End > user.Now())
			throw Error.InvalidDateRange.Throw();

		if (!AccountList.Any())
			throw Error.OrderNoAccounts.Throw();

		if (user.Settings.UseCategories)
		{
			if (!CategoryList.Any())
				throw Error.OrderNoCategories.Throw();
		}
		else
		{
			if (CategoryList.Any())
				throw Error.CategoriesDisabled.Throw();
		}

		return new Order
		{
			Guid = Guid.NewGuid(),
			Start = Start.Value,
			End = End.Value,
			User = user,
		};
	}
}
