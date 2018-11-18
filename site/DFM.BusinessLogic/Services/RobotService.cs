using System;
using System.Collections.Generic;
using System.Linq;
using DK.Generic.Exceptions;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.ObjectInterfaces;
using DFM.BusinessLogic.Repositories;
using DFM.Email;
using DFM.Entities;
using DFM.Entities.Extensions;
using DFM.Generic;

namespace DFM.BusinessLogic.Services
{
	public class RobotService : BaseService
	{
		private readonly ScheduleRepository scheduleRepository;
		private readonly DetailRepository detailRepository;

		internal RobotService(ServiceAccess serviceAccess, ScheduleRepository scheduleRepository,
			DetailRepository detailRepository)
			: base(serviceAccess)
		{
			this.scheduleRepository = scheduleRepository;
			this.detailRepository = detailRepository;
		}



		public EmailStatus RunSchedule()
		{
			Parent.Safe.VerifyUser();

			var useCategories = Parent.Current.User.Config.UseCategories;

			var equalResult = runScheduleEqualConfig(useCategories);
			var diffResult = runScheduleDiffConfig(useCategories);

			Parent.BaseMove.FixSummaries();

			return max(equalResult, diffResult);
		}

		private EmailStatus runScheduleEqualConfig(Boolean useCategories)
		{
			var sameConfigList = scheduleRepository.GetRunnable(Parent.Current.User, useCategories);

			return runSchedule(sameConfigList);
		}

		private EmailStatus runScheduleDiffConfig(Boolean useCategories)
		{
			var diffConfigList = scheduleRepository.GetRunnable(Parent.Current.User, !useCategories);

			EmailStatus emailsSent;

			try
			{
				var mainConfig = new MainConfig { UseCategories = !useCategories };
				Parent.Admin.UpdateConfig(mainConfig);
				emailsSent = runSchedule(diffConfigList);
			}
			finally
			{
				var mainConfig = new MainConfig { UseCategories = useCategories };
				Parent.Admin.UpdateConfig(mainConfig);
			}

			return emailsSent;
		}

		private EmailStatus runSchedule(IEnumerable<Schedule> scheduleList)
		{
			var emailsStati = new List<EmailStatus>();

			foreach (var schedule in scheduleList)
			{
				var accountOutUrl = schedule.Out?.Url;
				var accountInUrl = schedule.In?.Url;

				var category = schedule.Category;
				var categoryName = category == null ? null : schedule.Category.Name;

				var result = addNewMoves(schedule, accountOutUrl, accountInUrl, categoryName);

				emailsStati.AddRange(result);
			}

			if (!emailsStati.Any())
				return EmailStatus.EmailSent;

			return emailsStati.Max();
		}

		private IEnumerable<EmailStatus> addNewMoves(Schedule schedule, String accountOutUrl, String accountInUrl,
			String categoryName)
		{
			var emailsStati = new List<EmailStatus>();

			while (schedule.CanRunNow())
			{
				var newMove = schedule.GetNewMove();

				schedule.LastRun++;

				var result = saveOrUpdateMove(newMove, accountOutUrl, accountInUrl, categoryName);

				schedule.MoveList.Add(result.Success);
				emailsStati.Add(result.Error);
			}

			return emailsStati;
		}

		private ComposedResult<Move, EmailStatus> saveOrUpdateMove(Move move, String accountOutUrl, String accountInUrl,
			String categoryName)
		{
			return InTransaction(() => Parent.BaseMove.SaveOrUpdateMove(move, accountOutUrl, accountInUrl, categoryName));
		}

		private static EmailStatus max(EmailStatus equalResult, EmailStatus diffResult)
		{
			return equalResult > diffResult
				? equalResult
				: diffResult;
		}



		public Schedule SaveOrUpdateSchedule(Schedule schedule, String accountOutUrl, String accountInUrl,
			String categoryName)
		{
			Parent.Safe.VerifyUser();

			if (schedule == null)
				throw DFMCoreException.WithMessage(ExceptionPossibilities.ScheduleRequired);

			var operationType =
				schedule.ID == 0
					? OperationType.Creation
					: OperationType.Edit;

			return InTransaction(
				() => saveOrUpdate(schedule, accountOutUrl, accountInUrl, categoryName),
				() => resetSchedule(schedule, operationType)
			);
		}

		private Schedule saveOrUpdate(Schedule schedule, String accountOutUrl, String accountInUrl, String categoryName)
		{
			schedule.Out = Parent.BaseMove.GetAccountByUrl(accountOutUrl);
			schedule.In = Parent.BaseMove.GetAccountByUrl(accountInUrl);
			schedule.Category = Parent.BaseMove.GetCategoryByName(categoryName);
			schedule.User = Parent.Current.User;

			if (schedule.ID == 0 || !schedule.IsDetailed())
			{
				scheduleRepository.SaveOrUpdate(schedule);
				detailRepository.SaveDetails(schedule);
			}
			else
			{
				detailRepository.SaveDetails(schedule);
				scheduleRepository.SaveOrUpdate(schedule);
			}

			return schedule;
		}

		private static void resetSchedule(Schedule schedule, OperationType operationType)
		{
			if (operationType == OperationType.Creation)
				schedule.ID = 0;
		}



		public void DisableSchedule(Int32 id)
		{
			Parent.Safe.VerifyUser();

			InTransaction(() => scheduleRepository.Disable(id));
		}



		public IList<Schedule> GetScheduleList()
		{
			Parent.Safe.VerifyUser();

			return scheduleRepository.SimpleFilter(
				s => s.Active && s.User.ID == Parent.Current.User.ID
			);
		}

	}


}
