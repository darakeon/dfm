using System;
using DFM.BusinessLogic.Helpers;
using DFM.MVC.Models;

namespace DFM.MVC.Areas.API.Models
{
	public class BaseApiModel : BaseModel
	{
		public String Language => Current.Language;
		public String Theme => UserTheme.Simplify().ToString();
	}
}
