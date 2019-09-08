using System;
using System.Collections.Generic;
using System.Linq;
using Keon.Util.Exceptions;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Repositories;
using DFM.Email;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Entities.Extensions;
using Error = DFM.BusinessLogic.Exceptions.Error;

namespace DFM.BusinessLogic.Services
{
	internal class BaseMoveSaverService : BaseService
	{
		private readonly MoveRepository moveRepository;
		private readonly DetailRepository detailRepository;
		private readonly SummaryRepository summaryRepository;
		private readonly MonthRepository monthRepository;
		private readonly YearRepository yearRepository;

		internal BaseMoveSaverService(ServiceAccess serviceAccess, MoveRepository moveRepository, DetailRepository detailRepository, SummaryRepository summaryRepository, MonthRepository monthRepository, YearRepository yearRepository)
			: base(serviceAccess)
		{
			this.moveRepository = moveRepository;
			this.detailRepository = detailRepository;
			this.summaryRepository = summaryRepository;
			this.monthRepository = monthRepository;
			this.yearRepository = yearRepository;
		}



		internal ComposedResult<Move, EmailStatus> SaveOrUpdateMove(
			Move move,
			string accountOutUrl, string accountInUrl, string categoryName,
			OperationType operationType
		) {
			var oldMove = moveRepository.GetNonCached(move.ID);

			if (oldMove != null)
			{
				VerifyMove(oldMove);
				move.Checked = oldMove.Checked;
			}

			linkEntities(move, accountOutUrl, accountInUrl, categoryName);

			var now = Parent.Current.User.Now();

			if (move.ID == 0 || !move.IsDetailed())
			{
				move = moveRepository.SaveOrUpdate(move, now);
				detailRepository.SaveDetails(move, oldMove);
			}
			else
			{
				detailRepository.SaveDetails(move, oldMove);
				move = moveRepository.SaveOrUpdate(move, now);
			}

			if (oldMove != null)
				breakSummaries(oldMove);

			breakSummaries(move);

			var emailStatus = moveRepository.SendEmail(move, operationType);

			return new ComposedResult<Move, EmailStatus>(move, emailStatus);
		}

		internal Account GetAccountByUrl(String accountUrl)
		{
			return accountUrl == null
				? null
				: Parent.Admin.GetAccountByUrlInternal(accountUrl);
		}

		internal Category GetCategoryByName(String categoryName)
		{
			if (Parent.Current.User.Config.UseCategories)
				return Parent.Admin.GetCategoryByNameInternal(categoryName);

			if (!String.IsNullOrEmpty(categoryName))
				throw Error.CategoriesDisabled.Throw();

			return null;
		}



		private void linkEntities(Move move, String accountOutUrl, String accountInUrl, String categoryName)
		{
			move.Category = GetCategoryByName(categoryName);

			var accountOut = GetAccountByUrl(accountOutUrl);
			var monthOut = accountOut == null ? null : getMonth(move, accountOut);

			var accountIn = GetAccountByUrl(accountInUrl);
			var monthIn = accountIn == null ? null : getMonth(move, accountIn);

			moveRepository.PlaceMonthsInMove(move, monthOut, monthIn);
		}

		private Month getMonth(Move move, Account account)
		{
			if (move.Date == DateTime.MinValue)
				return null;

			var year = yearRepository.GetOrCreateYearWithSummary((Int16)move.Date.Year, account, move.Category);

			return monthRepository.GetOrCreateMonthWithSummary((Int16)move.Date.Month, year, move.Category);
		}



		internal void BreakSummaries(Move move)
		{
			var oldMove = moveRepository.GetNonCached(move.ID);

			breakSummaries(oldMove);
		}

		private void breakSummaries(Move move)
		{
			if (move.Nature != MoveNature.Out)
			{
				var monthIn = monthRepository.Get(move.In.ID);

				summaryRepository.CreateIfNotExists(monthIn, move.Category);

				summaryRepository.Break(monthIn, move.Category);
				summaryRepository.Break(monthIn.Year, move.Category);
			}

			if (move.Nature != MoveNature.In)
			{
				var monthOut = monthRepository.Get(move.Out.ID);

				summaryRepository.CreateIfNotExists(monthOut, move.Category);

				summaryRepository.Break(monthOut, move.Category);
				summaryRepository.Break(monthOut.Year, move.Category);
			}

		}



		internal void FixSummaries()
		{
			InTransaction(() =>
			{
				var summaryList =
					summaryRepository
						.SimpleFilter(s => s.Broken)
						.Where(s => s.User() == Parent.Current.User)
						.ToList();

				var monthSummaryList = summaryList.Where(s => s.Nature == SummaryNature.Month);
				fixMonthSummaries(monthSummaryList);

				var yearSummaryList = summaryList.Where(s => s.Nature == SummaryNature.Year);
				fixYearSummaries(yearSummaryList);
			});
		}

		private void fixMonthSummaries(IEnumerable<Summary> summaryList)
		{
			foreach (var summary in summaryList)
			{
				var @in = moveRepository.GetIn(summary.Month, summary.Category);
				var @out = moveRepository.GetOut(summary.Month, summary.Category);

				summaryRepository.Fix(summary, @in, @out);
			}
		}

		private void fixYearSummaries(IEnumerable<Summary> summaryList)
		{
			foreach (var summary in summaryList)
			{
				var @in = monthRepository.GetIn(summary.Year, summary.Category);
				var @out = monthRepository.GetOut(summary.Year, summary.Category);

				summaryRepository.Fix(summary, @in, @out);
			}
		}

		internal void VerifyMove(Move move)
		{
			if (move == null || moveRepository.GetUser(move).ID != Parent.Current.User.ID)
				throw Error.InvalidMove.Throw();
		}
	}
}
