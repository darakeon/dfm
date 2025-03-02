using System;

namespace DFM.BusinessLogic.Response;

public class TFACheck : ITFAForm
{
	public String TFACode { get; set; }
	public String Password { get; set; }
}
