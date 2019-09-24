using System;
using System.IO;
using System.Linq;
using DFM.Authentication;
using DFM.BusinessLogic;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Helpers;
using DFM.Email;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Tests.Helpers;
using System.Text.RegularExpressions;
using DFM.BusinessLogic.Repositories;
using DFM.BusinessLogic.Response;
using DFM.Entities.Bases;
using Keon.Util.Extensions;
using TechTalk.SpecFlow;
using error = DFM.BusinessLogic.Exceptions.Error;

namespace DFM.Tests.BusinessLogic
{
	public abstract class BaseStep : ContextHelper
	{
		protected static ServiceAccess Service;
		protected static Current Current => Service.Current;

		private protected static AccountRepository accountRepository;
		private protected static CategoryRepository categoryRepository;
		private protected static SummaryRepository summaryRepository;
		private protected static MoveRepository moveRepository;
		private protected static ScheduleRepository scheduleRepository;
		private protected static UserRepository userRepository;
		private protected static TicketRepository ticketRepository;
		private protected static SecurityRepository securityRepository;
		private protected static ContractRepository contractRepository;

		private static String logFileName;

		protected static void setLogName()
		{
			var date = $"{DateTime.Now:yyyy-MM-dd-HH-mm-ss}";

			var path =
				Path.Combine(
					AppDomain.CurrentDomain.BaseDirectory,
					@"..\..\",
					"log"
				);

			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			logFileName = Path.Combine(path, $"tests_{date}.log");
			accountRepository = new AccountRepository();
			categoryRepository = new CategoryRepository();
			summaryRepository = new SummaryRepository();
			moveRepository = new MoveRepository();
			scheduleRepository = new ScheduleRepository();
			userRepository = new UserRepository();
			ticketRepository = new TicketRepository();
			securityRepository = new SecurityRepository();
			contractRepository = new ContractRepository();
		}

		protected static void log(String text)
		{
			var title = ScenarioContext.Current?.ScenarioInfo?.Title;
			var log = $"{DateTime.Now:HH:mm:ss-fff}\t{title}\t{text}\n";
			File.AppendAllText(logFileName, log);
		}

		protected Int32? GetInt(String str)
		{
			return String.IsNullOrEmpty(str)
				? (Int32?) null
				: Int32.Parse(str);
		}

		protected static String MakeUrlFromName(String name)
		{
			var regex = new Regex("[^A-Za-z0-9_]");
			var url = regex.Replace(name, "");

			return url;
		}

		protected String getLastTokenForUser(String email, SecurityAction action)
		{
			var user = userRepository.GetByEmail(email);

			return securityRepository.SimpleFilter(
				s => s.User.ID == user.ID
				     && s.Action == action
				     && s.Active
				     && s.Expire >= Current.Now
			).FirstOrDefault()?.Token;
		}

		#region Get or Create
		protected void CreateUserIfNotExists(
			String userEmail,
			String userPassword,
			Boolean shouldActivateUser = false
		)
		{
			var userError = verifyUser(userEmail, userPassword);

			if (userError == null)
				return;

			switch (userError.Type)
			{
				case error.InvalidUser:
					var info = new SignUpInfo
					{
						Email = userEmail,
						Password = userPassword,
						RetypePassword = userPassword,
						Language = Defaults.CONFIG_LANGUAGE,
					};

					Service.Safe.SaveUserAndSendVerify(info);

					if (shouldActivateUser)
					{
						var token = getLastTokenForUser(
							userEmail,
							SecurityAction.UserVerification
						);
						Service.Safe.ActivateUser(token);
					}

					return;

				case error.DisabledUser:
					return;

				default:
					throw userError;
			}
		}

		private CoreError verifyUser(String userEmail, String userPassword)
		{
			try
			{
				var info = new SignInInfo
				{
					Email = userEmail,
					Password = userPassword,
					TicketKey = TicketKey,
					TicketType = TicketType.Local,
				};

				Service.Safe.ValidateUserAndCreateTicket(info);

				return null;
			}
			catch (CoreError e)
			{
				return e;
			}
		}


		protected Account GetOrCreateAccount(String accountUrl)
		{
			var user = userRepository.GetByEmail(Current.Email);
			var account = accountRepository.GetByUrl(accountUrl, user);
			if (account != null) return account;

			Service.Admin.CreateAccount(
				new AccountInfo
				{
					Name = accountUrl,
					Url = accountUrl,
				}
			);

			return accountRepository.GetByUrl(accountUrl, user);
		}

		protected Category GetOrCreateCategory(String categoryName)
		{
			var user = userRepository.GetByEmail(Current.Email);
			var category = categoryRepository.GetByName(categoryName, user);
			if (category != null) return category;

			Service.Admin.CreateCategory(
				new CategoryInfo
				{
					Name = categoryName
				}
			);

			return categoryRepository.GetByName(categoryName, user);
		}

		protected SessionInfo GetSavedUser(String email, String password)
		{
			var info = new SignInInfo
			{
				Email = email,
				Password = password,
				TicketKey = TicketKey,
				TicketType = TicketType.Local,
			};

			var key = Service.Safe.ValidateUserAndCreateTicket(info);

			return Service.Safe.GetSessionByTicket(key);
		}
		#endregion

		#region Context
		protected static ClientTicket getTicket(Boolean remember)
		{
			return new ClientTicket(TicketKey, TicketType.Local);
		}

