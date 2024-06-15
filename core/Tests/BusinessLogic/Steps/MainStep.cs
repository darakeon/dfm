using System;
using System.Linq;
using DFM.BusinessLogic.Repositories.Mappings;
using DFM.BusinessLogic.Response;
using DFM.BusinessLogic.Tests.Helpers;
using DFM.Entities;
using DFM.Generic;
using DFM.Generic.Datetime;
using DFM.Language;
using Keon.NHibernate.Schema;
using Keon.NHibernate.Sessions;
using NUnit.Framework;
using TechTalk.SpecFlow;
using Error = DFM.BusinessLogic.Exceptions.Error;

namespace DFM.BusinessLogic.Tests.Steps
{
	[Binding]
	public class MainStep : BaseStep
	{
		public MainStep(ScenarioContext context)
			: base(context) { }

		[Given(@"test user login")]
		public void GivenIHaveACompleteUserLoggedIn()
		{
			createLogoffLogin(userEmail);
		}

		[Given(@"there is a bad person logged in")]
		public void GivenIHaveABadPersonLoggedIn()
		{
			createLogoffLogin(badPersonEmail);
		}

		[Given(@"there is another person logged in")]
		public void GivenIHaveAnotherPersonLoggedIn()
		{
			createLogoffLogin(anotherPersonEmail);
		}

		[Given(@"the user have accepted the contract")]
		public void GivenTheUserHaveAcceptedTheContract()
		{
			service.Law.AcceptContract();
		}

		[Given(@"I have an account")]
		public void GivenIHaveAnAccount()
		{
			accountInfo = AccountInfo.Convert(
				getOrCreateAccount($"{mainAccountUrl}_{scenarioCode}")
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
		public void GivenIPassValidAccountUrl()
		{
			accountUrl = accountInfo.Name.IntoUrl();
		}

		[Given(@"I set test start date here")]
		public void GivenISetTestStartDateHere()
		{
			testStart = DateTime.UtcNow;
		}

		[Given(@"the user(.*) is marked for deletion")]
		public void GivenTheUserIsMarkedForDeletion(String passedEmail)
		{
			passedEmail = String.IsNullOrEmpty(passedEmail)
				? null
				: passedEmail.Trim().ForScenario(scenarioCode);

			var user = repos.User.GetByEmail(
				passedEmail ?? email ?? userEmail
			);

			db.Execute(() => repos.Control.MarkDeletion(user));
		}

		[Given(@"the user(.*) asked data wipe")]
		public void GivenTheUserAskedDataWipe(String passedEmail)
		{
			passedEmail = String.IsNullOrEmpty(passedEmail)
				? null
				: passedEmail.Trim().ForScenario(scenarioCode);

			var user = repos.User.GetByEmail(
				passedEmail ?? email ?? userEmail
			);

			db.Execute(() => repos.Control.RequestWipe(user));
		}

		[When(@"test user login")]
		public void WhenTestUserLogin()
		{
			createLogoffLogin(userEmail);
		}

		[When(@"robot user login")]
		public void WhenRobotUserLogin()
		{
			createLogoffLoginRobot();
		}

		[Then(@"I will receive a core error")]
		public void ThenIWillReceiveACoreError()
		{
			Assert.That(error, Is.Not.Null);
		}

		[Then(@"I will receive this core error: ([A-Za-z]+)")]
		public void ThenIWillReceiveThisError(Error expectedError)
		{
			Assert.That(error, Is.Not.Null);
			Assert.That(error.Type, Is.EqualTo(expectedError));
		}

		[Then(@"I will receive these core errors")]
		public void ThenIWillReceiveTheseCoreErrors(Table table)
		{
			Assert.That(error, Is.Not.Null);

			var expectedErrors = table.Rows
				.Select(r => r["Error"])
				.Select(EnumX.Parse<Error>)
				.ToList();

			Assert.That(error.Types.Count, Is.EqualTo(expectedErrors.Count));

			for (var e = 0; e < expectedErrors.Count; e++)
			{
				var actualError = error.Types.Single(
					type => type.Metadata is Int16 metadata && metadata == e+1
				);

				var expectedError = expectedErrors[e];

				Assert.That(actualError.Error, Is.EqualTo(expectedError));
			}
		}

		[Then(@"I will receive no core error")]
		public void ThenIWillReceiveNoCoreError()
		{
			Assert.That(error, Is.Null);
		}

		[BeforeTestRun]
		public static void Start()
		{
			Cfg.Init();

			setLogName();

			setRepositories();

			log("General", "BeforeTestRun");

			TZ.Init(false);

			ControlMap.IsTest = true;

			SessionFactoryManager.Initialize<UserMap, User>(Cfg.DB);
			SessionManager.Init(getTicketKey);

			service = new ServiceAccess(
				getTicket, getSite, fileService, queueService
			);

			createContract();

			PlainText.Initialize(runPath);
		}

		private static void createContract()
		{
			var contract = new Contract
			{
				BeginDate = DateTime.UtcNow,
				Version = "0.0.0.0",
			};

			repos.Contract.SaveOrUpdate(contract);
		}

		[AfterTestRun]
		public static void End()
		{
			log("General", "End");
			SessionFactoryManager.End();
		}

		// ReSharper disable once UnusedMember.Global
		[BeforeScenario]
		public void RegisterRun()
		{
			log("Before scenario");
			testStart = DateTime.UtcNow;
		}

		// ReSharper disable once UnusedMember.Global
		[AfterScenarioBlock]
		public void CloseSession()
		{
			log("After scenario block");
			SessionManager.Close();
		}

		// ReSharper disable once UnusedMember.Global
		[AfterScenario]
		public void CleanSchedulesAndLogoff()
		{
			log("After scenario");

			var pendentSchedules = repos.Schedule.Where(s => s.Active);
			foreach (var schedule in pendentSchedules)
			{
				repos.Schedule.Disable(schedule.Guid, schedule.User);
			}

			if (current.IsAuthenticated)
				current.Clear();

			var loggedTickets = repos.Ticket.Where(s => s.Active);
			foreach (var ticket in loggedTickets)
			{
				ticket.LastAccess = DateTime.UtcNow;
				db.Execute(() => repos.Ticket.Disable(ticket));
			}

			var user = repos.User.GetByEmail(userEmail);
			if (user != null)
			{
				var control = user.Control;
				control.LastAccess = DateTime.UtcNow;
				db.Execute(() => repos.Control.SaveOrUpdate(control));
			}
		}

		// ReSharper disable once UnusedMember.Global
		[StepArgumentTransformation(@"( not?|not? )?")]
		public Boolean NotToBoolTransform(String not)
		{
			return not.Trim() != "not" && not.Trim() != "no";
		}

		// ReSharper disable once UnusedMember.Global
		[StepArgumentTransformation(@"(Yes|No)")]
		public Boolean YesNoToBoolTransform(String not)
		{
			return not.Trim() == "Yes";
		}

		// ReSharper disable once UnusedMember.Global
		[StepArgumentTransformation(@"(dis|en)?")]
		public Boolean DisEnToBoolTransform(String prefix)
		{
			return prefix == "en";
		}

		// ReSharper disable once UnusedMember.Global
		[StepArgumentTransformation(@"(never|once|twice)?")]
		public Int32 OnceTwiceTransform(String times)
		{
			return times == "once" ? 1 :
				times == "twice" ? 2 :
				0;
		}
	}
}
