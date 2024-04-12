using DFM.BusinessLogic.Exceptions;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace DFM.MVC.Models;

public class UsersSendWipedDataModel : BaseSiteModel
{
	[Required(ErrorMessage = "*")]
	public String Email { get; set; }

	[Required(ErrorMessage = "*")]
	public String Password { get; set; }

	internal List<String> SendFile()
	{
		var errors = new List<String>();

		try
		{
			outside.SendWipedUserCSV(Email, Password);
			
			return null;
		}
		catch (CoreError e)
		{
			errors.Add(translator[e]);
		}

		return errors;
	}
}