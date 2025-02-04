using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DFM.BusinessLogic.Exceptions;

namespace DFM.MVC.Models;

public class UsersAskRemoveTFAModel : BaseSiteModel
{
	[Required(ErrorMessage = "*")]
	public String Password { get; set; }

	internal IList<String> AskRemoveTFA()
	{
		var errors = new List<String>();

		try
		{
			auth.AskRemoveTFA(Password);
		}
		catch (CoreError e)
		{
			errors.Add(translator[e]);
		}

		return errors;
	}
}
