namespace DFM.Entities.Enums
{
	public enum ScheduleStatus
	{
		Ok = 0,
		CategoriesDisabled = 1,
		CategoryInvalid = 2,
		UserInactive = 3,
		UserRobot = 4,
		UserMarkedDelete = 5,
		UserNoSignContract = 6,
		UserRequestedWipe = 7,
		MoveOutOfLimit = 8,
		CategoryDisabled = 9,
		AccountClosed = 10,
		CurrencyChange = 11,
		Error = 99,
	}
}
