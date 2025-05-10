using DFM.BusinessLogic.Exceptions;

namespace DFM.MVC.Models
{
	public class ReportsModel : BaseErrorModel
	{
		private ReportsModel() {}
		private static readonly ReportsModel model = new();

		public static void DismissTip()
		{
			model.dismissTip();
		}

		private void dismissTip()
		{
			try
			{
				clip.DismissTip();
			}
			catch (CoreError e)
			{
				errorAlert.Add(e.Type);
			}
		}
	}
}
