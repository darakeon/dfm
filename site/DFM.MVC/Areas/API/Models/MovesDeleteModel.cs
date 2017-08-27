using System;

namespace DFM.MVC.Areas.API.Models
{
	public class MovesDeleteModel : BaseApiModel
	{
		public static void Delete(Int32 id)
		{
			Money.DeleteMoveByFakeId(id);
		}
	}
}