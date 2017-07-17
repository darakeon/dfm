using DFM.Core.Entities;
using DFM.MVC.Models;

namespace DFM.MVC.Areas.Accounts.Models
{
    public class CategoryCreateModel : BaseLoggedModel
    {
        public Category Category { get; set; }
    }
}