using System;

namespace DFM.MVC.Areas.API.Models
{
    public class UserGetConfigModel : BaseApiModel
    {
        public UserGetConfigModel()
        {
            UseCategories = Current.User.Config.UseCategories;
        }

        public Boolean UseCategories { get; set; }

    }
}