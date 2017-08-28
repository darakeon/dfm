using System;

namespace DFM.MVC.Areas.API.Models
{
	public class MovesDeleteModel : BaseApiModel
	{
		public static void Delete(Int32 id)
		{
			Money.DeleteMove(id);
		}

		public static void Check(Int32 id)
		{
			Money.ToggleMoveCheck(id, true);
		}

		public static void Uncheck(Int32 id)
		{
			Money.ToggleMoveCheck(id, false);
		}
	}
}