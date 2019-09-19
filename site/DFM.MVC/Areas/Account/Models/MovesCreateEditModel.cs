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
			: base(new MoveInfo())  { }

		public MovesCreateEditModel(OperationType type)
			: base(new MoveInfo(), type) { }

		public MovesCreateEditModel(Int32 id, OperationType type)
			: base(Service.Access.Money.GetMove(id), type) { }


		internal override void Save()
		{
			var value = Move.Value;
			var details = Move.DetailList.ToList();

			if (IsDetailed)
				Move.Value = null;
			else
				Move.DetailList.Clear();

			var result = money.SaveMove(Move);

			if (result.Email.IsWrong())
			{
				Move.Value = value;
				Move.DetailList = details;

				var message = Translator.Dictionary["MoveSave"];
				var error = Translator.Dictionary[result.Email].ToLower();
				var final = String.Format(message, error);

				ErrorAlert.AddTranslated(final);
			}
		}


		public MoveInfo Move
		{
			get => (MoveInfo) GenericMove;
			set => GenericMove = value;
		}


	}

}
