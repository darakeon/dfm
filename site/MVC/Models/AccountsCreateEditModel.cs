using System;
using System.Collections.Generic;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Response;
using DFM.Entities.Enums;
using DFM.MVC.Helpers.Global;

namespace DFM.MVC.Models
{
	public class AccountsCreateEditModel : BaseSiteModel
	{
		public AccountsCreateEditModel()
		{
			Account = new AccountInfo();
		}

		public AccountsCreateEditModel(OperationType type) : this()
		{
			Type = type;
		}

		public AccountsCreateEditModel(OperationType type, String id) : this(type)
		{
			Account = admin.GetAccount(id);
		}

		public OperationType Type { get; set; }

		public AccountInfo Account { get; set; }

		public Boolean HasLimit
		{
			get => Account.RedLimit != null || Account.YellowLimit != null;
			set => setLimit(value);
		}

		private void setLimit(Boolean hasLimit)
		{
			if (hasLimit)
			{
				if (Account.RedLimit == null)
					Account.RedLimit = 0;

				if (Account.YellowLimit == null)
					Account.YellowLimit = 0;
			}
			else
			{
				Account.RedLimit = null;
				Account.YellowLimit = null;
			}
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
				errors.Add(Translator.Dictionary[e]);
			}

			return errors;
		}
	}
}
