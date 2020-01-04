using System;
using System.Linq;
using DFM.BusinessLogic.Response;
using DFM.Email;
using DFM.Entities.Enums;
using DFM.MVC.Helpers;
using DFM.MVC.Helpers.Global;

namespace DFM.MVC.Areas.Account.Models
{
	public class MovesCreateEditModel : BaseMovesModel
	{
		public MovesCreateEditModel()
			: base(new MoveInfo(), OperationType.Creation)  { }

		public MovesCreateEditModel(Int32 id)
			: base(Service.Access.Money.GetMove(id), OperationType.Edition) { }

		internal override void Save()
		{
			var value = move.Value;
			var details = move.DetailList.ToList();

			if (IsDetailed)
				move.Value = null;
			else
				move.DetailList.Clear();

			var result = money.SaveMove(move);

			if (result.Email.IsWrong())
			{
				move.Value = value;
				move.DetailList = details;

				var message = Translator.Dictionary["MoveSave"];
				var error = Translator.Dictionary[result.Email].ToLower();
				var final = String.Format(message, error);

				ErrorAlert.AddTranslated(final);
			}
		}

		public Int32 ID { set => GenericMove.ID = value; }

		private MoveInfo move => (MoveInfo) GenericMove;

	}

}
