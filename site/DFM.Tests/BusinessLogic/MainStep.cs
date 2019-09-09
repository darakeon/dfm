using System;
using System.Linq;
using DFM.BusinessLogic;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Repositories.Mappings;
using DFM.BusinessLogic.Response;
using DFM.Entities;
using DFM.Language;
using DFM.Tests.BusinessLogic.Helpers;
using Keon.NHibernate.Base;
using NUnit.Framework;
using TechTalk.SpecFlow;
using error = DFM.BusinessLogic.Exceptions.Error;

namespace DFM.Tests.BusinessLogic
{
	[Binding]
	public class MainStep : BaseStep
	{
		[Given(@"I have a complete user logged in")]
		public void GivenIHaveAnActiveUser()
		{
			createLogoffLogin(USER_EMAIL, UserPassword);
		}

		[Given(@"there is a bad person logged in")]
		public void GivenIHaveABadPersonLoggedIn()
		{
			createLogoffLogin(BAD_PERSON_USER, UserPassword);
		}

		private void createLogoffLogin(String email, String password)
		{
			resetTicket();
			CreateUserIfNotExists(email, password, true);
			Current.Set(email, password, false);
			Service.Safe.AcceptContract();
		}

		[Given(@"the right user login again")]
		public void GivenTheRightUserLoginAgain()
		{
			createLogoffLogin(USER_EMAIL, UserPassword);
		}

		[Given(@"the user have accepted the contract")]
		public void GivenTheUserHaveAcceptedTheContract()
		{
			Service.Safe.AcceptContract();
		}

		[Given(@"I have an account")]
		public void GivenIHaveAnAccount()
		{
			Account = AccountInfo.Convert(
				GetOrCreateAccount(MAIN_ACCOUNT_URL)
			);
		}

		[Given(@"I have a category")]
		public void GivenIHaveACategory()
		{
			Category = CategoryInfo.Convert(
				GetOrCreateCategory(MAIN_CATEGORY_NAME)
			);
		}

		[Given(@"I pass a valid account url")]
		public void GivenIPassValidAccountName()
		{
			AccountUrl = Account.Url;
		}


		[Then(@"I will receive this core error: ([A-Za-z]+)")]
		public void ThenIWillReceiveThisError(error error)
		{
			Assert.IsNotNull(Error);
			Assert.AreEqual(error, Error.Type);
		}

		[Then(@"I will receive no core error")]
		public void ThenIWillReceiveNoCoreError()
		{
			Assert.IsNull(Error);
		}

		[BeforeTestRun]
		public static void Start()
		{
			setLogName();
			log("BeforeTestRun");

			DBHelper.Cleanup();

			SessionFactoryManager.Initialize<UserMap, User>();
			SessionManager.Init(getTicketKey);

			Service = new ServiceAccess(getTicket, getPath);

			PlainText.Initialize(RunPath);
		}

		[AfterTestRun]
		public static void End()
		{
			log("End");
			SessionFactoryManager.End();
		}

		[BeforeScenario]
		public static void RegisterRun()
		{
			log("Before scenario");
		}

		[AfterScenarioBlock]
		public static void CloseSession()
		{
			log("After scenario block");
			SessionManager.Close();
		}

		[AfterScenario]
		public void CleanSchedulesAndLogoff()
		{
			log("After scenario");
			if (!Current.IsAuthenticated)
				return;

			try
			{
				var pendentSchedules = Service.Robot.GetScheduleList();

				foreach (var pendentSchedule in pendentSchedules)
				{
					Service.Robot.DisableSchedule(pendentSchedule.ID);
				}
			}
			catch (CoreError e)
			{
				var ignored = new [] {
					error.NotSignedLastContract,
					error.TFANotVerified,
				};

				if (!ignored.Contains(e.Type))
				{
					throw;
				}
			}

			Current.Clear();
		}

		[StepArgumentTransformation(@"( not)?")]
		[StepArgumentTransformation(@"(not )?")]
		public bool NotToBoolTransform(string not)
		{
			return not.Trim() != "not";
		}
	}
}
