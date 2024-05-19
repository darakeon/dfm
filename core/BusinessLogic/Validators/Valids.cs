using DFM.BusinessLogic.Exceptions;
using DFM.Entities;
using DFM.Entities.Bases;

namespace DFM.BusinessLogic.Validators;

internal class Valids
{
	internal MoveValidator<Move> Move;

	internal Valids()
	{
		Move = new MoveValidator<Move>(
			MaxLen.MoveDescription,
			Error.MoveDescriptionRequired
		);
	}

}
