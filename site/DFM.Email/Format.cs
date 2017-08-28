using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using DFM.Entities.Enums;
using DFM.Generic;
using DFM.Multilanguage;
using DFM.Multilanguage.MultiLanguage.EmailLayouts;
using DK.Generic.Extensions;

namespace DFM.Email
{
	public class Format
	{
		public String Subject { get; set; }
		public String Layout { get; set; }

		private enum Type
		{
			SecurityAction,
			MoveNotification,
		}


		public static Format MoveNotification(String language)
		{
			return new Format(language, Type.MoveNotification, Type.MoveNotification);
        }

		public static Format SecurityAction(String language, SecurityAction securityAction)
		{
			return new Format(language, Type.SecurityAction, securityAction);
		}

		private Format(String language, object keyType, object layoutType)
		{
			var key = keyType.ToString();
			var layoutName = layoutType.ToString();

			var resourceManager = getResourceForLayout(layoutName);
			var resourceSet = resourceManager.ToDictionary(new CultureInfo(language));

			Subject = resourceSet["Title"];
			Layout = PlainText.EmailLayout[key].Format(resourceSet);
		}

		private static IEnumerable<ResourceManager> getResourceForLayout(String layoutName)
		{
			var mainType = typeof(Master);
			var assembly = mainType.Assembly;
			var typeName = mainType.FullName.Replace(mainType.Name, layoutName);
			var resourceType = assembly.GetType(typeName);
			var property = resourceType.GetProperties().Single(p => p.PropertyType == typeof(ResourceManager));

			return new[] {
				(ResourceManager) property.GetValue(null),
				 Master.ResourceManager
			};
		}


	}
}
