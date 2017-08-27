using System;
using DFM.MVC.Areas.API.Models;

namespace DFM.MVC.Areas.API.Controllers
{
	public class MovesDeleteModel : BaseApiModel
	{
		public static void Delete(Int32 id)
		{
			Money.DeleteMove(id);
		}
	}
}