		protected static String getPath(PathType pathType)
		{
			return null;
		}

		protected static String getTicketKey()
		{
			return mainTicket ?? (mainTicket = Token.New());
		}

		protected void resetTicket()
		{
			mainTicket = null;
		}

		protected static String TicketKey => getTicketKey();

		private static String mainTicket
		{
			get => Get<String>("mainTicket");
			set => Set("mainTicket", value);
		}

		protected static CoreError Error
		{
			get => Get<CoreError>("error");
			set => Set("error", value);
		}

		protected static SessionInfo Session
		{
			get => Get<SessionInfo>("Session");
			set => Set("Session", value);
		}

		protected static AccountInfo Account
		{
			get => Get<AccountInfo>("Account");
			set => Set("Account", value);
		}

		protected static CategoryInfo Category
		{
			get => Get<CategoryInfo>("Category");
			set => Set("Category", value);
		}


		protected static Account AccountOut
		{
			get => Get<Account>("AccountOut");
			set => Set("AccountOut", value);
		}

		protected static Decimal AccountOutTotal
		{
			get => Get<Decimal>("AccountOutTotal");
			set => Set("AccountOutTotal", value);
		}

		protected static Decimal YearAccountOutTotal
		{
			get => Get<Decimal>("YearAccountOutTotal");
			set => Set("YearAccountOutTotal", value);
		}

		protected static Decimal MonthAccountOutTotal
		{
			get => Get<Decimal>("MonthAccountOutTotal");
			set => Set("MonthAccountOutTotal", value);
		}

		protected static Decimal YearCategoryAccountOutTotal
		{
			get => Get<Decimal>("YearCategoryAccountOutTotal");
			set => Set("YearCategoryAccountOutTotal", value);
		}

		protected static Decimal MonthCategoryAccountOutTotal
		{
			get => Get<Decimal>("MonthCategoryAccountOutTotal");
			set => Set("MonthCategoryAccountOutTotal", value);
		}


		protected static Account AccountIn
		{
			get => Get<Account>("AccountIn");
			set => Set("AccountIn", value);
		}

		protected static Decimal AccountInTotal
		{
			get => Get<Decimal>("AccountInTotal");
			set => Set("AccountInTotal", value);
		}

		protected static Decimal YearAccountInTotal
		{
			get => Get<Decimal>("YearAccountInTotal");
			set => Set("YearAccountInTotal", value);
		}

		protected static Decimal MonthAccountInTotal
		{
			get => Get<Decimal>("MonthAccountInTotal");
			set => Set("MonthAccountInTotal", value);
		}

		protected static Decimal YearCategoryAccountInTotal
		{
			get => Get<Decimal>("YearCategoryAccountInTotal");
			set => Set("YearCategoryAccountInTotal", value);
		}

		protected static Decimal MonthCategoryAccountInTotal
		{
			get => Get<Decimal>("MonthCategoryAccountInTotal");
			set => Set("MonthCategoryAccountInTotal", value);
		}


		protected static String AccountUrl
		{
			get => Get<String>("AccountUrl");
			set => Set("AccountUrl", value);
		}

		protected static String CategoryName
		{
			get => Get<String>("CategoryName");
			set => Set("CategoryName", value);
		}



		protected static EmailStatus? CurrentEmailStatus
		{
			get => Get<EmailStatus?>("CurrentEmailStatus");
			set => Set("CurrentEmailStatus", value);
		}

		protected static MoveInfo moveInfo
		{
			get => Get<MoveInfo>("MoveInfo");
			set => Set("MoveInfo", value);
		}

		protected static MoveResult moveResult
		{
			get => Get<MoveResult>("MoveResult");
			set => Set("MoveResult", value);
		}

		protected static ScheduleInfo scheduleInfo
		{
			get => Get<ScheduleInfo>("ScheduleInfo");
			set => Set("ScheduleInfo", value);
		}

		protected static DateTime Date =>
			moveInfo?.Date ??
			scheduleInfo?.Date ??
			DateTime.MinValue;
		#endregion

		protected const String USER_EMAIL = "test@dontflymoney.com";
		protected const String BAD_PERSON_USER = "badperson@dontflymoney.com";

		protected static String UserPassword = "password";
		protected const String MAIN_ACCOUNT_URL = "first_account";
		protected const String MAIN_CATEGORY_NAME = "first category";

		protected const String ACCOUNT_OUT_NAME = "Account Out";
		protected const String ACCOUNT_IN_NAME = "Account In";
		protected static String AccountOutUrl = MakeUrlFromName(ACCOUNT_OUT_NAME);
		protected static String AccountInUrl = MakeUrlFromName(ACCOUNT_IN_NAME);

		#region Helpers
		protected DetailInfo GetDetailFromTable(TableRow detailData)
		{
			var newDetail = new DetailInfo { Description = detailData["Description"] };

			if (!String.IsNullOrEmpty(detailData["Value"]))
				newDetail.Value = Decimal.Parse(detailData["Value"]);

			if (!String.IsNullOrEmpty(detailData["Amount"]))
				newDetail.Amount = Int16.Parse(detailData["Amount"]);
			return newDetail;
		}

		protected Boolean IsCurrent(ScenarioBlock block)
		{
			return ScenarioContext.Current.CurrentScenarioBlock == block;
		}

		#endregion
	}
}
