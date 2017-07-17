using System;
using System.Web;
using DFM.Core.Entities;
using DFM.MVC.Helpers;

namespace DFM.MVC.Models
{
    public class CategoryCreateEditModel : BaseLoggedModel
    {
        public Category Category { get; set; }

        public Boolean IsMoveCRUD { get; private set; }


        internal void DefineAction(HttpRequestBase request)
        {
            IsMoveCRUD = request.Path.Contains(RouteNames.Accounts);
        }

    }
}