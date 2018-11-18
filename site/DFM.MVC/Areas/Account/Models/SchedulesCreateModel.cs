using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using DK.MVC.Forms;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Generic;
using DFM.MVC.Helpers.Global;

namespace DFM.MVC.Areas.Account.Models
{
	public class SchedulesCreateModel : BaseMovesModel
	{
		public SchedulesCreateModel()
			: base(new Schedule(), OperationType.Schedule)
		{
			FrequencySelectList =
				SelectListExtension.CreateSelect(MultiLanguage.GetEnumNames<ScheduleFrequency>());
		}



		internal override void SaveOrUpdate()
		{
			robot.SaveOrUpdateSchedule(Schedule, AccountOutUrl, AccountInUrl, CategoryName);
		}



		public Schedule Schedule
		{
			get { return (Schedule)GenericMove; }
			set { GenericMove = value; }
		}

		[Required]
		public ScheduleFrequency Frequency
		{
			get { return Schedule.Frequency; }
			set { Schedule.Frequency = value; }
		}

		[Required]
		public Boolean Boundless
		{
			get { return Schedule.Boundless; }
			set { Schedule.Boundless = value; }
		}

		[Required]
		public Int16 Times
		{
			get { return Schedule.Times; }
			set { Schedule.Times = value; }
		}

		public Boolean ShowInstallment
		{
			get { return Schedule.ShowInstallment; }
			set { Schedule.ShowInstallment = value; }
		}

		public SelectList FrequencySelectList { get; set; }


	}
}