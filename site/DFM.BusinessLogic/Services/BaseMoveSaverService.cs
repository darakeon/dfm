using System;
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

		internal BaseMoveSaverService(ServiceAccess serviceAccess, MoveRepository moveRepository, DetailRepository detailRepository, SummaryRepository summaryRepository)
			: base(serviceAccess)
		{
			this.moveRepository = moveRepository;
			this.detailRepository = detailRepository;
			this.summaryRepository = summaryRepository;
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
				BreakSummaries(oldMove);

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
			move.Out = GetAccountByUrl(accountOutUrl);
			move.In = GetAccountByUrl(accountInUrl);

			breakSummaries(move);
		}

		internal void BreakSummaries(Move move)
		{
			move = moveRepository.GetNonCached(move.ID);
			breakSummaries(move);
		}

		private void breakSummaries(Move move)
		{
			if (move.Out != null)
			{
				summaryRepository.Break(move.Out, move.Category, move.Date);
			}

			if (move.In != null)
			{
				summaryRepository.Break(move.In, move.Category, move.Date);
			}
		}

		internal void FixSummaries()
		{
			InTransaction(() =>
			{
				summaryRepository
					.SimpleFilter(s => s.Broken)
					.Where(s => s.User() == Parent.Current.User)
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
			if (move == null || moveRepository.GetUser(move).ID != Parent.Current.User.ID)
				throw Error.InvalidMove.Throw();
		}
	}
}
