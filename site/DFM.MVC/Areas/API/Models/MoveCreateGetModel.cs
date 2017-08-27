using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities.Enums;
using DFM.Entities.Extensions;
using DFM.MVC.Areas.API.Helpers;

namespace DFM.MVC.Areas.API.Models
{
    internal class MoveCreateGetModel : BaseApiModel
    {
        public MoveCreateGetModel(String accountUrl)
        {
            AccountList = Current.User.VisibleAccountList()
                .Where(a => a.Url != accountUrl)
                .Select(a => new SelectItem<String, String>(a.Name, a.Url))
                .ToList();

            CategoryList = Current.User.VisibleCategoryList()
                .Select(a => new SelectItem<String, String>(a.Name, a.Name))
                .ToList();

            NatureList = AccountList.Any() 
                ? SelectItemEnum.SelectItem<MoveNature>() 
                : SelectItemEnum.SelectItem<PrimalMoveNature>();
        }

        public IList<SelectItem<String, String>> AccountList { get; set; }
        public IList<SelectItem<String, String>> CategoryList { get; set; }
        public IList<SelectItem<String, Int32>> NatureList { get; set; }

    }
}