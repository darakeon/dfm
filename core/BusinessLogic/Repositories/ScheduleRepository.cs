using System;
using System.Collections.Generic;
using System.Linq;
using DFM.BusinessLogic.Bases;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Response;
using DFM.Entities;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using Keon.NHibernate.Queries;

namespace DFM.BusinessLogic.Repositories
{
	internal class ScheduleRepository() : GenericMoveRepository<Schedule>(
		MaxLen.ScheduleDescription,
		Error.TooLargeScheduleDescription
	)
	{
		internal Schedule Save(Schedule schedule)
		{
			if (schedule.ID == 0)
				schedule.Active = true;

			return SaveOrUpdate(schedule, validate, complete);
		}

		new void validate(Schedule schedule)
		{
			base.validate(schedule);

			if (!schedule.Boundless && schedule.Times <= 0)
				throw Error.ScheduleTimesCantBeZero.Throw();
		}

		internal IList<Schedule> GetRunnable(User user)
		{
			return getRunnable(user).List
				.Where(s => s.LastDateRun() < user.Now())
				.ToList();
		}

		internal Schedule Get(Guid guid)
		{
			return SingleOrDefault(s => s.ExternalId == guid.ToByteArray());
		}

		internal void Disable(Guid guid, User user)
		{
			var schedule = Get(guid);

			if (schedule == null || schedule.User.ID != user.ID)
				throw Error.InvalidSchedule.Throw();

			disable(schedule);
		}

		private void disable(Schedule schedule)
		{
			if (!schedule.Active)
				throw Error.DisabledSchedule.Throw();

			schedule.Active = false;
			SaveOrUpdate(schedule);
		}

		public void DisableAll(Account account)
		{
			var scheduleList = Where(
				s => s.Active
				&& (s.In.ID == account.ID
					|| s.Out.ID == account.ID)
			);

			foreach (var schedule in scheduleList)
			{
				disable(schedule);
			}
		}

		public void DeleteAll(Account account)
		{
			var scheduleList = Where(
				s => s.In.ID == account.ID
				|| s.Out.ID == account.ID
			);

			foreach (var schedule in scheduleList)
			{
				if (schedule.Active)
				{
					throw Error.CantDeleteAccountWithSchedules.Throw();
				}

				Delete(schedule);
			}
		}

		internal override User GetUser(Schedule entity)
		{
			return entity.User;
		}

		internal void AddDeleted(Schedule schedule)
		{
			schedule = Get(schedule.ID);
			schedule.Deleted++;
			SaveOrUpdate(schedule);
		}

		public void UpdateState(Schedule schedule)
		{
			var lastRun = schedule.LastRun;
			var active = schedule.CanRun();

			schedule = Get(schedule.ID);

			schedule.LastRun = lastRun;
			schedule.Active = active;

			SaveOrUpdate(schedule);
		}

		public IList<Move> SimulateMoves(Account account, Int16 dateYear, Int16 dateMonth)
		{
			var foreseenMoves = new List<Move>();

			var schedules = getRunnable(account);

			foreach (var schedule in schedules)
			{
				foreseenMoves.AddRange(
					schedule.CreateMovesByFrequency(dateYear, dateMonth)
				);
			}

			return foreseenMoves
				.OrderBy(d => d.GetDate())
				.ToList();
		}

		public Decimal GetForeseenTotal(Account account, Int16 dateYear)
		{
			return GetForeseenTotal(account, dateYear, 12);
		}

		public Decimal GetForeseenTotal(Account account, Int16 dateYear, Int16 dateMonth)
		{
			return getRunnable(account).Sum(
				s => s.PreviewSumUntil(
					account, dateYear, dateMonth
				)
			) / 100m;
		}

		private Int32 getForeseenAt(Account account, Int16 dateYear, Int16 dateMonth, PrimalMoveNature nature)
		{
			return getRunnable(account, nature).Sum(
				s => s.PreviewSumAt(
					account, dateYear, dateMonth
				)
			);
		}

		private IList<Schedule> getRunnable(Account account, PrimalMoveNature? nature = null)
		{
			var query = getRunnable(account.User);

			switch (nature)
			{
				case PrimalMoveNature.In:
					query = query.Where(s => s.In == account);
					break;

				case PrimalMoveNature.Out:
					query = query.Where(s => s.Out == account);
					break;

				default:
					query = query.Where(s => s.In == account || s.Out == account);
					break;
			}

			return query.List;
		}

		private Query<Schedule, Int64> getRunnable(User user)
		{
			return NewQuery()
				.Where(s => s.User == user && s.Active)
				.Where(s => s.Boundless || s.LastRun < s.Times);
		}

		internal void FillForeseenTotals(Account account, Int16 dateYear, IList<YearReport.MonthItem> months)
		{
			for (var n = 1; n < 13; n++)
			{
				var number = dateYear * 100 + n;
				var month = months.SingleOrDefault(m => m.Number == number);

				if (month == null)
				{
					month = new YearReport.MonthItem {Number = +number};
					months.Add(month);
				}

				var dateMonth = (Int16)n;

				month.ForeseenInCents = getForeseenAt(
					account, dateYear, dateMonth, PrimalMoveNature.In
				);

				month.ForeseenOutCents = getForeseenAt(
					account, dateYear, dateMonth, PrimalMoveNature.Out
				);
			}
		}

		public IList<Schedule> ByAccount(Account account)
		{
			return Where(
				s => (s.In != null && s.In.ID == account.ID)
					|| (s.Out != null && s.Out.ID == account.ID)
			);
		}

		public IList<Schedule> ByCategory(Category category)
		{
			return Where(s => s.Category.ID == category.ID);
		}

		public void UpdateCategory(Schedule schedule, Category category)
		{
			schedule.Category = category;
			SaveOrUpdate(schedule);
		}
	}
}
