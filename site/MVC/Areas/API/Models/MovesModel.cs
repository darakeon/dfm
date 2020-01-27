using System;
using DFM.Entities.Enums;

namespace DFM.MVC.Areas.Api.Models
{
	public class MovesModel : BaseApiModel
	{
		public void Delete(Int32 id)
		{
			money.DeleteMove(id);
		}

		public void Check(Int32 id, PrimalMoveNature nature)
		{
			money.CheckMove(id, nature);
		}

		public void Uncheck(Int32 id, PrimalMoveNature nature)
		{
			money.UncheckMove(id, nature);
		}
	}
}
