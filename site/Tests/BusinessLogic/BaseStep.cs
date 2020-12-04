using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DFM.Authentication;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Helpers;
using DFM.BusinessLogic.Repositories;
using DFM.BusinessLogic.Response;
using DFM.Email;
using DFM.Entities;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using DFM.Tests.Util;
using Keon.Util.Extensions;
using TechTalk.SpecFlow;

namespace DFM.BusinessLogic.Tests
{
	public abstract class BaseStep : ContextHelper
	{
		protected static ServiceAccess service;
		protected static Current current => service.Current;

		private protected static Repos repos;

		private static String logFileName;

		protected static void setLogName()
		{
			var logDate = $"{DateTime.Now:yyyy-MM-dd-HH-mm-ss}";

			var path =
				Path.Combine(
					AppDomain.CurrentDomain.BaseDirectory,
					@"..", @"..", @"..", @"..", "log"
				);

			path = Path.GetFullPath(path);

			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			logFileName = Path.Combine(path, $"tests_{logDate}.log");
		}

		protected static void setRepositories()
		{
			repos = new Repos(getSite);
		}

		protected static String getSite()
		{
			return "https://dontflymoney.com";
		}

		protected static void log(String text)
		{
			var log = $"{DateTime.Now:HH:mm:ss-fff}\t{scenarioTitle}\t{text}\n";
			File.AppendAllText(logFileName, log);
		}

		protected Int32? getInt(String str)
		{
			return String.IsNullOrEmpty(str)
				? (Int32?) null
				: Int32.Parse(str);
		}

		protected static String makeUrlFromName(String name)
		{
			var regex = new Regex("[^A-Za-z0-9_]");
			var url = regex.Replace(name, "");

			return url;
		}

		protected String getLastTokenForUser(String email, SecurityAction action)
		{
			var user = repos.User.GetByEmail(email);

			return repos.Security.Where(
				s => s.User.ID == user.ID
				     && s.Action == action
				     && s.Active
				     && s.Expire >= current.Now
			).FirstOrDefault()?.Token;
		}

		#region Get or Create
		protected void createUserIfNotExists(
			String email,
			String password,
			Boolean shouldActivateUser = false,
			Boolean shouldSignContract = false
		)
		{
			var user = repos.User.GetByEmail(email);

			if (user == null)
			{
				var info = new SignUpInfo
				{
					Email = email,
					Password = password,
					RetypePassword = password,
					Language = Defaults.ConfigLanguage,
				};

				service.Safe.SaveUser(info);

				user = repos.User.GetByEmail(email);

				if (shouldActivateUser)
				{
					repos.User.Activate(user);
				}

				if (shouldSignContract)
				{
					var contract = repos.Contract.GetContract();
					repos.Acceptance.Accept(user, contract);
				}
			}
		}

		protected Account getOrCreateAccount(String url)
		{
			var user = repos.User.GetByEmail(current.Email);
			var account = repos.Account.GetByUrl(url, user);
			if (account != null) return account;

			service.Admin.CreateAccount(
				new AccountInfo
				{
					Name = url,
					Url = url,
				}
			);

			return repos.Account.GetByUrl(url, user);
		}

		protected Category getOrCreateCategory(String name)
		{
			var user = repos.User.GetByEmail(current.Email);
			var category = repos.Category.GetByName(name, user);
			if (category != null) return category;

			service.Admin.CreateCategory(
				new CategoryInfo
				{
					Name = name
				}
			);

			return repos.Category.GetByName(name, user);
		}
		#endregion

		#region Context
		protected static ClientTicket getTicket(Boolean remember)
		{
			return new ClientTicket(ticketKey, TicketType.Local);
		}

		protected static String getTicketKey()
		{
			return mainTicket ??= Token.New();
		}

		protected void resetTicket()
		{
			mainTicket = null;
		}

		protected static String ticketKey => getTicketKey();

		private static String mainTicket
		{
			get => get<String>("mainTicket");
			set => set("mainTicket", value);
		}

		protected static CoreError error
		{
			get => get<CoreError>("error");
			set => set("error", value);
		}

		protected static SessionInfo session
		{
			get => get<SessionInfo>("Session");
			set => set("Session", value);
		}

		protected static AccountInfo accountInfo
		{
			get => get<AccountInfo>("AccountInfo");
			set => set("AccountInfo", value);
		}

		protected static CategoryInfo categoryInfo
		{
			get => get<CategoryInfo>("CategoryInfo");
			set => set("CategoryInfo", value);
		}


