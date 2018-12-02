using System;
using DK.Generic.Exceptions;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.ObjectInterfaces;
using DFM.BusinessLogic.Repositories;
using DFM.Email;
using DFM.Entities;
using DFM.Entities.Enums;

namespace DFM.BusinessLogic.Services
{
	public class MoneyService : BaseService
	{
		private readonly MoveRepository moveRepository;
		private readonly DetailRepository detailRepository;
		private readonly ScheduleRepository scheduleRepository;

		internal MoneyService(ServiceAccess serviceAccess, MoveRepository moveRepository, DetailRepository detailRepository, ScheduleRepository scheduleRepository)
			: base(serviceAccess)
		{
			this.moveRepository = moveRepository;
			this.detailRepository = detailRepository;
			this.scheduleRepository = scheduleRepository;
		}


		public Move GetMoveById(Int32 id)
		{
			Parent.Safe.VerifyUser();
			return GetMoveByIdInternal(id);
		}

		internal Move GetMoveByIdInternal(Int32 id)
		{
			var move = moveRepository.Get(id);

			verifyMove(move);

			return move;
		}


		public ComposedResult<Move, EmailStatus> SaveOrUpdateMove(Move move, String accountOutUrl, String accountInUrl, String categoryName)
		{
			Parent.Safe.VerifyUser();

			var result = saveOrUpdate(move, accountOutUrl, accountInUrl, categoryName);

			Parent.BaseMove.FixSummaries();

			return result;
		}

		private ComposedResult<Move, EmailStatus> saveOrUpdate(Move move, String accountOutUrl, String accountInUrl, String categoryName)
		{
			var operationType =
				move.ID == 0
					? OperationType.Creation
					: OperationType.Edition;

			return InTransaction(
				() => Parent.BaseMove.SaveOrUpdateMove(
					move, accountOutUrl, accountInUrl, categoryName, operationType
				),
				() => resetMove(move, operationType)
			);
		}

		private static void resetMove(Move move, OperationType operationType)
		{
			if (operationType == OperationType.Creation)
			{
				move.ID = 0;
			}
		}


		public ComposedResult<Boolean, EmailStatus> DeleteMove(Int32 id)
		{
			Parent.Safe.VerifyUser();

			var result = InTransaction(() => deleteMove(id));

			Parent.BaseMove.FixSummaries();

			return result;
		}

		private ComposedResult<Boolean, EmailStatus> deleteMove(Int32 id)
		{
			var move = GetMoveByIdInternal(id);

			verifyMove(move);

			moveRepository.Delete(id);

			Parent.BaseMove.BreakSummaries(move);

			if (move.Schedule != null)
			{
				updateScheduleDeleted(move.Schedule);
			}

			var emailStatus = moveRepository.SendEmail(move, OperationType.Deletion);

			return new ComposedResult<Boolean, EmailStatus>(true, emailStatus);
		}

		private void updateScheduleDeleted(Schedule schedule)
		{
			schedule.Deleted++;

			var useCategories = schedule.User.Config.UseCategories;

			if (schedule.Category == null && useCategories)
			{
				var mainConfig = new MainConfig { UseCategories = false };
				Parent.Admin.UpdateConfigWithinTransaction(mainConfig);
			}

			if (schedule.Category != null && !useCategories)
			{
				var mainConfig = new MainConfig { UseCategories = true };
				Parent.Admin.UpdateConfigWithinTransaction(mainConfig);
			}

			scheduleRepository.SaveOrUpdate(schedule);

			if (schedule.User.Config.UseCategories != useCategories)
			{
				var mainConfig = new MainConfig { UseCategories = useCategories };
				Parent.Admin.UpdateConfigWithinTransaction(mainConfig);
			}
		}


		public Detail GetDetailById(Int32 id)
		{
			Parent.Safe.VerifyUser();

			var detail = detailRepository.Get(id);

			if (detail == null)
				throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidDetail);

			verifyMove(detail.Move);

			return detail;
		}


		// ReSharper disable once UnusedParameter.Local
		private void verifyMove(Move move)
		{
			if (move == null)
				throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidMove);

			if (move.User.Email != Parent.Current.User.Email)
				throw DFMCoreException.WithMessage(ExceptionPossibilities.Unauthorized);
		}



		public Move CheckMove(Int32 moveId)
		{
			Parent.Safe.VerifyUser();
			return toggleMoveCheck(moveId, true);
		}

		public Move UncheckMove(Int32 moveId)
		{
			Parent.Safe.VerifyUser();
			return toggleMoveCheck(moveId, false);
		}

		private Move toggleMoveCheck(Int32 moveId, Boolean check)
		{
			var move = GetMoveByIdInternal(moveId);

			verifyMove(move);
			verifyMoveForCheck(move, check);

			move.Checked = check;

			InTransaction(() => moveRepository.SaveOrUpdate(move));

			return move;
		}

		private void verifyMoveForCheck(Move move, Boolean check)
		{
			if (!Parent.Current.User.Config.MoveCheck)
			{
				throw DFMCoreException.WithMessage(ExceptionPossibilities.MoveCheckDisabled);
			}

			if (move.Checked == check)
			{
				var error = move.Checked
					? ExceptionPossibilities.MoveAlreadyChecked
					: ExceptionPossibilities.MoveAlreadyUnchecked;

				throw DFMCoreException.WithMessage(error);
			}
		}
	}
}
