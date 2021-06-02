using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DFM.BusinessLogic.Exceptions;
using DFM.Email;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Exchange;
using Keon.Util.DB;
using Keon.Util.Extensions;
using Error = DFM.BusinessLogic.Exceptions.Error;

namespace DFM.BusinessLogic.Repositories
{
	internal class PurgeRepository : Repo<Purge>
	{
		public PurgeRepository(Repos repos, Current.GetUrl getUrl)
		{
			this.repos = repos;
			this.getUrl = getUrl;
		}

		private readonly Repos repos;
		private readonly Current.GetUrl getUrl;

		public void Execute(User user, DateTime date, RemovalReason reason, Action<String> upload)
		{
			var accounts = repos.Account.Where(a => a.User.ID == user.ID);
			String s3 = null;

			using (var csv = new CSV())
			{
				foreach (var account in accounts)
				{
					csv.Add(repos.Move.ByAccount(account));
					csv.Add(repos.Schedule.ByAccount(account));
				}

				csv.Create(user);

				if (csv.Path != null)
				{
					upload(csv.Path);
					s3 = csv.Path;
				}
			}

			var purge = new Purge
			{
				Email = user.Email,
				When = DateTime.UtcNow,
				Why = reason,
				S3 = s3,
				Password = user.Password,
				TFA = user.TFASecret,
			};

			SaveOrUpdate(purge);

			notifyPurge(user, date, reason);

			purgeAll(repos.Ticket, t => t.User, u => u.ID == user.ID);
			purgeAll(repos.Security, s => s.User, u => u.ID == user.ID);
			purgeAll(repos.Acceptance, a => a.User, u => u.ID == user.ID);

			foreach (var account in accounts)
			{
				purgeAll(repos.Summary, m => m.Account, a => a.ID == account.ID);

				purgeAll(repos.Move, m => m.In, a => a.ID == account.ID);
				purgeAll(repos.Move, m => m.Out, a => a.ID == account.ID);

				purgeAll(repos.Schedule, m => m.In, a => a.ID == account.ID);
				purgeAll(repos.Schedule, m => m.Out, a => a.ID == account.ID);
			}

			purgeAll(repos.Account, a => a.User, u => u.ID == user.ID);
			purgeAll(repos.Category, c => c.User, u => u.ID == user.ID);

			repos.User.Delete(user);
			repos.Config.Delete(user.Config);
			repos.Control.Delete(user.Control);
		}

		private void notifyPurge(User user, DateTime dateTime, RemovalReason removalReason)
		{
			var dic = new Dictionary<String, String>
			{
				{ "Url", getUrl() },
				{ "Date", dateTime.ToShortDateString() },
				{ "UserEmail", user.Email },
			};

			var format = Format.PurgeNotice(user, removalReason);

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

		private void purgeAll<E, P>(
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
