using System;
using DFM.Entities.Enums;

namespace DFM.API.Models;

public class MovesToggleCheckModel : BaseApiModel
{
	public PrimalMoveNature Nature { get; set; }

	public void Check(Guid guid)
	{
		money.CheckMove(guid, Nature);
	}

	public void Uncheck(Guid guid)
	{
		money.UncheckMove(guid, Nature);
	}
}