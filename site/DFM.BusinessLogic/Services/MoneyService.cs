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
			Parent.Safe.VerifyUser();
			return MoveInfo.Convert(
				GetMoveEntity(id)
			);
		}

		internal Move GetMoveEntity(Int64 id)
		{
			var move = moveRepository.Get(id);

			Parent.BaseMove.VerifyMove(move);

			return move;
		}

		public MoveResult SaveMove(MoveInfo move)
		{
			Parent.Safe.VerifyUser();

			var result = save(move);

			Parent.BaseMove.FixSummaries();

			return result;
		}

		private MoveResult save(MoveInfo move)
		{
			var operationType =
				move.ID == 0
					? OperationType.Creation
					: OperationType.Edition;

			return InTransaction(
				() => Parent.BaseMove.SaveMove(
					move, operationType
				)
			);
		}


		public MoveResult DeleteMove(Int64 id)
		{
			Parent.Safe.VerifyUser();

			var result = InTransaction(() => deleteMove(id));

			Parent.BaseMove.FixSummaries();

			return result;
		}

		private MoveResult deleteMove(Int64 id)
		{
			var move = GetMoveEntity(id);

			Parent.BaseMove.VerifyMove(move);

			moveRepository.Delete(id);

			Parent.BaseMove.BreakSummaries(move);

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
				Parent.Admin.UpdateConfigWithinTransaction(mainConfig);
			}

			if (schedule.Category != null && !useCategories)
			{
				var mainConfig = new ConfigInfo { UseCategories = true };
				Parent.Admin.UpdateConfigWithinTransaction(mainConfig);
			}

			scheduleRepository.SaveOrUpdate(schedule);

			if (schedule.User.Config.UseCategories != useCategories)
			{
				var mainConfig = new ConfigInfo { UseCategories = useCategories };
				Parent.Admin.UpdateConfigWithinTransaction(mainConfig);
			}
		}

		public MoveResult CheckMove(Int64 moveId)
		{
			return toggleMoveCheck(moveId, true);
		}

		public MoveResult UncheckMove(Int64 moveId)
		{
			return toggleMoveCheck(moveId, false);
		}

		private MoveResult toggleMoveCheck(Int64 moveId, Boolean check)
		{
			Parent.Safe.VerifyUser();

			var move = GetMoveEntity(moveId);

			Parent.BaseMove.VerifyMove(move);
			verifyMoveForCheck(move, check);

			move.Checked = check;
			var now = Parent.Current.User.Now();

			InTransaction(() => 
				moveRepository.Save(move, now)
			);

			return new MoveResult(move);
		}

		private void verifyMoveForCheck(Move move, Boolean check)
		{
			if (!Parent.Current.User.Config.MoveCheck)
				throw Error.MoveCheckDisabled.Throw();

			if (move.Checked != check)
				return;

			var error = move.Checked
				? Error.MoveAlreadyChecked
				: Error.MoveAlreadyUnchecked;

			throw error.Throw();
		}
	}
}
