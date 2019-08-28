using DFM.BusinessLogic.Exceptions;

namespace DFM.MVC.Models
{
	public class OpsModel : BaseSiteModel
	{
		public OpsModel()
		{
		}

		public OpsModel(DfMError error) : this()
		{
			Error = error;
		}

		public DfMError Error { get; set; }


	}
}