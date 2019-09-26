using System;

namespace DFM.MVC.Areas.API.Models
{
	public class MovesModel : BaseApiModel
	{
		static MovesModel()
		{
			model = new MovesModel();
		}

		private static readonly MovesModel model;


		public static void Delete(Int32 id)
		{
			model.money.DeleteMove(id);
		}

		public static void Check(Int32 id)
		{
			model.money.CheckMove(id);
		}

		public static void Uncheck(Int32 id)
		{
			model.money.UncheckMove(id);
		}
	}
}