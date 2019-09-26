using System;
using System.Collections.Generic;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Repositories;
using DFM.BusinessLogic.Response;
using DFM.Email;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Entities.Extensions;
using Error = DFM.BusinessLogic.Exceptions.Error;

namespace DFM.BusinessLogic.Services
{
	public class RobotService : BaseService
	{
		private readonly ScheduleRepository scheduleRepository;
		private readonly MoveRepository moveRepository;
		private readonly DetailRepository detailRepository;

		internal RobotService(
			ServiceAccess serviceAccess,
			ScheduleRepository scheduleRepository,
			MoveRepository moveRepository,
			DetailRepository detailRepository
		)
			: base(serviceAccess)
		{
			this.scheduleRepository = scheduleRepository;
			this.detailRepository = detailRepository;
			this.moveRepository = moveRepository;
		}



		public EmailStatus RunSchedule()
		{
			parent.Safe.VerifyUser();

			var useCategories = parent.Current.UseCategories;

			var equalResult = runScheduleEqualConfig(useCategories);
			var diffResult = runScheduleDiffConfig(useCategories);

			parent.BaseMove.FixSummaries();

			return max(equalResult, diffResult);
		}

		private EmailStatus runScheduleEqualConfig(Boolean useCategories)
		{
			var user = parent.Safe.GetCurrent();
			var sameConfigList = scheduleRepository.GetRunnable(user, useCategories);

			return runSchedule(sameConfigList);
		}

		private EmailStatus runScheduleDiffConfig(Boolean useCategories)
		{
			var user = parent.Safe.GetCurrent();
			var diffConfigList = scheduleRepository.GetRunnable(user, !useCategories);

			EmailStatus emailsSent;

			try
			{
				var mainConfig = new ConfigInfo { UseCategories = !useCategories };
				parent.Admin.UpdateConfig(mainConfig);
				emailsSent = runSchedule(diffConfigList);
			}
			finally
			{
				var mainConfig = new ConfigInfo { UseCategories = useCategories };
				parent.Admin.UpdateConfig(mainConfig);
			}

			return emailsSent;
		}

		private EmailStatus runSchedule(IEnumerable<Schedule> scheduleList)
		{
			var emailsStati = new List<EmailStatus>();

			foreach (var schedule in scheduleList)
			{
				var result = InTransaction(() =>
					addNewMoves(schedule)
				);

				emailsStati.AddRange(result);
			}

			if (!emailsStati.Any())
				return EmailStatus.EmailSent;

			return emailsStati.Max();
		}

		private IEnumerable<EmailStatus> addNewMoves(Schedule schedule)
		{
			var emailsStati = new List<EmailStatus>();

			while (schedule.CanRunNow())
			{
				var newMove = schedule.GetNewMove();

				schedule.LastRun++;

				var result = parent.BaseMove.SaveMove(
					newMove, OperationType.Scheduling
				);

				var move = moveRepository.Get(result.ID);

				schedule.MoveList.Add(move);
				emailsStati.Add(result.Email);
			}

			if (!schedule.CanRun())
				schedule.Active = false;

			scheduleRepository.Save(schedule);

			return emailsStati;
		}

		private static EmailStatus max(EmailStatus equalResult, EmailStatus diffResult)
		{
			return equalResult > diffResult
				? equalResult
				: diffResult;
		}



		public ScheduleResult SaveSchedule(ScheduleInfo info)
		{
			parent.Safe.VerifyUser();

			if (info == null)
				throw Error.ScheduleRequired.Throw();

			return InTransaction(
				() => save(info)
			);
		}

		private ScheduleResult save(ScheduleInfo info)
		{
			var schedule = new Schedule
			{
				Out = parent.BaseMove.GetAccountByUrl(info.OutUrl),
				In = parent.BaseMove.GetAccountByUrl(info.InUrl),
				Category = parent.BaseMove.GetCategoryByName(info.CategoryName),
				User = parent.Safe.GetCurrent()
			};

			info.Update(schedule);

			if (schedule.ID == 0 || !schedule.IsDetailed())
			{
				scheduleRepository.Save(schedule);
				detailRepository.SaveDetails(schedule);
			}
			else
			{
				detailRepository.SaveDetails(schedule);
				scheduleRepository.Save(schedule);
			}

			return new ScheduleResult(schedule.ID);
		}

		public void DisableSchedule(Int64 id)
		{
			parent.Safe.VerifyUser();

			var user = parent.Safe.GetCurrent();

			InTransaction(() => 
				scheduleRepository.Disable(id, user)
			);
		}

		public IList<ScheduleInfo> GetScheduleList()
		{
			parent.Safe.VerifyUser();

			var user = parent.Safe.GetCurrent();

			return scheduleRepository
				.SimpleFilter(
					s => s.Active && s.User.ID == user.ID
				)
				.Select(ScheduleInfo.Convert)
				.ToList();
		}
	}


}
