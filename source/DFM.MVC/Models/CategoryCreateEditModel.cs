using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using DFM.Entities;
using DFM.Generic;
using DFM.MVC.Helpers;

namespace DFM.MVC.Models
{
    public class CategoryCreateEditModel : BaseLoggedModel
    {
        public CategoryCreateEditModel()
        {
            Category = new Category();
        }

        public CategoryCreateEditModel(OperationType type) : this()
        {
            Type = type;
        }

        

        public OperationType Type { get; set; }

        public Category Category { get; set; }

        private String name;

        [Required(ErrorMessage = "*")]
        public String Name
        {
            get
            {
                switch (Type)
                {
                    case OperationType.Creation:
                        return Category.Name;
                    case OperationType.Edit:
                        return name ?? Category.Name;
                    default:
                        throw new NotImplementedException();
                }
            }
            set
            {
                switch (Type)
                {
                    case OperationType.Creation:
                        Category.Name = value;
                        break;
                    case OperationType.Edit:
                        name = value;
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }



        public Boolean IsMoveCRUD { get; private set; }


        internal void DefineAction(HttpRequestBase request)
        {
            IsMoveCRUD = request.Path.Contains(RouteNames.Accounts);
        }


    }
}