using System;
using System.Collections.Generic;
using System.Linq;
using DFM.BusinessLogic.Repositories;
using DFM.Entities;
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

		public Builder(Repos repos, Action<Action> inTransaction, String code)
		{
			this.repos = repos;
			this.inTransaction = inTransaction;
			this.code = code;

			objects = new Dictionary<String, Func<User, IEntityLong>>
			{
				{nameof(Control), controlFor},
				{nameof(Settings), settingsFor},

				{nameof(Acceptance), acceptanceFor},
				{nameof(Security), securityFor},
				{nameof(Ticket), ticketFor},
				{nameof(Tips), tipsFor},

				{nameof(Account), accountFor},
				{nameof(Category), categoryFor},

				{nameof(Move), moveFor},
				{ $"{nameof(Move)} with Detail", moveDetailedFor},
				{nameof(Detail), detailFor},
				{nameof(Summary), summaryFor},
				{nameof(Schedule), scheduleFor},
				{ $"{nameof(Schedule)} with Detail", scheduleDetailedFor},

				{nameof(Archive), archiveFor},
				{nameof(Line), lineFor},
				{ $"{nameof(Line)} with Detail", lineDetailedFor},
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

		private Tips tipsFor(User user)
		{
			return repos.Tips.SaveOrUpdate(
				new Tips
				{
					User = user,
					Countdown = 0,
					Last = 0,
					Permanent = 0,
					Repeat = 0,
					Temporary = 0,
					Type = TipType.Mobile,
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
			var acceptance = new Acceptance
			{
				Contract = contract(),
				User = user,
				CreateDate = DateTime.UtcNow
			};

			repos.Acceptance.SaveOrUpdate(acceptance);

			return acceptance;
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
			return moveFor(user, null, false);
		}

		private Move moveDetailedFor(User user)
		{
			return moveFor(user, " detailed", true);
		}

		private Move moveFor(User user, String suffix, Boolean detailed)
		{
			var category = categoryFor(user, "move" + suffix);
			var accountIn = accountFor(user, "in" + suffix);
			var accountOut = accountFor(user, "out" + suffix);

			var move = new Move
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
			};

			if (!detailed)
				move.ValueCents = 27;

			move = repos.Move.SaveOrUpdate(move);

			if (detailed)
				detailFor(3, move);

			return move;
		}

		private void detailFor(Int32 count, IEntityLong parent)
		{
			for (var d = 0; d < count; d++)
			{
				var detail = new Detail
				{
					Guid = Guid.NewGuid(),
					Description = $"Detail {d+1}",
					Amount = 3,
					ValueCents = 9,
				};

				Action<Detail> addToParent;

				if (parent is Move move)
				{
					detail.Move = move;
					addToParent = move.DetailList.Add;
				}
				else if (parent is Schedule schedule)
				{
					detail.Schedule = schedule;
					addToParent = schedule.DetailList.Add;
				}
				else if (parent is Line line)
				{
					detail.Line = line;
					addToParent = line.DetailList.Add;
				}
				else
				{
					throw new NotImplementedException();
				}

				repos.Detail.SaveOrUpdate(detail);
				addToParent(detail);
			}
		}

		private Detail detailFor(User user)
		{
			var move = moveFor(user, "detail", true);

			return move.DetailList[0];
		}

		private Schedule scheduleFor(User user)
		{
			return scheduleFor(user, null, false);
		}

		private Schedule scheduleDetailedFor(User user)
		{
			return scheduleFor(user, " detailed", true);
		}

		private Schedule scheduleFor(User user, String suffix, Boolean detailed)
		{
			var category = categoryFor(user, "schedule" + suffix);
			var accountIn = accountFor(user, "in");
			var accountOut = accountFor(user, "out");

			var schedule = new Schedule
			{
				Guid = Guid.NewGuid(),
				Description = $"Schedule {code}",
				Year = 3000,
				Month = 3,
				Day = 27,
				Category = category,
				Nature = MoveNature.Transfer,
				In = accountIn,
				Out = accountOut,
				Boundless = false,
				Times = 10,
				User = user,
			};

			if (!detailed)
				schedule.ValueCents = 27;
			else
				detailFor(3, schedule);

			return repos.Schedule.SaveOrUpdate(schedule);
		}

		private Control controlFor(User user)
		{
			return user.Control;
		}

		private Settings settingsFor(User user)
		{
			return user.Settings;
		}

		private Summary summaryFor(User user)
		{
			return repos.Summary.GetAll()
				.FirstOrDefault(s => s.User().ID == user.ID);
		}

		private Archive archiveFor(User user)
		{
			var archive = new Archive
			{
				Guid = Guid.NewGuid(),
				Filename = $"File {code}",
				LineList = new List<Line>(),
				Status = ImportStatus.Pending,
				User = user,
			};

			return repos.Archive.SaveOrUpdate(archive);
		}

		private Line lineFor(User user)
		{
			return lineFor(user, false);
		}

		private Line lineDetailedFor(User user)
		{
			return lineFor(user, true);
		}

		private Line lineFor(User user, Boolean detailed)
		{
			var archive = archiveFor(user);

			var line = new Line
			{
				Description = $"Schedule {code}",
				Date = new DateTime(1986, 3, 27),
				Category = "Category",
				Nature = MoveNature.Transfer,
				In = "Account In",
				Out = "Account Out",
				Status = ImportStatus.Pending,
				Archive = archive,
				Position = 1,
			};

			if (!detailed)
				line.Value = 27;
			else
				detailFor(3, line);

			return repos.Line.SaveOrUpdate(line);
		}
	}
}
