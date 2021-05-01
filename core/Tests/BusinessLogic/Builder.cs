using System;
using System.Collections.Generic;
using DFM.BusinessLogic.Repositories;
using DFM.Entities;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using Keon.Util.DB;
using Keon.Util.Extensions;

namespace DFM.BusinessLogic.Tests
{
	class Builder
	{
		private readonly Repos repos;
		private readonly Action<Action> inTransaction;
		private readonly String code;

		public Builder(Repos repos, Action<Action> inTransaction, string code)
		{
			this.repos = repos;
			this.inTransaction = inTransaction;
			this.code = code;

			objects = new Dictionary<String, Func<User, IEntityLong>>
			{
				{nameof(Account), accountFor},
				{nameof(Category), categoryFor},
				{nameof(Acceptance), acceptanceFor},
				{nameof(Security), securityFor},
				{nameof(Ticket), ticketFor},
				{nameof(Move), moveFor},
				{nameof(Schedule), scheduleFor},
			};
		}

		public void CreateFor(User user, String entityName)
		{
			inTransaction(
				() => objects[entityName](user)
			);
		}

		private readonly IDictionary<String, Func<User, IEntityLong>> objects;

		private Ticket ticketFor(User user)
		{
			return repos.Ticket.SaveOrUpdate(
				new Ticket
				{
					User = user,
					Key = "",
				}
			);
		}

		private Security securityFor(User user)
		{
			return repos.Security.SaveOrUpdate(
				new Security
				{
					User = user,
					Token = "",
				}
			);
		}

		private Contract contract()
		{
			return repos.Contract.SaveOrUpdate(
				new Contract
				{
					Version = $"Auto {code}"
				}
			);
		}

		private Acceptance acceptanceFor(User user)
		{
			return repos.Acceptance.GetOrCreate(user, contract());
		}

		private Account accountFor(User user)
		{
			return accountFor(user, null);
		}

		private Account accountFor(User user, String suffix)
		{
			return repos.Account.SaveOrUpdate(
				new Account
				{
					User = user,
					Url = Token.New(),
					Name = $"Account {code} {suffix}",
				}
			);
		}

		private Category categoryFor(User user)
		{
			return categoryFor(user, null);
		}

		private Category categoryFor(User user, String suffix)
		{
			return repos.Category.SaveOrUpdate(
				new Category
				{
					User = user,
					Name = $"Category {code} {suffix}",
				}
			);
		}

		private Move moveFor(User user)
		{
			var category = categoryFor(user, "move");
			var accountIn = accountFor(user, "in");
			var accountOut = accountFor(user, "out");

			repos.Move.SaveOrUpdate(
				new Move
				{
					Guid = Guid.NewGuid(),
					Description = $"Move {code}",
					Year = 2021,
					Month = 5,
					Day = 7,
					Category = category,
					Nature = MoveNature.Transfer,
					In = accountIn,
					Out = accountOut,
					ValueCents = 27,
				}
			);

			var move = repos.Move.SaveOrUpdate(
				new Move
				{
					Guid = Guid.NewGuid(),
					Description = $"Move {code}",
					Year = 2021,
					Month = 5,
					Day = 7,
					Category = category,
					Nature = MoveNature.Transfer,
					In = accountIn,
					Out = accountOut,
				}
			);

			detailFor(3, move);

			return move;
		}

		private void detailFor(Int32 count, IMove parent)
		{
			var move = parent as Move;
			var schedule = parent as Schedule;

			for (var d = 0; d < count; d++)
			{
				repos.Detail.SaveOrUpdate(
					new Detail
					{
						Guid = Guid.NewGuid(),
						Move = move,
						Schedule = schedule,
						Description = $"Detail {parent.Description} {d+1}",
						Amount = 3,
						ValueCents = 9,
					}
				);
			}
		}

		private Schedule scheduleFor(User user)
		{
			var category = categoryFor(user, "schedule");
			var accountIn = accountFor(user, "in");
			var accountOut = accountFor(user, "out");

			var today = user.Now().Date;
			var firstSch = new DateTime(today.Year, today.Month - 2, 27);
			var secondSch = new DateTime(today.Year, today.Month - 1, 7);

			var schedule = repos.Schedule.SaveOrUpdate(
				new Schedule
				{
					Guid = Guid.NewGuid(),
					Description = $"Schedule {code}",
					Category = category,
					Nature = MoveNature.Transfer,
					In = accountIn,
					Out = accountOut,
					ValueCents = 27,
					Boundless = false,
					Times = 10,
					User = user,
				}.SetDate(firstSch)
			);

			detailFor(3, schedule);

			schedule = repos.Schedule.SaveOrUpdate(
				new Schedule
				{
					Guid = Guid.NewGuid(),
					Description = $"Schedule {code}",
					Category = category,
					Nature = MoveNature.Transfer,
					In = accountIn,
					Out = accountOut,
					ValueCents = 27,
					Boundless = true,
					User = user,
				}.SetDate(secondSch)
			);

			return schedule;
		}
	}
}
