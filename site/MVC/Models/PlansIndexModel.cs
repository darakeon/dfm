using DFM.Entities;

namespace DFM.MVC.Models;

public class PlansIndexModel : BaseSiteModel
{
	public Plan Plan { get; set; } = law.GetPlan();
}
