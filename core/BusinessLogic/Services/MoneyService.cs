using System;
using System.Collections.Generic;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Repositories;
using DFM.BusinessLogic.Response;
using DFM.BusinessLogic.Validators;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Exchange.Importer;
using Error = DFM.BusinessLogic.Exceptions.Error;

namespace DFM.BusinessLogic.Services
{
	public class MoneyService : Service
	{
		internal MoneyService(ServiceAccess serviceAccess, Repos repos, Valids valids)
			: base(serviceAccess, repos, valids) { }

		public MoveResult SaveMove(MoveInfo move)
		{
			parent.Auth.VerifyUser();

			var result = save(move);

			parent.BaseMove.FixSummaries();

			return result;
		}

		private MoveResult save(MoveInfo move)
		{
			var operationType =
				move.Guid == Guid.Empty
					? OperationType.Creation
					: OperationType.Edition;

			return inTransaction("SaveMove",
				() => parent.BaseMove.SaveMove(
					move, operationType
				)
			);
		}

		public MoveInfo GetMove(Guid guid)
		{
			parent.Auth.VerifyUser();
			return MoveInfo.Convert4Edit(
				GetMoveEntity(guid)
			);
		}

		internal Move GetMoveEntity(Guid guid)
		{
			var move = repos.Move.Get(guid);

			parent.BaseMove.VerifyUser(move);

			return move;
		}

		public MoveInfo CheckMove(Guid guid, PrimalMoveNature nature)
		{
			return toggleMoveCheck(guid, nature, true);
		}

		public MoveInfo UncheckMove(Guid guid, PrimalMoveNature nature)
		{
			return toggleMoveCheck(guid, nature, false);
		}

		private MoveInfo toggleMoveCheck(Guid guid, PrimalMoveNature nature, Boolean check)
		{
			parent.Auth.VerifyUser();

			var move = GetMoveEntity(guid);

			parent.BaseMove.VerifyUser(move);
			verifyMoveForCheck(move, nature, check);

			move.Check(nature, check);

			inTransaction("ToggleMoveCheck", () =>
				repos.Move.SaveCheck(move)
			);

			return MoveInfo.Convert4Report(move, nature);
		}

		private void verifyMoveForCheck(Move move, PrimalMoveNature nature, Boolean check)
		{
			if (!parent.Current.MoveCheck)
				throw Error.MoveCheckDisabled.Throw();

			var moveChecked = move.IsChecked(nature);

			if (move.Nature != MoveNature.Transfer)
			{
				var natureToCheck = (Int32)nature;
				var natureOfMove = (Int32)move.Nature;

				if (natureToCheck != natureOfMove)
					throw Error.MoveCheckWrongNature.Throw();
			}

			if (moveChecked == check)
			{
				var error = moveChecked
					? Error.MoveAlreadyChecked
					: Error.MoveAlreadyUnchecked;

				throw error.Throw();
			}
		}

		public MoveResult DeleteMove(Guid guid)
		{
			parent.Auth.VerifyUser();

			var result = inTransaction("DeleteMove", () => deleteMove(guid));

			parent.BaseMove.FixSummaries();

			return result;
		}

		private MoveResult deleteMove(Guid guid)
		{
			var move = GetMoveEntity(guid);

			parent.BaseMove.VerifyUser(move);

			repos.Move.Delete(move);

			parent.BaseMove.BreakSummaries(move);

			if (move.Schedule != null)
			{
				repos.Schedule.AddDeleted(move.Schedule);
			}

			var user = parent.Auth.GetCurrent();
			var security = repos.Security.Grab(user, SecurityAction.UnsubscribeMoveMail);
			var emailStatus = repos.Move.SendEmail(move, OperationType.Deletion, security);

			return new MoveResult(move, emailStatus);
		}

		public void ImportMovesFile(String csv)
		{
			var user = parent.Auth.VerifyUser();

			var importer = new CSVImporter(csv);

			var errors = new Dictionary<ImporterError, Error>
			{
				{ ImporterError.Header, Error.InvalidArchiveColumn },
				{ ImporterError.Empty, Error.InvalidArchive },
				{ ImporterError.DateRequired, Error.MoveDateRequired },
				{ ImporterError.DateInvalid, Error.MoveDateInvalid },
				{ ImporterError.NatureRequired, Error.MoveNatureRequired },
				{ ImporterError.NatureInvalid, Error.MoveNatureInvalid },
				{ ImporterError.ValueInvalid, Error.MoveValueInvalid },
				{ ImporterError.DetailAmountRequired, Error.MoveDetailAmountRequired },
				{ ImporterError.DetailAmountInvalid, Error.MoveDetailAmountInvalid },
				{ ImporterError.DetailValueRequired, Error.MoveDetailValueRequired },
				{ ImporterError.DetailValueInvalid, Error.MoveDetailValueInvalid },
				{ ImporterError.DetailConversionInvalid, Error.MoveDetailConversionInvalid },
			};

			if (importer.Error.HasValue)
				throw errors[importer.Error.Value].Throw();

			var accountsIn = importer.MoveList
				.Select(m => m.In)
				.Distinct()
				.Where(m => !String.IsNullOrEmpty(m))
				.ToDictionary(m => m, _ => Error.InMoveWrong);

			var accountsOut = importer.MoveList
				.Select(m => m.Out)
				.Distinct()
				.Where(m => !String.IsNullOrEmpty(m))
				.ToDictionary(m => m, _ => Error.OutMoveWrong);

			var accounts =
				accountsIn
					.Union(accountsOut)
					.DistinctBy(a => a.Key)
					.ToDictionary(
						a => a.Key,
						a => repos.Account.GetByName(a.Key, user, Error.InvalidAccount)
					);

			var categories =
				importer.MoveList
					.Where(m => !String.IsNullOrEmpty(m.Category))
					.Select(m => m.Category)
					.Distinct()
					.ToDictionary(
						name => name,
						name => repos.Category.GetByName(name, user, Error.InvalidCategory)
					);

			importer.MoveList
				.Select(m => m.ToMove(accounts, categories))
				.ToList()
				.ForEach(m => validate(m, user));

			var archive = new Archive();

			archive.LineList =
				importer.MoveList
					.Select(m => m.ToLine(archive))
					.ToList();

			inTransaction(
				"ImportMovesFile",
				() => repos.Archive.SaveOrUpdate(archive)
			);
		}

		private void validate(Move move, User user)
		{
			valids.Move.Validate(move, user);

			move.DetailList
				.ToList()
				.ForEach(valids.Detail.Validate);
		}
	}
}
