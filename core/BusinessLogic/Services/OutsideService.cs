using DFM.BusinessLogic.Repositories;
using System;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities.Enums;
using DFM.Entities;
using DFM.BusinessLogic.Response;

namespace DFM.BusinessLogic.Services
{
	public class OutsideService : Service
	{
		internal OutsideService(ServiceAccess serviceAccess, Repos repos)
			: base(serviceAccess, repos) { }

		public void TestSecurityToken(String token, SecurityAction securityAction)
		{
			var security = repos.Security.ValidateAndGet(token, securityAction);

			if (security.User.Control.ProcessingDeletion)
				throw Error.UserDeleted.Throw();

			if (security.User.Control.WipeRequest != null)
				throw Error.UserAskedWipe.Throw();
		}

		public void DisableToken(String token)
		{
			inTransaction("DisableToken",
				() => repos.Security.Disable(token)
			);
		}

		public void SendUserVerify(String email)
		{
			inTransaction("SendUserVerify", () =>
			{
				var user = repos.User.GetByEmail(email);

				if (user == null)
					throw Error.InvalidUser.Throw();

				if (user.Control.ProcessingDeletion)
					throw Error.UserDeleted.Throw();

				if (user.Control.WipeRequest != null)
					throw Error.UserAskedWipe.Throw();

				SendUserVerify(user);
			});
		}

		internal void SendUserVerify(User user)
		{
			repos.Security.CreateAndSendToken(
				user, SecurityAction.UserVerification
			);
		}

		public void ActivateUser(String token)
		{
			inTransaction("ActivateUser", () =>
			{
				var security = repos.Security.ValidateAndGet(
					token, SecurityAction.UserVerification
				);

				if (security.User.Control.ProcessingDeletion)
					throw Error.UserDeleted.Throw();

				if (security.User.Control.WipeRequest != null)
					throw Error.UserAskedWipe.Throw();

				repos.Control.Activate(security.User);

				repos.Security.Disable(token);
			});
		}

		public void SendPasswordReset(String email)
		{
			inTransaction("SendPasswordReset", () =>
			{
				var user = repos.User.GetByEmail(email);

				if (user == null)
					return;

				if (user.Control.ProcessingDeletion)
					throw Error.UserDeleted.Throw();

				if (user.Control.WipeRequest != null)
					throw Error.UserAskedWipe.Throw();

				repos.Security.CreateAndSendToken(
					user, SecurityAction.PasswordReset
				);
			});
		}

		public void ResetPassword(PasswordResetInfo reset)
		{
			reset.VerifyPassword();

			inTransaction("PasswordReset", () =>
			{
				var security = repos.Security.ValidateAndGet(
					reset.Token,
					SecurityAction.PasswordReset
				);

				var user = security.User;

				if (user.Control.ProcessingDeletion)
					throw Error.UserDeleted.Throw();

				if (user.Control.WipeRequest != null)
					throw Error.UserAskedWipe.Throw();

				user.Password = reset.Password;

				repos.User.ChangePassword(user);

				repos.Security.Disable(reset.Token);
			});
		}

		public void UnsubscribeMoveMail(String token)
		{
			inTransaction("UnsubscribeMoveMail", () =>
			{
				var security = repos.Security.ValidateAndGet(
					token, SecurityAction.UnsubscribeMoveMail
				);

				var user = security.User;
				if (!parent.Law.IsLastContractAccepted(user))
					throw Error.NotSignedLastContract.Throw();

				var settings = user.Settings;
				settings.SendMoveEmail = false;
				repos.Settings.Update(settings);

				repos.Security.Disable(token);
			});
		}

		public void SendWipedUserCSV(String email, String password)
		{
			var wipes = repos.Wipe.GetByUser(email, password);

			wipes = wipes.Where(
				w => w.Why != RemovalReason.PersonAsked
			).ToList();

			if (!wipes.Any())
				throw Error.WipeUserAsked.Throw();

			wipes = wipes.Where(
				w => !String.IsNullOrEmpty(w.S3)
			).ToList();

			if (!wipes.Any())
				throw Error.WipeNoMoves.Throw();

			foreach (var wipe in wipes)
			{
				var security = repos.Security.Create(wipe);
				repos.Wipe.SendCSV(email, security);
			}
		}
	}
}
