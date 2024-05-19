using DFM.BusinessLogic.Exceptions;
using DFM.Entities;
using DFM.Entities.Bases;

namespace DFM.BusinessLogic.Validators;

public class MoveValidator() : GenericMoveValidator<Move>(
	MaxLen.MoveDescription,
	Error.TooLargeMoveDescription
);
