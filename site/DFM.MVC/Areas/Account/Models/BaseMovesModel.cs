using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Keon.MVC.Forms;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using DFM.Entities;
using DFM.Entities.Extensions;
using DFM.Generic;
using DFM.MVC.Helpers.Global;
using account = DFM.Entities.Account;

namespace DFM.MVC.Areas.Account.Models
{
	public abstract class BaseMovesModel : BaseAccountModel
	{
		private readonly IList<account> accountList;


		private BaseMovesModel()
		{
			accountList = admin.GetAccountList(true);

			var transferIsPossible = accountList.Count > 1;

			populateDropDowns(transferIsPossible);
		}

		protected BaseMovesModel(IMove iMove)
			: this()
		{
			GenericMove = iMove;

			if (Date == DateTime.MinValue)
				Date = today;

			AccountOutUrl = iMove.AccOut()?.Url;
			AccountInUrl = iMove.AccIn()?.Url;

			if (iMove.Category != null)
				CategoryName = iMove.Category.Name;

			arrangeDetails();
		}

		protected BaseMovesModel(IMove iMove, OperationType type)
			: this(iMove)
		{
			Type = type;
		}

		public void SetDefaultAccount()
		{
			AccountOutUrl = CurrentAccountUrl;
			AccountInUrl = CurrentAccountUrl;
		}



		private void populateDropDowns(Boolean transferIsPossible)
		{
			if (GenericMove == null)
				GenericMove = initIMove();

			makeNatureList(transferIsPossible);

			makeCategoryList();

			makeAccountList();

			arrangeDetails();
		}

		private IMove initIMove()
		{
			if (GetType() == typeof(SchedulesCreateModel))
				return new Schedule();

			return new Move();
		}

		private void makeNatureList(bool transferIsPossible)
		{
			NatureSelectList =
				transferIsPossible
					? SelectListExtension.CreateSelect(Translator.GetEnumNames<MoveNature>())
					: SelectListExtension.CreateSelect(Translator.GetEnumNames<PrimalMoveNature>());
		}

		private void makeCategoryList()
		{
			var categoryList = admin.GetCategoryList(true);

			CategorySelectList = SelectListExtension
				.CreateSelect(categoryList, mv => mv.Name, mv => mv.Name);
		}

		private void makeAccountList()
		{
			AccountOutSelectList = SelectListExtension.CreateSelect(accountList, a => a.Url, a => a.Name);
			AccountInSelectList = SelectListExtension.CreateSelect(accountList, a => a.Url, a => a.Name);
		}

		private void arrangeDetails()
		{
			IsDetailed = GenericMove.ID != 0 && GenericMove.IsDetailed();
		}


		public OperationType Type { get; set; }


		protected internal IMove GenericMove { get; set; }

		public const Int32 DETAIL_COUNT = 100;

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
						detailList.Add(new DetailUI(new Detail()));
					}

					for (var d = detailList.Count; d < DETAIL_COUNT; d++)
					{
						detailList.Add(new DetailUI());
					}
				}

				return detailList;
			}
			set
			{
				GenericMove.DetailList = value.Where(d => d.Send).Select(d => d.Detail).ToList();
			}
		}


		[Required(ErrorMessage = "*")]
		public String Description
		{
			get { return GenericMove.Description; }
			set { GenericMove.Description = value; }
		}


		[Required(ErrorMessage = "*")]
		public DateTime Date
		{
			get { return GenericMove.Date; }
			set { GenericMove.Date = value; }
		}


		[Required(ErrorMessage = "*")]
		public MoveNature Nature
		{
			get { return GenericMove.Nature; }
			set { GenericMove.Nature = value; }
		}

		public SelectList NatureSelectList { get; set; }



		[Required(ErrorMessage = "*")]
		public SelectList CategorySelectList { get; set; }
		public String CategoryName { get; set; }

		public SelectList AccountOutSelectList { get; set; }
		public String AccountOutUrl { get; set; }

		public SelectList AccountInSelectList { get; set; }
		public String AccountInUrl { get; set; }


		public Boolean IsDetailed { get; set; }


		public Decimal? Value
		{
			get { return GenericMove.Value; }
			set { GenericMove.Value = value; }
		}

		public Boolean ShowNoCategories => IsUsingCategories && !CategorySelectList.Any();
		public Boolean ShowNoAccounts => !accountList.Any();
		public Boolean BlockScreen => ShowNoCategories || ShowNoAccounts;
		public Boolean ShowLosingCategory => !IsUsingCategories && !String.IsNullOrEmpty(CategoryName);


		internal abstract void SaveOrUpdate();


		public IList<String> CreateEditSchedule()
		{
			var errors = new List<String>();

			try
			{
				if (!IsDetailed)
					GenericMove.DetailList.Clear();

				SaveOrUpdate();
			}
			catch (DFMCoreException e)
			{
				errors.Add(Translator.Dictionary[e]);
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

			public MoveNature Nature { get; private set; }
			public String Enable { get; private set; }
			public String Disable { get; private set; }
		}



		public class DetailTabUI
		{
			public DetailTabUI(String resourceKey, String targetId, Boolean isDetailed)
			{
				ResourceKey = resourceKey;
				TargetId = targetId;
				IsDetailed = isDetailed;
			}

			public String ResourceKey { get; private set; }
			public String TargetId { get; private set; }
			public Boolean IsDetailed { get; private set; }
		}

		public class DetailUI
		{
			public DetailUI()
			{
				Detail = new Detail();
				Send = false;
			}

			public DetailUI(Detail detail)
			{
				Detail = detail;
				Send = true;
			}

			public Detail Detail { get; set; }
			public Boolean Send { get; set; }
		}


	}
}
