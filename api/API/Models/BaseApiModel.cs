using System.Runtime.CompilerServices;
using DFM.MVC.Models;

[assembly: InternalsVisibleTo("DFM.Api.Tests")]
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
