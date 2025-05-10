using DFM.BaseWeb.Models;
using DFM.MVC.Errors;

namespace DFM.MVC.Models;

public class BaseErrorModel : BaseModel
{
	protected ErrorAlert errorAlert => context.GetErrorAlert();
}
