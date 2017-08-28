using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities;
using DFM.Generic;
using DFM.MVC.Helpers.Global;

namespace DFM.MVC.Models
{
    public class CategoriesCreateEditModel : BaseLoggedModel
    {
        public CategoriesCreateEditModel()
        {
            Category = new Category();
        }

        public CategoriesCreateEditModel(OperationType type) : this()
        {
            Type = type;
        }

        public CategoriesCreateEditModel(OperationType type, String categoryName) : this(type)
        {
            Category = Admin.GetCategoryByName(categoryName);
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
                name = value;

                if (Type == OperationType.Creation)
                    Category.Name = value;
            }
        }



        public Boolean IsMoveCRUD { get; private set; }


        internal void DefineAction(HttpRequestBase request)
        {
            IsMoveCRUD = request.Path.Contains(RouteNames.ACCOUNT);
        }



        internal DFMCoreException CreateEdit()
        {
            Category.User = Current.User;

            try
            {
                if (Type == OperationType.Creation)
                    Admin.CreateCategory(Category);
                else
                    Admin.UpdateCategory(Category, Name);
            }
            catch (DFMCoreException e)
            {
				if (e.Type != ExceptionPossibilities.CategoryAlreadyExists)
					return e;
            }

            return null;
        }


    }
}