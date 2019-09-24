namespace DFM.MVC.Helpers.Views
{
	public class AjaxOptions : System.Web.Mvc.Ajax.AjaxOptions
	{
		public AjaxOptions()
		{
			OnBegin = "BeginAjaxPost";
			OnComplete = "EndAjaxPost";
			OnFailure = "AjaxFail";
			OnSuccess = "TellResultAndReload";
			HttpMethod = "Post";
		}
	}
}
