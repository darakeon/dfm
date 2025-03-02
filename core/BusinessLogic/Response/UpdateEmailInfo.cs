using System;

namespace DFM.BusinessLogic.Response;

public class UpdateEmailInfo: ITFAForm
{
	public String Email { get; set; }
	public String Password { get; set; }
	public String TFACode { get; set; }
}
