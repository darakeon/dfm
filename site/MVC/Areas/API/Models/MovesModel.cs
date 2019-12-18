using System;
using DFM.Entities.Enums;

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

		public static void Check(Int32 id, PrimalMoveNature nature)
		{
			model.money.CheckMove(id, nature);
		}

		public static void Uncheck(Int32 id, PrimalMoveNature nature)
		{
			model.money.UncheckMove(id, nature);
		}
	}
}
