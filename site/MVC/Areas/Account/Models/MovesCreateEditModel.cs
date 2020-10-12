using System;
using System.Linq;
using DFM.BusinessLogic.Response;
using DFM.Email;
using DFM.Entities.Enums;

namespace DFM.MVC.Areas.Account.Models
{
	public class MovesCreateEditModel : BaseMovesModel
	{
		public MovesCreateEditModel()
			: base(new MoveInfo(), OperationType.Creation)  { }

		public MovesCreateEditModel(Guid id)
			: base(service.Money.GetMove(id), OperationType.Edition) { }

		internal override void Save()
		{
			var value = move.Value;
			var details = move.DetailList.ToList();

			if (IsDetailed)
				move.Value = 0;
			else
				move.DetailList.Clear();

			var result = money.SaveMove(move);

			if (!result.Email.IsWrong()) return;

			move.Value = value;
			move.DetailList = details;

			var message = translator["MoveSave"];
			var error = translator[result.Email].ToLower();
			var final = String.Format(message, error);

			errorAlert.AddTranslated(final);
		}

		public Guid Guid { set => GenericMove.Guid = value; }

		private MoveInfo move => (MoveInfo) GenericMove;

		public override Boolean ShowRemoveCheck => move.Checked;
	}
}
