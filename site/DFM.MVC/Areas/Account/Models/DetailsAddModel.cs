using System;
using System.Collections.Generic;
using DFM.Entities;
using DFM.MVC.Models;

namespace DFM.MVC.Areas.Account.Models
{
	public class DetailsAddModel : BaseModel
	{
		public DetailsAddModel()
		{
			DetailList = new List<Detail>();
		}

		public DetailsAddModel(Int32 position)
			: this()
		{
			Position = position;

			for (var d = 0; d <= position; d++)
			{
				DetailList.Add(new Detail());
			}
		}

		public DetailsAddModel(Detail detail, Int32 position)
			: this(position)
		{
			DetailList[position] = detail;
		}

		public Int32 Position { get; set; }
		public IList<Detail> DetailList { get; set; }


	}
}