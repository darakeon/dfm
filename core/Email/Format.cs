using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities;
using DFM.Entities.Bases;
using DFM.Language;
using DFM.Language.Emails;
using DFM.Entities.Enums;
using DFM.Generic;
using Keon.Util.Extensions;

namespace DFM.Email
{
	public class Format
	{
		public String Subject { get; set; }
		public String Layout { get; set; }

		public static Format MoveNotification(User user)
		{
			return new(user, EmailType.MoveNotification, EmailType.MoveNotification);
		}

		public static Format SecurityAction(User user, SecurityAction securityAction)
		{
			return new(user, EmailType.SecurityAction, securityAction);
		}

		public static Format UserRemoval(User user, RemovalReason removalReason)
		{
			return new(user, EmailType.RemovalReason, removalReason);
		}

		public static Format WipeNotice(User user, RemovalReason removalReason)
		{
			return new(user, EmailType.WipeNotice, removalReason);
		}

		public static Format WipeCSVRecover(Wipe wipe)
		{
			return new(
				wipe,
				null,
				EmailType.SecurityAction,
				Entities.Enums.SecurityAction.DeleteCsvData
			);
		}

		private Format(User user, EmailType type, Object subtype)
			: this(
				user.Settings,
				user.GenerateMisc(),
				type,
				subtype
			) { }

		private Format(
			ITalkable talkable, Misc misc,
			EmailType type, Object subtype
		)
		{
			var subtypeName = subtype.ToString();
			var replaces = getReplaces(type.ToString(), subtypeName, talkable.Language);

			if (misc != null)
			{
				replaces.Add("MiscColor", misc.Color);
				replaces.Add("MiscAntenna", misc.Antenna);
				replaces.Add("MiscEye", misc.Eye);
				replaces.Add("MiscArm", misc.Arm);
				replaces.Add("MiscLeg", misc.Leg);
				replaces.Add("MiscBackground", misc.Background);
				replaces.Add("MiscBorder", misc.Border);

				replaces.Add(
					"MiscImageReading",
					PlainText.GetMiscText(misc, talkable.Language)
				);

				replaces.Add(
					"DaysWipe", DayLimits.WIPE.ToString()
				);
			}

			Subject = replaces["Subject"];
			Layout = FormatEmail(talkable.Theme, type, subtypeName, replaces, misc);
		}

		private static Dictionary<String, String> getReplaces(
			String type, String subtype, String language
		)
		{
			var sections = new[] {type, subtype};
			var replaces = PlainText.Email[sections, language];
			return replaces.ToDictionary(p => p.Name, p => p.Text);
		}

		public static String FormatEmail<T>(Theme theme, EmailType type, String subtype, IDictionary<String, T> replaces, Misc misc)
		{
			return PlainText.Html[theme, type, subtype, misc]
				.Format(
					replaces.ToDictionary(
						p => p.Key,
						p => p.Value?
							.ToString()?
							.Replace("\n", "<br />")
					)
				);
		}
	}
}
