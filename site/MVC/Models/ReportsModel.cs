using DFM.BusinessLogic.Exceptions;

namespace DFM.MVC.Models
{
	public class ReportsModel : BaseModel
	{
		private ReportsModel() {}
		private static readonly ReportsModel model = new();

		public static void DismissTip()
		{
			try
			{
				model.report.DismissTip();
			}
			catch (CoreError e)
			{
				model.errorAlert.Add(e.Type);
			}
		}
	}
}
