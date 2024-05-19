using System;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Repositories;
using DFM.BusinessLogic.Response;
using DFM.BusinessLogic.Validators;
using DFM.Entities;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using DFM.Entities.Extensions;
using Error = DFM.BusinessLogic.Exceptions.Error;

namespace DFM.BusinessLogic.Services
{
	internal class BaseMoveSaverService : Service
	{
		internal BaseMoveSaverService(ServiceAccess serviceAccess, Repos repos, Valids valids)
			: base(serviceAccess, repos, valids) { }

		internal MoveResult SaveMove(MoveInfo info, OperationType operationType)
		{
			var move = repos.Move.GetNonCached(info.Guid);

			if (move == null)
			{
				move = new Move();
			}
			else
			{
				move.DetailList
					.Select(d => d.ID)
					.ToList()
					.ForEach(repos.Detail.Delete);

				VerifyUser(move);

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

			var moveIsNew = operationType != OperationType.Edition;

			if (moveIsNew || !move.IsDetailed())
			{
				move = repos.Move.SaveMainInfo(move);
				repos.Detail.SaveDetails(move);
			}
			else
			{
				repos.Detail.SaveDetails(move);
				move = repos.Move.SaveMainInfo(move);
			}

			if (!moveIsNew)
				BreakSummaries(move);

			var user = parent.Auth.GetCurrent();
			var security = repos.Security.Grab(
				user, SecurityAction.UnsubscribeMoveMail
			);

			var emailStatus = repos.Move.SendEmail(move, operationType, security);

			if (move.In != null && move.In.BeginDate > move.GetDate())
			{
				move.In.BeginDate = move.GetDate();
				repos.Account.Save(move.In);
			}

			if (move.Out != null && move.Out.BeginDate > move.GetDate())
			{
				move.Out.BeginDate = move.GetDate();
				repos.Account.Save(move.Out);
			}

			return new MoveResult(move, emailStatus);
		}

		internal Account GetAccount(String url)
		{
			return url == null
				? null
				: parent.Admin.GetAccountEntity(url);
		}

		internal Category GetCategory(String name)
		{
			if (parent.Current.UseCategories)
				return parent.Admin.GetCategoryEntity(name);

			if (!String.IsNullOrEmpty(name))
				throw Error.CategoriesDisabled.Throw();

			return null;
		}

		private void linkEntities(Move move, MoveInfo info)
		{
			move.Category = GetCategory(info.CategoryName);
			move.Out = GetAccount(info.OutUrl);
			move.In = GetAccount(info.InUrl);
		}

		internal void BreakSummaries(Move move)
		{
			move = repos.Move.GetNonCached(move.Guid);
			breakSummaries(move);
		}

		private void breakSummaries(Move move)
		{
			var category = move.Category != null
				? repos.Category.Get(move.Category.ID)
				: null;

			if (move.Out != null)
			{
				var accountOut = repos.Account.Get(move.Out.ID);
				repos.Summary.Break(accountOut, category, move);
			}

			if (move.In != null)
			{
				var accountIn = repos.Account.Get(move.In.ID);
				repos.Summary.Break(accountIn, category, move);
			}
		}

		internal void FixSummaries()
		{
			var user = parent.Auth.GetCurrent();
			FixSummaries(user);
		}

		internal void FixSummaries(User user)
		{
			inTransaction("FixSummaries", () =>
			{
				repos.Summary
					.Where(s => s.Broken)
					.Where(s => s.User() == user)
					.ToList()
					.ForEach(fixSummaries);
			});
		}

		private void fixSummaries(Summary summary)
		{
			var @in = repos.Move.GetIn(summary);
			var @out = repos.Move.GetOut(summary);

			repos.Summary.Fix(summary, @in, @out);
		}

		internal void VerifyUser(Move move)
		{
			var user = parent.Auth.GetCurrent();
			if (move == null || repos.Move.GetUser(move).ID != user.ID)
				throw Error.MoveNotFound.Throw();
		}
	}
}
