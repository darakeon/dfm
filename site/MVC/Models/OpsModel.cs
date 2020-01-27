using DFM.BusinessLogic.Exceptions;

namespace DFM.MVC.Models
{
	public class OpsModel : BaseSiteModel
	{
		public OpsModel() { }

		public OpsModel(Error error) : this()
		{
			Error = error;
		}

		public Error Error { get; set; }
	}
}
