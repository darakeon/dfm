using System;
using Keon.Util.Exceptions;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.InterfacesAndBases;
using DFM.BusinessLogic.Repositories;
using DFM.Email;
using DFM.Entities;
using DFM.Entities.Enums;
using Error = DFM.BusinessLogic.Exceptions.Error;

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


		public Move GetMoveById(Int64 id)
		{
			Parent.Safe.VerifyUser();
			return GetMoveByIdInternal(id);
		}

		internal Move GetMoveByIdInternal(Int64 id)
		{
			var move = moveRepository.Get(id);

			Parent.BaseMove.VerifyMove(move);

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


		public ComposedResult<Boolean, EmailStatus> DeleteMove(Int64 id)
		{
			Parent.Safe.VerifyUser();

			var result = InTransaction(() => deleteMove(id));

			Parent.BaseMove.FixSummaries();

			return result;
		}

		private ComposedResult<Boolean, EmailStatus> deleteMove(Int64 id)
		{
			var move = GetMoveByIdInternal(id);

			Parent.BaseMove.VerifyMove(move);

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
				var mainConfig = new ConfigOptions { UseCategories = false };
				Parent.Admin.UpdateConfigWithinTransaction(mainConfig);
			}

			if (schedule.Category != null && !useCategories)
			{
				var mainConfig = new ConfigOptions { UseCategories = true };
				Parent.Admin.UpdateConfigWithinTransaction(mainConfig);
			}

			scheduleRepository.SaveOrUpdate(schedule);

			if (schedule.User.Config.UseCategories != useCategories)
			{
				var mainConfig = new ConfigOptions { UseCategories = useCategories };
				Parent.Admin.UpdateConfigWithinTransaction(mainConfig);
			}
		}

		public Move CheckMove(Int64 moveId)
		{
			return toggleMoveCheck(moveId, true);
		}

		public Move UncheckMove(Int64 moveId)
		{
			return toggleMoveCheck(moveId, false);
		}

		private Move toggleMoveCheck(Int64 moveId, Boolean check)
		{
			Parent.Safe.VerifyUser();

			var move = GetMoveByIdInternal(moveId);

			Parent.BaseMove.VerifyMove(move);
			verifyMoveForCheck(move, check);

			move.Checked = check;
			var now = Parent.Current.User.Now();

			InTransaction(() => 
				moveRepository.SaveOrUpdate(move, now)
			);

			return move;
		}

		private void verifyMoveForCheck(Move move, Boolean check)
		{
			if (!Parent.Current.User.Config.MoveCheck)
			{
				throw Error.MoveCheckDisabled.Throw();
			}

			if (move.Checked == check)
			{
				var error = move.Checked
					? Error.MoveAlreadyChecked
					: Error.MoveAlreadyUnchecked;

				throw error.Throw();
			}
		}
	}
}
