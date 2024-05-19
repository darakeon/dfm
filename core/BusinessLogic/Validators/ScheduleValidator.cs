using DFM.BusinessLogic.Exceptions;
using DFM.Entities;
using DFM.Entities.Bases;

namespace DFM.BusinessLogic.Validators;

public class ScheduleValidator() : GenericMoveValidator<Schedule>(
	MaxLen.ScheduleDescription,
	Error.TooLargeScheduleDescription
);
