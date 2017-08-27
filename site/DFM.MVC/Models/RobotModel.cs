using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DFM.BusinessLogic.Exceptions;
using DFM.MVC.Helpers.Global;

namespace DFM.MVC.Models
{
    public class RobotModel : BaseModel
    {
        internal void Disable(Int32 id)
        {
            try
            {
                Robot.DisableSchedule(id);
            }
            catch (DFMCoreException e)
            {
                ErrorAlert.Add(e.Type);
            }
        }

    }
}