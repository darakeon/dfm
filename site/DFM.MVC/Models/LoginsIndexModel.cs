using System.Collections.Generic;
using DFM.Entities;

namespace DFM.MVC.Models
{
    public class LoginsIndexModel : BaseLoggedModel
    {
        public LoginsIndexModel()
        {
            LoginsList = Safe.ListLogins();
        }

        public IList<Ticket> LoginsList { get; set; }
    }
}