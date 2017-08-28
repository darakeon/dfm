using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities;
using DFM.Generic;
using DFM.MVC.Helpers.Global;

namespace DFM.MVC.Models
{
	public class AccountsCreateEditModel : BaseLoggedModel
	{
		public AccountsCreateEditModel()
		{
			Account = new Account();
		}

		public AccountsCreateEditModel(OperationType type) : this()
		{
			Type = type;
		}

		public AccountsCreateEditModel(OperationType type, String id) : this(type)
		{
			Account = Admin.GetAccountByUrl(id);
		}



		public OperationType Type { get; set; }

		public Account Account { get; set; }


		private String url;

		[Required(ErrorMessage = "*")]
		public new String Url
		{
			get
			{
				switch (Type)
				{
					case OperationType.Creation:
						return Account.Name;
					case OperationType.Edit:
						return url ?? Account.Url;
					default:
						throw new NotImplementedException();
				}
			}
			set
			{
				url = value;

				if (Type == OperationType.Creation)
					Account.Url = value;
			}
		}



		public Boolean HasLimit
		{
			get { return Account.RedLimit != null || Account.YellowLimit != null; }
			set { setLimit(value); }
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

		

		internal void ResetAccountUrl(OperationType type, string id)
		{
			var oldAccount = Admin.GetAccountByUrl(id);

			Type = type;
			Account.Url = oldAccount.Url;
		}

		

		internal IList<String> CreateOrUpdate()
		{
			var errors = new List<String>();

			try
			{
				Account.User = Current.User;

				if (Type == OperationType.Creation)
					Admin.CreateAccount(Account);
				else
					Admin.UpdateAccount(Account, Url);
			}
			catch (DFMCoreException e)
			{
				errors.Add(MultiLanguage.Dictionary[e]);
			}

			return errors;
		}



	}
}