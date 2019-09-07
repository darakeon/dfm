using System;
using System.Linq;
using DFM.Email;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.MVC.Helpers;
using DFM.MVC.Helpers.Global;

namespace DFM.MVC.Areas.Account.Models
{
	public class MovesCreateEditModel : BaseMovesModel
	{
		public MovesCreateEditModel()
			: base(new Move())  { }

		public MovesCreateEditModel(OperationType type)
			: base(new Move(), type) { }

		public MovesCreateEditModel(Int32 id, OperationType type)
			: base(Service.Access.Money.GetMoveById(id), type) { }


		internal override void SaveOrUpdate()
		{
			var value = Move.Value;
			var details = Move.DetailList.ToList();

			if (IsDetailed)
				Move.Value = null;
			else
				Move.DetailList.Clear();

			var result = money.SaveOrUpdateMove(Move, AccountOutUrl, AccountInUrl, CategoryName);

			if (result.Error.IsWrong())
			{
				Move.Value = value;
				Move.DetailList = details;

				var message = Translator.Dictionary["MoveSave"];
				var error = Translator.Dictionary[result.Error].ToLower();
				var final = String.Format(message, error);

				ErrorAlert.AddTranslated(final);
			}
		}


		public Move Move
		{
			get { return (Move) GenericMove; }
			set { GenericMove = value; }
		}


	}

}
