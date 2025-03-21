using System;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Repositories;
using DFM.BusinessLogic.Response;
using DFM.BusinessLogic.Validators;
using DFM.Entities;
using DFM.Entities.Enums;

namespace DFM.BusinessLogic.Services
{
	public class OutsideService : Service
	{
		internal OutsideService(ServiceAccess serviceAccess, Repos repos, Valids valids)
			: base(serviceAccess, repos, valids) { }

		public void TestSecurityToken(String token, SecurityAction securityAction)
		{
			var security = repos.Security.ValidateAndGet(token, securityAction);

			if (security.User != null)
			{
				valids.User.CheckUserDeletion(security.User);
			}
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

				valids.User.CheckUserDeletion(user);

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

				valids.User.CheckUserDeletion(security.User);

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

				valids.User.CheckUserDeletion(user);

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

				valids.User.CheckUserDeletion(user);

				user.Password = reset.Password;

				repos.User.ChangePassword(user);

				repos.Security.Disable(reset.Token);

				repos.Ticket.List(user)
					.ToList().ForEach(repos.Ticket.Disable);
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

				parent.Law.CheckContractAccepted(user);

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
				w => !String.IsNullOrEmpty(w.CSVAddress)
			).ToList();

			if (!wipes.Any())
				throw Error.WipeNoMoves.Throw();

			foreach (var wipe in wipes)
			{
				var security = repos.Security.Create(wipe);
				repos.Wipe.SendCSV(email, security);
			}
		}

		public void WipeCsv(String token)
		{
			var security = repos.Security.ValidateAndGet(
				token, SecurityAction.DeleteCsvData
			);

			var csv = security.Wipe.CSVAddress;

			inTransaction("WipeCsv", () =>
			{
				repos.Wipe.DeleteFile(csv);
				repos.Security.Disable(token);
			});
		}
	}
}
