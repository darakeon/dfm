using System;
using System.ComponentModel.DataAnnotations;
using DFM.BusinessLogic.Response;

namespace DFM.API.Models
{
    public class UsersSignUpModel : BaseApiModel
    {
        [Required]
        public String Email { get; set; }

        [Required]
        public String Password { get; set; }

		[Required]
        public Boolean AcceptedContract { get; set; }

        [Required]
        public String Language { get; set; }

        [Required]
        public String Timezone { get; set; }

		internal void SignUp()
        {
	        auth.SaveUser(
		        new SignUpInfo
		        {
			        Email = Email,
			        Password = Password,
			        AcceptedContract = AcceptedContract,
			        Language = Language,
			        TimeZone = Timezone,
			        RetypePassword = Password,
		        }
		    );
        }
	}
}
