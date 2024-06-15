using System;
using System.IO;
using System.Linq;
using DFM.Authentication;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Repositories;
using DFM.BusinessLogic.Response;
using DFM.BusinessLogic.Validators;
using DFM.Email;
using DFM.Entities;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using DFM.Exchange.Exporter;
using DFM.Generic;
using DFM.Generic.Datetime;
using DFM.Queue;
using DFM.Tests.Util;
using Keon.Util.Extensions;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace DFM.BusinessLogic.Tests.Steps
{
	public abstract class BaseStep : ContextHelper
	{
		protected BaseStep(ScenarioContext context)
			: base(context)
		{
			instance = this;
		}

		protected static ServiceAccess service;
		protected static Current current => service.Current;

		private protected static Valids valids;
		private protected static Repos repos;
		private protected static IFileService fileService;
		private protected static IQueueService queueService;

		private static String logFileName;

		protected static TestService db = new(service, repos, valids);

		protected static void setLogName()
		{
			var logDate = DateTime.UtcNow.UntilSecond();

			var path =
				Path.Combine(
					Cfg.LogErrorsPath, "..", "tests"
				);

			path = Path.GetFullPath(path);

			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			logFileName = Path.Combine(path, $"tests_{logDate}.log");
		}

		protected static void setRepositories()
		{
			fileService = new LocalFileService();
			queueService = new LocalQueueService();
			valids = new Valids();
			repos = new Repos(getSite, valids, fileService);
		}

		protected static String getSite()
		{
			return "https://dontflymoney.com";
		}

		protected void log(String text)
		{
			log(scenarioTitle, text);
		}

		protected static void log(String title, String text)
		{
			var log = $"{DateTime.UtcNow:HH:mm:ss-fff}\t{title}\t{text}\n";
			File.AppendAllText(logFileName, log);
		}

		protected Int32? getInt(String str)
		{
			return String.IsNullOrEmpty(str)
				? null
				: Int32.Parse(str);
		}

		protected String getLastTokenForUser(String email, SecurityAction action)
		{
			var user = repos.User.GetByEmail(email);

			return repos.Security.Where(
				s => s.User.ID == user.ID
					&& s.Action == action
					&& s.Active
					&& s.Expire >= DateTime.UtcNow
			).FirstOrDefault()?.Token;
		}

		#region Get or Create
		protected void robotRunSchedule()
		{
			createLogoffLoginRobot();
			service.Robot.RunSchedule();
			createLogoffLogin(userEmail);
		}

		protected void robotRunWipe()
		{
			createLogoffLoginRobot();
			service.Robot.WipeUsers();
			resetTicket();
		}

		protected void createLogoffLoginRobot()
		{
			createLogoffLogin(robotEmail);

			var robot = repos.User.GetByEmail(robotEmail);
			robot.Control.IsRobot = true;
			repos.Control.SaveOrUpdate(robot.Control);
		}

		protected void createLogoffLogin(String email)
		{
			resetTicket();
			createUserIfNotExists(email, userPassword, true);
			current.Set(email, userPassword, false);
			service.Law.AcceptContract();
		}

		protected void createUserIfNotExists(
			String email,
			String password,
			Boolean shouldActivateUser = false,
			Boolean shouldSignContract = false,
			Int32? timezone = null,
			Theme? theme = null,
			String language = null,
			Int32? days = null
		)
		{
			var user = repos.User.GetByEmail(email);

			if (user != null) return;

			var info = new SignUpInfo
			{
				Email = email,
				Password = password,
				RetypePassword = password,
				Language = language ?? Defaults.SettingsLanguage
			};

			if (timezone != null)
			{
				var utc = DateTime.UtcNow;
				var tzHourToRun = 12 - utc.AddHours(-12).Hour;
				var userHour = tzHourToRun + timezone;

				info.TimeZone = $"UTC{userHour:+00;-00; 00}:00";
			}
			else
			{
				info.TimeZone = Defaults.SettingsTimeZone;
			}

			if (language != null)
			{
				info.Language = language;
			}

			service.Auth.SaveUser(info);

			user = repos.User.GetByEmail(email);

			if (theme.HasValue)
			{
				var settings = user.Settings;
				settings.Theme = theme.Value;
				db.Execute(() => repos.Settings.SaveOrUpdate(settings));
			}

			if (shouldActivateUser)
			{
				db.Execute(() => repos.Control.Activate(user));
			}

			if (timezone is > 0)
			{
				user.SetRobotCheckDay();
				db.Execute(() => repos.User.SaveOrUpdate(user));
			}

			if (shouldSignContract)
			{
				var contract = repos.Contract.GetContract();
				db.Execute(() => repos.Acceptance.Accept(user, contract));
			}

			if (days.HasValue)
			{
				var control = user.Control;
				control.Creation = DateTime.UtcNow.AddDays(days.Value);
				db.Execute(() => repos.Control.SaveOrUpdate(control));
			}
		}

		protected Account getOrCreateAccount(String url, Currency? currency = null)
		{
			return getOrCreateAccount(url, url, currency);
		}

		protected Account getOrCreateAccount(String name, String url, Currency? currency = null)
		{
			url = url.IntoUrl();
			var user = repos.User.GetByEmail(current.Email);
			var account = repos.Account.GetByUrl(url, user);
			if (account != null)
			{
				if (account.Currency != currency)
				{
					account.Currency = currency;
					repos.Account.SaveOrUpdate(account);
				}

				return account;
			}

			service.Admin.CreateAccount(
				new AccountInfo
				{
					Name = name,
					Currency = currency,
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
				new CategoryInfo { Name = name }
			);

			return repos.Category.GetByName(name, user);
		}
		#endregion

		#region Context
		protected static BaseStep instance;

		protected static ClientTicket getTicket(Boolean remember)
		{
			return new(instance.ticketKey, TicketType.Tests);
		}

		protected static String getTicketKey()
		{
			return instance == null
				? null
				: instance.mainTicket ??= Token.New();
		}

		protected void resetTicket()
		{
			mainTicket = null;
		}

		protected String ticketKey => getTicketKey();

		private String mainTicket
		{
			get => get<String>("mainTicket");
			set => set("mainTicket", value);
		}

		protected CoreError error
		{
			get => get<CoreError>("error");
			set => set("error", value);
		}

		protected SessionInfo session
		{
			get => get<SessionInfo>("Session");
			set => set("Session", value);
		}

		protected AccountInfo accountInfo
		{
			get => get<AccountInfo>("AccountInfo");
			set => set("AccountInfo", value);
		}

		protected CategoryInfo categoryInfo
		{
			get => get<CategoryInfo>("CategoryInfo");
			set => set("CategoryInfo", value);
		}


		protected Account accountOut
		{
			get => get<Account>("AccountOut");
			set => set("AccountOut", value);
		}

		protected Decimal accountOutTotal
		{
			get => get<Decimal>("AccountOutTotal");
			set => set("AccountOutTotal", value);
		}

		protected Decimal yearAccountOutTotal
		{
			get => get<Decimal>("YearAccountOutTotal");
			set => set("YearAccountOutTotal", value);
		}

		protected Decimal monthAccountOutTotal
		{
			get => get<Decimal>("MonthAccountOutTotal");
			set => set("MonthAccountOutTotal", value);
		}

		protected Decimal yearCategoryAccountOutTotal
		{
			get => get<Decimal>("YearCategoryAccountOutTotal");
			set => set("YearCategoryAccountOutTotal", value);
		}

		protected Decimal monthCategoryAccountOutTotal
		{
			get => get<Decimal>("MonthCategoryAccountOutTotal");
			set => set("MonthCategoryAccountOutTotal", value);
		}


		protected Account accountIn
		{
			get => get<Account>("AccountIn");
			set => set("AccountIn", value);
		}

		protected Decimal accountInTotal
		{
			get => get<Decimal>("AccountInTotal");
			set => set("AccountInTotal", value);
		}

		protected Decimal yearAccountInTotal
		{
			get => get<Decimal>("YearAccountInTotal");
			set => set("YearAccountInTotal", value);
		}

		protected Decimal monthAccountInTotal
		{
			get => get<Decimal>("MonthAccountInTotal");
			set => set("MonthAccountInTotal", value);
		}

		protected Decimal yearCategoryAccountInTotal
		{
			get => get<Decimal>("YearCategoryAccountInTotal");
			set => set("YearCategoryAccountInTotal", value);
		}

		protected Decimal monthCategoryAccountInTotal
		{
			get => get<Decimal>("MonthCategoryAccountInTotal");
			set => set("MonthCategoryAccountInTotal", value);
		}


		protected String accountUrl
		{
			get => get<String>("AccountUrl");
			set => set("AccountUrl", value);
		}

		protected String categoryName
		{
			get => get<String>("CategoryName");
			set => set("CategoryName", value);
		}

		protected EmailStatus? currentEmailStatus
		{
			get => get<EmailStatus?>("CurrentEmailStatus");
			set => set("CurrentEmailStatus", value);
		}

		protected MoveInfo moveInfo
		{
			get => get<MoveInfo>("MoveInfo");
			set => set("MoveInfo", value);
		}

		protected MoveResult moveResult
		{
			get => get<MoveResult>("MoveResult");
			set => set("MoveResult", value);
		}

		protected ScheduleInfo scheduleInfo
		{
			get => get<ScheduleInfo>("ScheduleInfo");
			set => set("ScheduleInfo", value);
		}

		protected ScheduleResult scheduleResult
		{
			get => get<ScheduleResult>("ScheduleResult");
			set => set("ScheduleResult", value);
		}

		protected DateTime testStart
		{
			get => get<DateTime>("testStart");
			set => set("testStart", value);
		}

		protected String token
		{
			get => get<String>("Token");
			set => set("Token", value);
		}

		protected String email
		{
			get => get<String>("Email");
			set => set("Email", value);
		}

		protected DateTime entityDate =>
			moveInfo?.GetDate() ??
			scheduleInfo?.GetDate() ??
			DateTime.MinValue;
		#endregion

		protected String userEmail =>
			$"{scenarioCode}@dontflymoney.com";

		protected const String badPersonEmail = "badperson@dontflymoney.com";
		protected const String anotherPersonEmail = "person@dontflymoney.com";
		protected const String robotEmail = "robot@dontflymoney.com";

		protected static String userPassword = "password";
		protected const String mainAccountUrl = "Account";
		protected const String mainCategoryName = "Category";

		protected const String accountOutName = "Account Out";
		protected const String accountInName = "Account In";
		protected static String accountOutUrl = accountOutName.IntoUrl();
		protected static String accountInUrl = accountInName.IntoUrl();

		protected DetailInfo getDetailFromTable(TableRow detailData)
		{
			return detailData.CreateInstance<DetailInfo>();
		}

		protected void createFor(User user, String entityName)
		{
			new Builder(repos, db.Execute, scenarioCode)
				.CreateFor(user, entityName);
		}
	}
}
