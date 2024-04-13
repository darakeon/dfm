using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Helpers;
using DFM.BusinessLogic.Response;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using DFM.MVC.Helpers.Views;
using Keon.MVC.Forms;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DFM.MVC.Areas.Account.Models
{
	public abstract class BaseMovesModel : BaseAccountModel
	{
		private readonly IList<AccountListItem> accountList;

		private BaseMovesModel()
		{
			accountList = admin.GetAccountList(true);
			populateDropDowns();
		}

		protected BaseMovesModel(IMoveInfo move)
			: this()
		{
			GenericMove = move;

			if (GenericMove.GetDate() == DateTime.MinValue)
				Date = now.ToShortDateString();

			arrangeDetails();
		}

		protected BaseMovesModel(IMoveInfo iMove, OperationType type)
			: this(iMove)
		{
			Type = type;
		}

		public void SetDefaultAccount()
		{
			AccountOutUrl = CurrentAccountUrl;
			AccountInUrl = CurrentAccountUrl;
		}



		private void populateDropDowns()
		{
			GenericMove ??= initIMove();

			makeCategoryList();

			makeAccountList();

			arrangeDetails();
		}

		private IMoveInfo initIMove()
		{
			if (GetType() == typeof(SchedulesCreateModel))
				return new ScheduleInfo();

			return new MoveInfo();
		}

		private void makeCategoryList()
		{
			var categoryList = admin.GetCategoryList(true);

			CategorySelectList = SelectListExtension
				.CreateSelect(categoryList, mv => mv.Name, mv => mv.Name);
		}

		private void makeAccountList()
		{
			AccountOutSelectList = SelectListExtension
				.CreateSelect(accountList, a => a.Url, a => a.Name);

			AccountInSelectList = SelectListExtension
				.CreateSelect(accountList, a => a.Url, a => a.Name);
		}

		private void arrangeDetails()
		{
			IsDetailed = GenericMove.IsDetailed();
		}


		public OperationType Type { get; set; }


		public IMoveInfo GenericMove { get; set; }

		public const Int32 DetailCount = 100;

		private IList<DetailUI> detailList;

		public IList<DetailUI> DetailList
		{
			get
			{
				if (detailList == null)
				{
					detailList =
						GenericMove.DetailList
							.Select(d => new DetailUI(d))
							.ToList();

					if (!detailList.Any())
					{
						detailList.Add(new DetailUI(new DetailInfo()));
					}

					for (var d = detailList.Count; d < DetailCount; d++)
					{
						detailList.Add(new DetailUI());
					}
				}

				return detailList;
			}
			set
			{
				GenericMove.DetailList = value
					.Where(d => d.Send)
					.Select(d => d.Detail)
					.ToList();
			}
		}


		[Required(ErrorMessage = "*")]
		public String Description
		{
			get => GenericMove.Description;
			set => GenericMove.Description = value;
		}


		[Required(ErrorMessage = "*")]
		public String Date
		{
			get => GenericMove.GetDate().ToString("yyyy-MM-dd");
			set => GenericMove.SetDate(value);
		}


		[Required(ErrorMessage = "*")]
		public MoveNature Nature
		{
			get => GenericMove.Nature;
			set => GenericMove.Nature = value;
		}



		[Required(ErrorMessage = "*")]
		public SelectList CategorySelectList { get; set; }
		public String CategoryName
		{
			get => GenericMove.CategoryName;
			set => GenericMove.CategoryName = value;
		}

		public SelectList AccountOutSelectList { get; set; }
		public String AccountOutUrl
		{
			get => GenericMove.OutUrl;
			set => GenericMove.OutUrl = value;
		}

		public SelectList AccountInSelectList { get; set; }
		public String AccountInUrl
		{
			get => GenericMove.InUrl;
			set => GenericMove.InUrl = value;
		}


		public Boolean IsDetailed { get; set; }


		public String Value
		{
			get =>
				GenericMove.IsDetailed()
					? null
					: GenericMove.Value.ToString("0.00");
			set =>
				GenericMove.Value = value == null
					? default
					: Decimal.Parse(value);
		}

		public Boolean ShowNoCategories => IsUsingCategories && !CategorySelectList.Any();
		public Boolean ShowNoAccounts => !accountList.Any();
		public Boolean BlockScreen => ShowNoCategories || ShowNoAccounts;
		public Boolean ShowLosingCategory => !IsUsingCategories && !String.IsNullOrEmpty(CategoryName);
		public virtual Boolean ShowRemoveCheck => false;

		public WizardHL HL;

		internal abstract void Save();


		public IList<String> CreateEditSchedule()
		{
			var errors = new List<String>();

			try
			{
				if (!IsDetailed)
					GenericMove.DetailList.Clear();

				Save();
			}
			catch (CoreError e)
			{
				errors.Add(translator[e]);
			}

			return errors;
		}



		public class NatureUI
		{
			public NatureUI(MoveNature nature, String enable, String disable = null)
			{
				Nature = nature;
				Enable = enable;
				Disable = disable;
			}

			public MoveNature Nature { get; }
			public String Enable { get; }
			public String Disable { get; }
		}



		public class DetailTabUI
		{
			public DetailTabUI(String resourceKey, String targetId, Boolean isDetailed, String @class = "")
			{
				ResourceKey = resourceKey;
				TargetId = targetId;
				IsDetailed = isDetailed;
				Class = @class;
			}

			public String ResourceKey { get; }
			public String TargetId { get; }
			public Boolean IsDetailed { get; }
			public String Class { get; set; }
		}

		public class DetailUI
		{
			public DetailUI()
			{
				Detail = new DetailInfo();
				Send = false;
			}

			public DetailUI(DetailInfo detail)
			{
				Detail = detail;
				Send = true;
			}

			public DetailInfo Detail { get; set; }
			public Boolean Send { get; set; }

			public String Value
			{
				get => Detail.Value.ToString("0.00");
				set =>
					Detail.Value = value == null
						? 0
						: Decimal.Parse(value);
			}
		}
	}
}
