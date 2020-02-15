﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Response;
using DFM.Generic;

namespace DFM.MVC.Models
{
	public class UsersSignUpModel : BaseSiteModel
	{
		public UsersSignUpModel()
		{
			Contract = safe.GetContract();
			Info = new SignUpInfo
			{
				EnableWizard = true
			};
		}

		[Required(ErrorMessage = "*")]
		public String Email
		{
			get => Info.Email;
			set => Info.Email = value;
		}

		[Required(ErrorMessage = "*")]
		public String Password
		{
			get => Info.Password;
			set => Info.Password = value;
		}

		[Required(ErrorMessage = "*")]
		public String RetypePassword
		{
			get => Info.RetypePassword;
			set => Info.RetypePassword = value;
		}

		[RegularExpression("True", ErrorMessage = "*")]
		public Boolean Accept
		{
			get => Info.AcceptedContract;
			set => Info.AcceptedContract = value;
		}

		public ContractInfo Contract { get; }

		public SignUpInfo Info { get; set; }

		public Int32 TimeZoneOffset { get; set; }

		internal IList<String> ValidateAndSendVerify()
		{
			var errors = new List<String>();

			try
			{
				Info.Language = Language;
				Info.TimeZone = TimeZoneOffset.GetTimeZone();
				safe.SaveUser(Info);
			}
			catch (CoreError e)
			{
				errors.Add(translator[e]);
			}

			return errors;
		}


	}
}
