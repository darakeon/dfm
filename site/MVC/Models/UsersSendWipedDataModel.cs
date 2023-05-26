using DFM.BusinessLogic.Exceptions;
using System;
using System.ComponentModel.DataAnnotations;
using DFM.Exchange;
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
			var s3 = new S3();
			outside.SendWipedUserCSV(Email, Password, s3.Download);
			
			return null;
		}
		catch (CoreError e)
		{
			errors.Add(translator[e]);
		}

		return errors;
	}
}