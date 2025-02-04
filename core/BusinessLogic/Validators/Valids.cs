namespace DFM.BusinessLogic.Validators;

internal class Valids
{
	internal MoveValidator Move;
	internal ScheduleValidator Schedule;
	internal DetailValidator Detail;
	internal UserValidator User;

	internal Valids()
	{
		Schedule = new ScheduleValidator();
		Move = new MoveValidator();
		Detail = new DetailValidator();
		User = new UserValidator();
	}

}
