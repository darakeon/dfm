using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DK.MVC.Forms;
using DFM.BusinessLogic.Exceptions;
using DFM.Generic;
using DFM.Multilanguage;
using DFM.MVC.Helpers.Global;

namespace DFM.MVC.Models
{
	public class UsersConfigModel : BaseModel
	{
		public UsersConfigModel()
		{
			var languageDictionary = 
				PlainText.AcceptedLanguage()
					.ToDictionary(l => l, l => MultiLanguage.Dictionary["Language" + l]);

			LanguageList = SelectListExtension.CreateSelect(languageDictionary);
			TimeZoneList = SelectListExtension.CreateSelect(DateTimeGMT.TimeZoneList());

			UseCategories = Current.User.Config.UseCategories;
			SendMoveEmail = Current.User.Config.SendMoveEmail;
			Language = Current.User.Config.Language;
			TimeZone = Current.User.Config.TimeZone;
		}



		public Boolean UseCategories { get; set; }
		public Boolean SendMoveEmail { get; set; }

		public String Language { get; set; }
		public String TimeZone { get; set; }
		
		public SelectList TimeZoneList { get; set; }
		public SelectList LanguageList { get; set; }



		internal IList<String> Save()
		{
			var errors = new List<String>();

			try
			{
				Admin.UpdateConfig(Language, TimeZone, SendMoveEmail, UseCategories);

				ErrorAlert.Add("ConfigChanged");
			}
			catch (DFMCoreException e)
			{
				errors.Add(MultiLanguage.Dictionary[e]);
			}

			return errors;
		}


	}
}