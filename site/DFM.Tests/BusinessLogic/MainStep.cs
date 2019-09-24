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
using Error = DFM.BusinessLogic.Exceptions.Error;

namespace DFM.Tests.BusinessLogic
{
	[Binding]
	public class MainStep : BaseStep
	{
		[Given(@"I have a complete user logged in")]
		public void GivenIHaveAnActiveUser()
		{
			createLogoffLogin(userEmail, userPassword);
		}

		[Given(@"there is a bad person logged in")]
		public void GivenIHaveABadPersonLoggedIn()
		{
			createLogoffLogin(badPersonUser, userPassword);
		}

		private void createLogoffLogin(String email, String password)
		{
			resetTicket();
			createUserIfNotExists(email, password, true);
			current.Set(email, password, false);
			service.Safe.AcceptContract();
		}

		[Given(@"the right user login again")]
		public void GivenTheRightUserLoginAgain()
		{
			createLogoffLogin(userEmail, userPassword);
		}

		[Given(@"the user have accepted the contract")]
		public void GivenTheUserHaveAcceptedTheContract()
		{
			service.Safe.AcceptContract();
		}

		[Given(@"I have an account")]
		public void GivenIHaveAnAccount()
		{
			accountInfo = AccountInfo.Convert(
				getOrCreateAccount(mainAccountUrl)
			);
		}

		[Given(@"I have a category")]
		public void GivenIHaveACategory()
		{
			categoryInfo = CategoryInfo.Convert(
				getOrCreateCategory(mainCategoryName)
			);
		}

		[Given(@"I pass a valid account url")]
		public void GivenIPassValidAccountName()
		{
			accountUrl = accountInfo.Url;
		}


		[Then(@"I will receive this core error: ([A-Za-z]+)")]
		public void ThenIWillReceiveThisError(Error expectedError)
		{
			Assert.IsNotNull(expectedError);
			Assert.AreEqual(expectedError, error.Type);
		}

		[Then(@"I will receive no core error")]
		public void ThenIWillReceiveNoCoreError()
		{
			Assert.IsNull(error);
		}

		[BeforeTestRun]
		public static void Start()
		{
			setLogName();
			log("BeforeTestRun");

			DBHelper.Cleanup();

			SessionFactoryManager.Initialize<UserMap, User>();
			SessionManager.Init(getTicketKey);

			service = new ServiceAccess(getTicket, getPath);

			PlainText.Initialize(runPath);
		}

		[AfterTestRun]
		public static void End()
		{
			log("End");
			SessionFactoryManager.End();
		}

		// ReSharper disable once UnusedMember.Global
		[BeforeScenario]
		public static void RegisterRun()
		{
			log("Before scenario");
		}

		// ReSharper disable once UnusedMember.Global
		[AfterScenarioBlock]
		public static void CloseSession()
		{
			log("After scenario block");
			SessionManager.Close();
		}

		// ReSharper disable once UnusedMember.Global
		[AfterScenario]
		public void CleanSchedulesAndLogoff()
		{
			log("After scenario");
			if (!current.IsAuthenticated)
				return;

			try
			{
				var pendentSchedules = service.Robot.GetScheduleList();

				foreach (var pendentSchedule in pendentSchedules)
				{
					service.Robot.DisableSchedule(pendentSchedule.ID);
				}
			}
			catch (CoreError e)
			{
				var ignored = new [] {
					Error.NotSignedLastContract,
					Error.TFANotVerified,
				};

				if (!ignored.Contains(e.Type))
				{
					throw;
				}
			}

			current.Clear();
		}

		// ReSharper disable once UnusedMember.Global
		[StepArgumentTransformation(@"( not)?")]
		[StepArgumentTransformation(@"(not )?")]
		public bool NotToBoolTransform(string not)
		{
			return not.Trim() != "not";
		}
	}
}
