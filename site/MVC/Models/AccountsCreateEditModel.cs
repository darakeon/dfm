using System;
using System.Collections.Generic;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Response;
using DFM.Entities.Enums;
using Keon.MVC.Forms;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DFM.MVC.Models
{
	public class AccountsCreateEditModel : BaseSiteModel
	{
		public AccountsCreateEditModel()
		{
			Type = OperationType.Creation;
			Account = new AccountInfo();
			makeCurrencyList();
		}

		public AccountsCreateEditModel(String id)
		{
			Type = OperationType.Edition;
			Account = admin.GetAccount(id);
			makeCurrencyList();
		}

		public OperationType Type { get; set; }

		public AccountInfo Account { get; set; }

		public SelectList CurrencySelectList { get; set; }

		public Boolean HasLimit
		{
			get => Account.HasLimit;
			set => setLimit(value);
		}

		public String YellowLimit
		{
			get => Account.YellowLimit?.ToString("0.00");
			set =>
				Account.YellowLimit = value == null
					? default(Decimal?)
					: Decimal.Parse(value);
		}

		public String RedLimit
		{
			get => Account.RedLimit?.ToString("0.00");
			set =>
				Account.RedLimit = value == null
					? default(Decimal?)
					: Decimal.Parse(value);
		}

		private void setLimit(Boolean hasLimit)
		{
			if (hasLimit)
			{
				Account.RedLimit ??= 0;
				Account.YellowLimit ??= 0;
			}
			else
			{
				Account.RedLimit = null;
				Account.YellowLimit = null;
			}
		}

		private void makeCurrencyList()
		{
			CurrencySelectList = SelectListExtension.CreateSelect(
				translator.GetEnumNames<Currency>()
			);
		}

		internal IList<String> CreateOrUpdate()
		{
			var errors = new List<String>();

			try
			{
				if (Type == OperationType.Creation)
					admin.CreateAccount(Account);
				else
					admin.UpdateAccount(Account);
			}
			catch (CoreError e)
			{
				errors.Add(translator[e]);
			}

			return errors;
		}
	}
}
