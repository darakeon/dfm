using System;
using DFM.BusinessLogic.Exceptions;

namespace DFM.MVC.Models
{
	public class RobotModel : BaseSiteModel
	{
		internal void Disable(Int32 id)
		{
			try
			{
				robot.DisableSchedule(id);
			}
			catch (CoreError e)
			{
				errorAlert.Add(e.Type);
			}
		}

	}
}
