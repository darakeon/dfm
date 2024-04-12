using System;

namespace DFM.API.Models
{
	public class MovesDeleteModel : BaseApiModel
	{
		public void Delete(Guid guid)
		{
			money.DeleteMove(guid);
		}
	}
}
