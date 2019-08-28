using System;
using System.IO;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using Keon.NHibernate.Base;
using NUnit.Framework;
using TechTalk.SpecFlow;

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
			Account = GetOrCreateAccount(MAIN_ACCOUNT_URL);
		}

		[Given(@"I have a category")]
		public void GivenIHaveACategory()
		{
			Category = GetOrCreateCategory(MAIN_CATEGORY_NAME);
		}

		[Given(@"I pass a valid account url")]
		public void GivenIPassValidAccountName()
		{
			AccountUrl = Account.Url;
		}


		[Then(@"I will receive this core error: ([A-Za-z]+)")]
		public void ThenIWillReceiveThisError(DfMError error)
		{
			Assert.IsNotNull(Error);
			Assert.AreEqual(error, Error.Type);
		}

		[Then(@"I will receive no core error")]
		public void ThenIWillReceiveNoCoreError()
		{
			Assert.IsNull(Error);
		}



		private static String logFileName;
		
		[BeforeTestRun]
		public static void SetLogName()
		{
			var date = DateTime.Now.ToString("yyyyMMddHHmmssffff");

			var path = 
				Path.Combine(
					AppDomain.CurrentDomain.BaseDirectory,
					@"..\..\",
					"log"
				);

			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			logFileName = Path.Combine(path, $"tests_{date}.log");
		}

		[BeforeScenario]
		public static void RegisterRun()
		{
			var title = ScenarioContext.Current
				.ScenarioInfo.Title;

			File.AppendAllLines(
				logFileName,
				new[] { title }
			);
		}

		[AfterScenarioBlock]
		public static void CloseSession()
		{
			SessionManager.Close();
		}

		[AfterScenario]
		public void CleanSchedulesAndLogoff()
		{
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
			catch (DFMCoreException e)
			{
				var ignored = new [] {
					DfMError.NotSignedLastContract,
					DfMError.TFANotVerified,
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
