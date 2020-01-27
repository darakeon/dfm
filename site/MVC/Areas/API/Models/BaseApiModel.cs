using DFM.MVC.Models;

namespace DFM.MVC.Areas.Api.Models
{
	public class BaseApiModel : BaseModel
	{
		public BaseApiModel()
		{
			Environment = new Environment(theme, language);
		}

		internal Environment Environment { get; }
	}
}
