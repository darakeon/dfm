using System;
using DFM.Entities.Enums;

namespace DFM.API.Models
{
	public class MovesModel : BaseApiModel
	{
		public void Delete(Guid guid)
		{
			money.DeleteMove(guid);
		}

		public void Check(Guid guid, PrimalMoveNature nature)
		{
			money.CheckMove(guid, nature);
		}

		public void Uncheck(Guid guid, PrimalMoveNature nature)
		{
			money.UncheckMove(guid, nature);
		}
	}
}
