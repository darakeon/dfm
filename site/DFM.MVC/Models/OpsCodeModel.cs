using DFM.Email;

namespace DFM.MVC.Models
{
	public class OpsCodeModel : BaseLoggedModel
	{
		public Error.Status EmailSent { get; set; }
	}
}