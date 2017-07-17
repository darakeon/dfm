using DFM.Core.Entities;
using DFM.MVC.Models;

namespace DFM.MVC.Areas.Accounts.Models
{
    public class ReportSeeYearModel : BaseLoggedModel
    {
        public Year Year { get; set; }
        public Account Account { get; set; }
    }
}