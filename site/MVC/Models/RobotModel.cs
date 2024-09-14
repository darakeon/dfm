using System;
using DFM.BusinessLogic.Exceptions;

namespace DFM.MVC.Models
{
	public class RobotModel : BaseSiteModel
	{
		internal void Disable(Guid guid)
		{
			try
			{
				attendant.DisableSchedule(guid);
			}
			catch (CoreError e)
			{
				errorAlert.Add(e.Type);
			}
		}
	}
}
