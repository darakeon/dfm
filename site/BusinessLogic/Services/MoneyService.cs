using System;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Repositories;
using DFM.BusinessLogic.Response;
using DFM.Entities;
using DFM.Entities.Enums;
using Error = DFM.BusinessLogic.Exceptions.Error;

namespace DFM.BusinessLogic.Services
{
	public class MoneyService : BaseService
	{
		private readonly MoveRepository moveRepository;
		private readonly ScheduleRepository scheduleRepository;

		internal MoneyService(ServiceAccess serviceAccess, MoveRepository moveRepository, ScheduleRepository scheduleRepository)
			: base(serviceAccess)
		{
			this.moveRepository = moveRepository;
			this.scheduleRepository = scheduleRepository;
		}


		public MoveInfo GetMove(Guid guid)
		{
			parent.Safe.VerifyUser();
			return MoveInfo.Convert4Edit(
				GetMoveEntity(guid)
			);
		}

		internal Move GetMoveEntity(Guid guid)
		{
			var move = moveRepository.Get(guid);

			parent.BaseMove.VerifyMove(move);

			return move;
		}

		public MoveResult SaveMove(MoveInfo move)
		{
			parent.Safe.VerifyUser();

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


		public MoveResult DeleteMove(Guid guid)
		{
			parent.Safe.VerifyUser();

			var result = inTransaction("DeleteMove", () => deleteMove(guid));

			parent.BaseMove.FixSummaries();

			return result;
		}

		private MoveResult deleteMove(Guid guid)
		{
			var move = GetMoveEntity(guid);

			parent.BaseMove.VerifyMove(move);

			moveRepository.Delete(move);

			parent.BaseMove.BreakSummaries(move);

			if (move.Schedule != null)
			{
				updateScheduleDeleted(move.Schedule);
			}

			var emailStatus = moveRepository.SendEmail(move, OperationType.Deletion);

			return new MoveResult(move, emailStatus);
		}

		private void updateScheduleDeleted(Schedule schedule)
		{
			schedule.Deleted++;

			var useCategories = schedule.User.Config.UseCategories;

			if (schedule.Category == null && useCategories)
			{
				var mainConfig = new ConfigInfo { UseCategories = false };
				parent.Admin.UpdateConfigWithinTransaction(mainConfig);
			}

			if (schedule.Category != null && !useCategories)
			{
				var mainConfig = new ConfigInfo { UseCategories = true };
				parent.Admin.UpdateConfigWithinTransaction(mainConfig);
			}

			scheduleRepository.Save(schedule);

			if (schedule.User.Config.UseCategories != useCategories)
			{
				var mainConfig = new ConfigInfo { UseCategories = useCategories };
				parent.Admin.UpdateConfigWithinTransaction(mainConfig);
			}
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
			parent.Safe.VerifyUser();

			var move = GetMoveEntity(guid);

			parent.BaseMove.VerifyMove(move);
			verifyMoveForCheck(move, nature, check);

			move.Check(nature, check);

			inTransaction("ToggleMoveCheck", () => 
				moveRepository.SaveCheck(move)
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
	}
}
