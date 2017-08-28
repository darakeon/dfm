using DFM.BusinessLogic.Exceptions;

namespace DFM.MVC.Models
{
	public class OpsModel : BaseLoggedModel
	{
		public OpsModel()
		{
		}

		public OpsModel(ExceptionPossibilities error) : this()
		{
			Error = error;
		}

		public ExceptionPossibilities Error { get; set; }

		
	}
}