		protected static Account accountOut
		{
			get => get<Account>("AccountOut");
			set => set("AccountOut", value);
		}

		protected static Decimal accountOutTotal
		{
			get => get<Decimal>("AccountOutTotal");
			set => set("AccountOutTotal", value);
		}

		protected static Decimal yearAccountOutTotal
		{
			get => get<Decimal>("YearAccountOutTotal");
			set => set("YearAccountOutTotal", value);
		}

		protected static Decimal monthAccountOutTotal
		{
			get => get<Decimal>("MonthAccountOutTotal");
			set => set("MonthAccountOutTotal", value);
		}

		protected static Decimal yearCategoryAccountOutTotal
		{
			get => get<Decimal>("YearCategoryAccountOutTotal");
			set => set("YearCategoryAccountOutTotal", value);
		}

		protected static Decimal monthCategoryAccountOutTotal
		{
			get => get<Decimal>("MonthCategoryAccountOutTotal");
			set => set("MonthCategoryAccountOutTotal", value);
		}


		protected static Account accountIn
		{
			get => get<Account>("AccountIn");
			set => set("AccountIn", value);
		}

		protected static Decimal accountInTotal
		{
			get => get<Decimal>("AccountInTotal");
			set => set("AccountInTotal", value);
		}

		protected static Decimal yearAccountInTotal
		{
			get => get<Decimal>("YearAccountInTotal");
			set => set("YearAccountInTotal", value);
		}

		protected static Decimal monthAccountInTotal
		{
			get => get<Decimal>("MonthAccountInTotal");
			set => set("MonthAccountInTotal", value);
		}

		protected static Decimal yearCategoryAccountInTotal
		{
			get => get<Decimal>("YearCategoryAccountInTotal");
			set => set("YearCategoryAccountInTotal", value);
		}

		protected static Decimal monthCategoryAccountInTotal
		{
			get => get<Decimal>("MonthCategoryAccountInTotal");
			set => set("MonthCategoryAccountInTotal", value);
		}


		protected static String accountUrl
		{
			get => get<String>("AccountUrl");
			set => set("AccountUrl", value);
		}

		protected static String categoryName
		{
			get => get<String>("CategoryName");
			set => set("CategoryName", value);
		}

		protected static EmailStatus? currentEmailStatus
		{
			get => get<EmailStatus?>("CurrentEmailStatus");
			set => set("CurrentEmailStatus", value);
		}

		protected static MoveInfo moveInfo
		{
			get => get<MoveInfo>("MoveInfo");
			set => set("MoveInfo", value);
		}

		protected static MoveResult moveResult
		{
			get => get<MoveResult>("MoveResult");
			set => set("MoveResult", value);
		}

		protected static ScheduleInfo scheduleInfo
		{
			get => get<ScheduleInfo>("ScheduleInfo");
			set => set("ScheduleInfo", value);
		}

		protected static ScheduleResult scheduleResult
		{
			get => get<ScheduleResult>("ScheduleResult");
			set => set("ScheduleResult", value);
		}

		protected static DateTime startDateTime
		{
			get => get<DateTime>("startDateTime");
			set => set("startDateTime", value);
		}

		protected static String userEmailByTest
		{
			get => get<String>("userEmailByTest");
			set => set("userEmailByTest", value);
		}

		protected static String token
		{
			get => get<String>("Token");
			set => set("Token", value);
		}

		protected static DateTime entityDate =>
			moveInfo?.GetDate() ??
			scheduleInfo?.GetDate() ??
			DateTime.MinValue;
		#endregion

		protected const String userEmail = "test@dontflymoney.com";
		protected const String badPersonEmail = "badperson@dontflymoney.com";
		protected const String anotherPersonEmail = "person@dontflymoney.com";

		protected static String userPassword = "password";
		protected const String mainAccountUrl = "first_account";
		protected const String mainCategoryName = "first category";

		protected const String accountOutName = "Account Out";
		protected const String accountInName = "Account In";
		protected static String accountOutUrl = makeUrlFromName(accountOutName);
		protected static String accountInUrl = makeUrlFromName(accountInName);

		#region Helpers
		protected DetailInfo getDetailFromTable(TableRow detailData)
		{
			var newDetail = new DetailInfo { Description = detailData["Description"] };

			if (!String.IsNullOrEmpty(detailData["Value"]))
				newDetail.Value = Decimal.Parse(detailData["Value"]);

			if (!String.IsNullOrEmpty(detailData["Amount"]))
				newDetail.Amount = Int16.Parse(detailData["Amount"]);
			return newDetail;
		}
		#endregion
	}
}
