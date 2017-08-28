using System;
using DFM.BusinessLogic.Exceptions;
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

			Current.Clear();
			Current.Set(USER_EMAIL, UserPassword, false);

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
				if (e.Type != ExceptionPossibilities.NotSignedLastContract)
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
