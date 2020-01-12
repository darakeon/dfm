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


		public MoveInfo GetMove(Int64 id)
		{
			parent.Safe.VerifyUser();
			return MoveInfo.Convert4Edit(
				GetMoveEntity(id)
			);
		}

		internal Move GetMoveEntity(Int64 id)
		{
			var move = moveRepository.Get(id);

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
				move.ID == 0
					? OperationType.Creation
					: OperationType.Edition;

			return InTransaction(
				() => parent.BaseMove.SaveMove(
					move, operationType
				)
			);
		}


		public MoveResult DeleteMove(Int64 id)
		{
			parent.Safe.VerifyUser();

			var result = InTransaction(() => deleteMove(id));

			parent.BaseMove.FixSummaries();

			return result;
		}

		private MoveResult deleteMove(Int64 id)
		{
			var move = GetMoveEntity(id);

			parent.BaseMove.VerifyMove(move);

			moveRepository.Delete(id);

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

		public MoveInfo CheckMove(Int64 moveId, PrimalMoveNature nature)
		{
			return toggleMoveCheck(moveId, nature, true);
		}

		public MoveInfo UncheckMove(Int64 moveId, PrimalMoveNature nature)
		{
			return toggleMoveCheck(moveId, nature, false);
		}

		private MoveInfo toggleMoveCheck(Int64 moveId, PrimalMoveNature nature, Boolean check)
		{
			parent.Safe.VerifyUser();

			var move = GetMoveEntity(moveId);

			parent.BaseMove.VerifyMove(move);
			verifyMoveForCheck(move, nature, check);

			move.Check(nature, check);

			InTransaction(() => 
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
