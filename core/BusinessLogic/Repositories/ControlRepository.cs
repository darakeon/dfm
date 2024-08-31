using System;
using System.Collections.Generic;
using DFM.BusinessLogic.Exceptions;
using DFM.Email;
using DFM.Entities;
using DFM.Entities.Bases;
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
			var wipeDate = dateTime.ToUniversalTime()
				.AddDays(DayLimits.WIPE).Date;

			var now = DateTime.UtcNow.Date;
			var diff = wipeDate - now;
			var count = (Int32) diff.TotalDays;

			var dic = new Dictionary<String, String>
			{
				{ "Url", getUrl() },
				{ "Date", dateTime.ToShortDateString() },
				{ "Count", count.ToString() },
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
			catch (MailError e)
			{
				throw Error.FailOnEmailSend.Throw(e);
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

		public void MarkDeletion(User user)
		{
			user.Control.ProcessingDeletion = true;
			SaveOrUpdate(user.Control);
		}

		public void RequestWipe(User user)
		{
			user.Control.WipeRequest = DateTime.UtcNow;
			SaveOrUpdate(user.Control);
		}

		public void ReMisc(User user)
		{
			var oldDna = user.Control.MiscDna;
			while (oldDna == user.Control.MiscDna)
			{
				user.Control.MiscDna = Misc.RandomDNA();
			}
			SaveOrUpdate(user.Control);
		}
	}
}
