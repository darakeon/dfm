using System;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Repositories;
using DFM.BusinessLogic.Response;
using DFM.Entities;
using DFM.Entities.Bases;
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
		private readonly CategoryRepository categoryRepository;
		private readonly AccountRepository accountRepository;

		internal BaseMoveSaverService(ServiceAccess serviceAccess, MoveRepository moveRepository, DetailRepository detailRepository, SummaryRepository summaryRepository, CategoryRepository categoryRepository, AccountRepository accountRepository)
			: base(serviceAccess)
		{
			this.moveRepository = moveRepository;
			this.detailRepository = detailRepository;
			this.summaryRepository = summaryRepository;
			this.categoryRepository = categoryRepository;
			this.accountRepository = accountRepository;
		}

		internal MoveResult SaveMove(MoveInfo info, OperationType operationType)
		{
			var move = moveRepository.GetNonCached(info.ID);

			if (move == null)
			{
				move = new Move();
			}
			else
			{
				move.DetailList
					.Select(d => d.ID)
					.ToList()
					.ForEach(detailRepository.Delete);

				VerifyMove(move);

				move.CheckedIn = false;
				move.CheckedOut = false;
			}

			info.Update(move);

			linkEntities(move, info);

			return SaveMove(move, operationType);
		}

		internal MoveResult SaveMove(Move move, OperationType operationType)
		{
			breakSummaries(move);

			var today = parent.Current.Now;
			var moveIsNew = operationType != OperationType.Edition;

			if (moveIsNew || !move.IsDetailed())
			{
				move = moveRepository.SaveMainInfo(move, today);
				detailRepository.SaveDetails(move);
			}
			else
			{
				detailRepository.SaveDetails(move);
				move = moveRepository.SaveMainInfo(move, today);
			}

			if (!moveIsNew)
				BreakSummaries(move);

			var emailStatus = moveRepository.SendEmail(move, operationType);

			if (move.In != null && move.In.BeginDate > move.GetDate())
			{
				move.In.BeginDate = move.GetDate();
				accountRepository.Save(move.In);
			}

			if (move.Out != null && move.Out.BeginDate > move.GetDate())
			{
				move.Out.BeginDate = move.GetDate();
				accountRepository.Save(move.Out);
			}

			return new MoveResult(move, emailStatus);
		}

		internal Account GetAccountByUrl(String accountUrl)
		{
			return accountUrl == null
				? null
				: parent.Admin.GetAccountByUrlInternal(accountUrl);
		}

		internal Category GetCategoryByName(String categoryName)
		{
			if (parent.Current.UseCategories)
				return parent.Admin.GetCategoryByNameInternal(categoryName);

			if (!String.IsNullOrEmpty(categoryName))
				throw Error.CategoriesDisabled.Throw();

			return null;
		}



		private void linkEntities(Move move, MoveInfo info)
		{
			move.Category = GetCategoryByName(info.CategoryName);
			move.Out = GetAccountByUrl(info.OutUrl);
			move.In = GetAccountByUrl(info.InUrl);
		}

		internal void BreakSummaries(Move move)
		{
			move = moveRepository.GetNonCached(move.ID);
			breakSummaries(move);
		}

		private void breakSummaries(Move move)
		{
			var category = move.Category != null
				? categoryRepository.Get(move.Category.ID)
				: null;

			if (move.Out != null)
			{
				var accountOut = accountRepository.Get(move.Out.ID);
				summaryRepository.Break(accountOut, category, move);
			}

			if (move.In != null)
			{
				var accountIn = accountRepository.Get(move.In.ID);
				summaryRepository.Break(accountIn, category, move);
			}
		}

		internal void FixSummaries()
		{
			InTransaction(() =>
			{
				var user = parent.Safe.GetCurrent();
				summaryRepository
					.SimpleFilter(s => s.Broken)
					.Where(s => s.User() == user)
					.ToList()
					.ForEach(fixSummaries);
			});
		}

		private void fixSummaries(Summary summary)
		{
			var @in = moveRepository.GetIn(summary);
			var @out = moveRepository.GetOut(summary);

			summaryRepository.Fix(summary, @in, @out);
		}

		internal void VerifyMove(Move move)
		{
			var user = parent.Safe.GetCurrent();
			if (move == null || moveRepository.GetUser(move).ID != user.ID)
				throw Error.InvalidMove.Throw();
		}
	}
}
