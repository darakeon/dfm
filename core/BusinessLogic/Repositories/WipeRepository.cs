using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DFM.BusinessLogic.Exceptions;
using DFM.Email;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Exchange;
using Keon.Util.Crypto;
using Keon.Util.DB;
using Keon.Util.Extensions;
using Error = DFM.BusinessLogic.Exceptions.Error;

namespace DFM.BusinessLogic.Repositories
{
	internal class WipeRepository : Repo<Wipe>
	{
		public WipeRepository(Repos repos, Current.GetUrl getUrl)
		{
			this.repos = repos;
			this.getUrl = getUrl;
		}

		private readonly Repos repos;
		private readonly Current.GetUrl getUrl;

		public void Execute(User user, DateTime date, Action<String> upload, RemovalReason reason)
		{
			var accounts = repos.Account.Where(a => a.User.ID == user.ID);

			var wipe = new Wipe
			{
				HashedEmail = Crypt.Do(user.Email),
				UsernameStart = user.Username.Substring(0, 2),
				DomainStart = user.Domain.Substring(0, 3),
				When = DateTime.UtcNow,
				Why = reason,
				Password = user.Password,
			};

			wipe.S3 = reason == RemovalReason.PersonAsked
				? null
				: extractToFile(wipe, accounts, upload);

			SaveOrUpdate(wipe);

			notifyWipe(user, date, reason);

			wipeAll(repos.Ticket, t => t.User, u => u.ID == user.ID);
			wipeAll(repos.Security, s => s.User, u => u.ID == user.ID);
			wipeAll(repos.Acceptance, a => a.User, u => u.ID == user.ID);

			foreach (var account in accounts)
			{
				wipeAll(repos.Summary, m => m.Account, a => a.ID == account.ID);

				wipeAll(repos.Move, m => m.In, a => a.ID == account.ID);
				wipeAll(repos.Move, m => m.Out, a => a.ID == account.ID);

				wipeAll(repos.Schedule, m => m.In, a => a.ID == account.ID);
				wipeAll(repos.Schedule, m => m.Out, a => a.ID == account.ID);
			}

			wipeAll(repos.Account, a => a.User, u => u.ID == user.ID);
			wipeAll(repos.Category, c => c.User, u => u.ID == user.ID);

			repos.User.Delete(user);
			repos.Settings.Delete(user.Settings);
			repos.Control.Delete(user.Control);
		}

		private String extractToFile(Wipe wipe, IList<Account> accounts, Action<String> upload)
		{
			String s3 = null;

			using var csv = new CSV();

			foreach (var account in accounts)
			{
				csv.Add(repos.Move.ByAccount(account));
				csv.Add(repos.Schedule.ByAccount(account));
			}

			csv.Create(wipe);

			if (csv.Path != null)
			{
				upload(csv.Path);
				s3 = csv.Path;
			}

			return s3;
		}

		private void notifyWipe(User user, DateTime dateTime, RemovalReason removalReason)
		{
			var dic = new Dictionary<String, String>
			{
				{ "Url", getUrl() },
				{ "Date", dateTime.ToShortDateString() },
				{ "UserEmail", user.Email },
			};

			var format = Format.WipeNotice(user, removalReason);

			var fileContent = format.Layout.Format(dic);

			var sender = new Sender()
				.To(user.Email)
				.Subject(format.Subject)
				.Body(fileContent);

			try
			{
				sender.Send();
			}
			catch (MailError e)
			{
				throw Error.FailOnEmailSend.Throw(e);
			}
		}

		private void wipeAll<E, P>(
			Repo<E> repo,
			Expression<Func<E, P>> parent,
			Expression<Func<P, Boolean>> condition
		)
			where E : class, IEntity<Int64>, new()
		{
			repo.NewQuery()
				.Where(parent, condition)
				.List.ToList()
				.ForEach(repo.Delete);
		}
	}
}
