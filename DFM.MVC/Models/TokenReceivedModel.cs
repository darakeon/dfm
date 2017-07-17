using System;
using System.ComponentModel.DataAnnotations;

namespace DFM.MVC.Models
{
    public class TokenReceivedModel
    {
        [Required(ErrorMessage = "*")]
        public String Token { get; set; }
    }
}