using DFM.Entities;

namespace DFM.MVC.Models;

public class PlansIndexModel : BaseSiteModel
{
	public Plan Plan => law.GetPlan();
}
