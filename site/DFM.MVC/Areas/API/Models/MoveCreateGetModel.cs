using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities.Enums;
using DFM.MVC.Areas.API.Helpers;

namespace DFM.MVC.Areas.API.Models
{
    internal class MoveCreateGetModel : BaseApiModel
    {
        public MoveCreateGetModel(String accountUrl)
        {
            AccountList = Current.User.AccountList
                .Where(a => a.Url != accountUrl)
                .Select(a => new SelectItem<String, String>(a.Name, a.Url))
                .ToList();

            CategoryList = Current.User.CategoryList
                .Select(a => new SelectItem<String, String>(a.Name, a.Name))
                .ToList();

            if (AccountList.Any())
            {
                NatureList = Enum.GetValues(typeof (MoveNature))
                    .Cast<MoveNature>()
                    .Select(n => new SelectItem<Int32, String>((Int32)n, n.ToString()))
                    .ToList();
            }
            else
            {
                NatureList = Enum.GetValues(typeof(PrimalMoveNature))
                    .Cast<PrimalMoveNature>()
                    .Select(n => new SelectItem<Int32, String>((Int32)n, n.ToString()))
                    .ToList();
            }
        }

        public IList<SelectItem<String, String>> AccountList { get; set; }
        public IList<SelectItem<String, String>> CategoryList { get; set; }
        public IList<SelectItem<Int32, String>> NatureList { get; set; }

    }
}