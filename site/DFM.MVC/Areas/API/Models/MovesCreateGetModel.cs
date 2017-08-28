using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities.Enums;
using DFM.MVC.Areas.API.Helpers;

namespace DFM.MVC.Areas.API.Models
{
    internal class MovesCreateGetModel : BaseApiModel
    {
        public MovesCreateGetModel(String accountUrl, Int32? id)
        {
	        if (id.HasValue && id != 0)
	        {
		        var move = Money.GetMoveById(id.Value);

		        Move = new MovesCreatePostModel(move);
	        }
	        else
	        {
				Move = new MovesCreatePostModel
				{
					AccountInUrl = accountUrl,
					AccountOutUrl = accountUrl
				};
	        }

	        AccountList = Current.User.VisibleAccountList()
                .Select(a => new SelectItem<String, String>(a.Name, a.Url))
                .ToList();

            UseCategories = Current.User.Config.UseCategories;

            if (UseCategories)
            {
                CategoryList = Current.User.VisibleCategoryList()
                    .Select(a => new SelectItem<String, String>(a.Name, a.Name))
                    .ToList();
            }

            NatureList = AccountList.Count > 1
                ? SelectItemEnum.SelectItem<MoveNature>() 
                : SelectItemEnum.SelectItem<PrimalMoveNature>();
        }

        public Boolean UseCategories { get; set; }

		public MovesCreatePostModel Move { get; set; }

        public IList<SelectItem<String, String>> AccountList { get; set; }
        public IList<SelectItem<String, String>> CategoryList { get; set; }
        public IList<SelectItem<String, Int32>> NatureList { get; set; }

    }
}