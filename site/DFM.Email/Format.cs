using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using DFM.Entities.Enums;
using DFM.Generic;
using DFM.Multilanguage;
using DFM.Multilanguage.Emails;
using DFM.Multilanguage.MultiLanguage.EmailLayouts;
using DK.Generic.Extensions;

namespace DFM.Email
{
	public class Format
	{
		public String Subject { get; set; }
		public String Layout { get; set; }


		public static Format MoveNotification(String language, SimpleTheme theme)
		{
			return new Format(language, theme, EmailType.MoveNotification, EmailType.MoveNotification);
        }

		public static Format SecurityAction(String language, SimpleTheme theme, SecurityAction securityAction)
		{
			return new Format(language, theme, EmailType.SecurityAction, securityAction);
		}

		private Format(String language, SimpleTheme theme, EmailType type, object layoutType)
		{
			var layoutName = layoutType.ToString();

			var resourceManager = getResourceForLayout(layoutName);
			var resourceSet = resourceManager.ToDictionary(new CultureInfo(language));

			Subject = resourceSet["Title"];
			Layout = PlainText.EmailLayout[theme, type].Format(resourceSet);
		}

		private static IEnumerable<ResourceManager> getResourceForLayout(String layoutName)
		{
			var mainType = typeof(General);
			var assembly = mainType.Assembly;
			var typeName = mainType.FullName.Replace(mainType.Name, layoutName);
			var resourceType = assembly.GetType(typeName);
			var property = resourceType.GetProperties().Single(p => p.PropertyType == typeof(ResourceManager));

			return new[] {
				(ResourceManager) property.GetValue(null),
				 General.ResourceManager
			};
		}


	}
}
