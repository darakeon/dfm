using System;
using DFM.BusinessLogic.Exceptions;
using DFM.MVC.Helpers.Global;

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
			catch (DFMCoreException e)
			{
				ErrorAlert.Add(e.Type);
			}
		}

	}
}