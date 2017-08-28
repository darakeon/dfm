using System;
using DFM.Entities.Enums;
using DFM.Multilanguage;

namespace DFM.Email
{
	public class Format
	{
		public String Subject { get; set; }
		public String Layout { get; set; }


		public Format(String language, MoveNature moveNature)
			: this(language, $"MoveNotification{moveNature}") { }

		public Format(String language, SecurityAction securityAction)
			: this(language, securityAction.ToString()) { }

		private Format(String language, String key)
		{
			Layout = PlainText.EmailLayout[language, key];
			Subject = PlainText.Dictionary["Email", language, key];
		}


	}
}
