using System;
using DFM.MVC.Models;

namespace DFM.MVC.Areas.API.Models
{
    internal class UserLogOnJsonModel : UserLogOnModel
    {
        public String MachineId { get; set; }

        protected override void SetLogOn()
        {
            Current.Set(Email, Password, MachineId);
        }

    }
}