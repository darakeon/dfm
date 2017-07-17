using System.ComponentModel.DataAnnotations;

namespace DFM.MVC.Models
{
    public class UserLogOnModel
    {
        [Required(ErrorMessage = "> Mandatory Field")]
        public string Login { get; set; }

        [Required(ErrorMessage = "> Mandatory Field")]
        public string Password { get; set; }
    }
}