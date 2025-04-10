﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DFM.BusinessLogic.Exceptions;
using DFM.Email;
using DFM.Entities;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using DFM.Exchange.Exporter;
using DFM.Files;
using Keon.Util.Crypto;
using Keon.Util.DB;
using Keon.Util.Extensions;
using Error = DFM.BusinessLogic.Exceptions.Error;

namespace DFM.BusinessLogic.Repositories
{
	internal class WipeRepository : Repo<Wipe>
	{
		public WipeRepository(
			Repos repos, Current.GetUrl getUrl,
			IFileService wipeFileService, IFileService exportFileService
		)
		{
			this.repos = repos;
			this.getUrl = getUrl;
			this.wipeFileService = wipeFileService;
			this.exportFileService = exportFileService;
		}

		private readonly Repos repos;
		private readonly Current.GetUrl getUrl;
		private readonly IFileService wipeFileService;
		private readonly IFileService exportFileService;

		public void Execute(User user, DateTime date, RemovalReason reason)
		{
			var accounts = repos.Account.Where(a => a.User.ID == user.ID);

			var wipe = Wipe.FromUser(user);

			wipe.Why = reason;

			wipe.S3 = reason == RemovalReason.PersonAsked
				? null
				: extractToFile(wipe, accounts);

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

			var files = repos.Order.ListFiles(user);

			foreach (var file in files)
			{
				if (exportFileService.Exists(file))
				{
					exportFileService.Delete(file);
				}
			}

			wipeAll(repos.Order, o => o.User, u => u.ID == user.ID);

			wipeAll(repos.Account, a => a.User, u => u.ID == user.ID);
			wipeAll(repos.Category, c => c.User, u => u.ID == user.ID);

			var archives = repos.Archive.Where(a => a.User.ID == user.ID);
			foreach (var archive in archives)
			{
				wipeAll(repos.Line, m => m.Archive, a => a.ID == archive.ID);
			}
			wipeAll(repos.Archive, c => c.User, u => u.ID == user.ID);

			wipeAll(repos.Tips, a => a.User, u => u.ID == user.ID);

			repos.User.Delete(user);
			repos.Settings.Delete(user.Settings);
			repos.Control.Delete(user.Control);
		}

		private String extractToFile(Wipe wipe, IList<Account> accounts)
		{
			String s3 = null;

			using var csv = new CSVExporter();

			foreach (var account in accounts)
			{
				csv.Add(repos.Move.ByAccount(account));
				csv.Add(repos.Schedule.ByAccount(account));
			}

			csv.Create(wipe);

			if (csv.Path != null)
			{
				wipeFileService.Upload(csv.Path);
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

		public IList<Wipe> GetByUser(String email, String password)
		{
			if (String.IsNullOrEmpty(password))
				throw Error.WipeInvalid.Throw();

			var emailParts = email.Split("@");
			if (emailParts.Length != 2)
				throw Error.WipeInvalid.Throw();

			var username = emailParts[0];
			if (username.Length < MaxLen.WipeUsernameStart)
				throw Error.WipeInvalid.Throw();
			var usernameStart = username[..MaxLen.WipeUsernameStart];

			var domain = emailParts[1];
			if (domain.Length < MaxLen.WipeDomainStart)
				throw Error.WipeInvalid.Throw();
			var domainStart = domain[..MaxLen.WipeDomainStart];

			var wipes = Where(
				w => w.UsernameStart == usernameStart
					&& w.DomainStart == domainStart
			).Where(
				w => Crypt.Check(email, w.HashedEmail)
					&& Crypt.Check(password, w.Password)
			).ToList();

			if (wipes.Count == 0)
				throw Error.WipeInvalid.Throw();

			return wipes;
		}

		public void SendCSV(String email, Security security)
		{
			if (!wipeFileService.Exists(security.Wipe.S3))
				throw Error.CSVNotFound.Throw();

			var dic = new Dictionary<String, String>
			{
				{ "Url", getUrl() },
				{ "DateTime", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm") },
				{ "PathAction", security.Action.ToString() },
				{ "Token", security.Token },
			};

			var format = Format.WipeCSVRecover(security.Wipe);
			var fileContent = format.Layout.Format(dic);

			var sender = new Sender()
				.To(email)
				.Subject(format.Subject)
				.Body(fileContent);

			wipeFileService.Download(security.Wipe.S3);
			sender.Attach(security.Wipe.S3);

			try
			{
				sender.Send();
			}
			catch (MailError e)
			{
				throw Error.FailOnEmailSend.Throw(e);
			}
		}

		public void DeleteFile(String csv)
		{
			if (!wipeFileService.Exists(csv))
				throw Error.CSVNotFound.Throw();

			wipeFileService.Delete(csv);
		}
	}
}
