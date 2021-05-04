using System;
using System.Collections.Generic;
using DFM.BusinessLogic.Exceptions;
using DFM.Email;
using DFM.Entities;
using DFM.Entities.Enums;
using Keon.Util.Extensions;
using Error = DFM.BusinessLogic.Exceptions.Error;

namespace DFM.BusinessLogic.Repositories
{
	internal class ControlRepository : Repo<Control>
	{
		private readonly Current.GetUrl getUrl;

		public ControlRepository(Current.GetUrl getUrl)
		{
			this.getUrl = getUrl;
		}

		internal void Activate(User user)
		{
			var control = user.Control;

			control.Active = true;
			control.WrongLogin = 0;

			SaveOrUpdate(control);
		}

		public void Deactivate(User user)
		{
			var control = user.Control;
			control.Active = false;
			SaveOrUpdate(control);
		}

		public void WarnRemoval(User user, DateTime dateTime, RemovalReason removalReason)
		{
			var dic = new Dictionary<String, String>
			{
				{ "Url", getUrl() },
				{ "Date", dateTime.ToShortDateString() },
			};

			var format = Format.UserRemoval(user, removalReason);

			var fileContent = format.Layout.Format(dic);

			var sender = new Sender()
				.To(user.Email)
				.Subject(format.Subject)
				.Body(fileContent);

			try
			{
				sender.Send();
			}
			catch (MailError)
			{
				throw Error.FailOnEmailSend.Throw();
			}

			var control = user.Control;
			control.RemovalWarningSent++;
			SaveOrUpdate(control);
		}

		public void SaveAccess(Control control)
		{
			control.LastAccess = DateTime.UtcNow;
			ResetWarnCounter(control);
		}

		public void ResetWarnCounter(Control control)
		{
			control.RemovalWarningSent = 0;
			SaveOrUpdate(control);
		}

		public void AnticipateRobotCheck(Control control)
		{
			control.RobotCheck = DateTime.UtcNow;
			SaveOrUpdate(control);
		}
	}
}
