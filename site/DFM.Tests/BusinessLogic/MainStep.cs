using System;
using System.Linq;
using DK.NHibernate.Base;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace DFM.Tests.BusinessLogic
{
	[Binding]
	public class MainStep : BaseStep
	{
		[Given(@"I have an active user")]
		public void GivenIHaveAnActiveUser()
		{
			CreateUserIfNotExists(USER_EMAIL, UserPassword, true);

			Current.Reset(USER_EMAIL, UserPassword);
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
		public void ThenIWillReceiveThisError(String error)
		{
			Assert.IsNotNull(Error);
			Assert.AreEqual(error, Error.Type.ToString());
		}

		[Then(@"I will receive no core error")]
		public void ThenIWillReceiveNoCoreError()
		{
			Assert.IsNull(Error);
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

			var pendentSchedules = Current.User.ScheduleList.Where(s => s.Active);

			foreach (var pendentSchedule in pendentSchedules)
			{
				SA.Robot.DisableSchedule(pendentSchedule.ID);
			}

			Current.Clean();
		}


		[StepArgumentTransformation(@"( not)?")]
		[StepArgumentTransformation(@"(not )?")]
		public bool NotToBoolTransform(string not)
		{
			return not.Trim() != "not";
		}

	}
}
