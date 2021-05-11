using System;
using DFM.Entities;
using DFM.Entities.Bases;

namespace DFM.Exchange.Tests
{
	public class TestSchedule : Schedule
	{
		public TestSchedule()
		{
			Guid = Guid.NewGuid();
		}

		public DateTime Date
		{
			set => this.SetDate(value);
		}

		public String CategoryName
		{
			set => base.Category = new Category { Name = value };
		}

		public String InName
		{
			set => base.In = new Account { Name = value };
		}

		public String OutName
		{
			set => base.Out = new Account { Name = value };
		}
	}
}
