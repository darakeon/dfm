using System.Web.Mvc.Ajax;

namespace DFM.MVC.Helpers
{
    public class AjaxOptionsDFM : AjaxOptions
    {
        public AjaxOptionsDFM()
        {
            OnBegin = "BeginAjaxPost";
            OnComplete = "EndAjaxPost";
            OnFailure = "AjaxFail";
            OnSuccess = "TellResultAndReload";
            HttpMethod = "Post";
        }
    }
}