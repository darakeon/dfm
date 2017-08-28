using System;

namespace DFM.MVC.Areas.API.Models
{
	public class MovesModel : BaseApiModel
	{
		public static void Delete(Int32 id)
		{
			Money.DeleteMove(id);
		}

		public static void Check(Int32 id)
		{
			Money.CheckMove(id);
		}

		public static void Uncheck(Int32 id)
		{
			Money.UncheckMove(id);
		}
	}
}