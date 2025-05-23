﻿using System;
using System.ComponentModel.DataAnnotations;
using DFM.BusinessLogic.Response;
using Keon.MVC.Forms;
using DFM.Entities.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DFM.MVC.Areas.Account.Models
{
	public class SchedulesCreateModel : BaseMovesModel
	{
		public SchedulesCreateModel()
			: base(new ScheduleInfo(), OperationType.Scheduling)
		{
			FrequencySelectList =
				SelectListExtension.CreateSelect(translator.GetEnumNames<ScheduleFrequency>());
		}

		internal override void Save()
		{
			attendant.SaveSchedule(Schedule);
		}



		public ScheduleInfo Schedule
		{
			get => (ScheduleInfo)GenericMove;
			set => GenericMove = value;
		}

		[Required]
		public ScheduleFrequency Frequency
		{
			get => Schedule.Frequency;
			set => Schedule.Frequency = value;
		}

		[Required]
		public Boolean Boundless
		{
			get => Schedule.Boundless;
			set => Schedule.Boundless = value;
		}

		[Required]
		public Int16 Times
		{
			get => Schedule.Times;
			set => Schedule.Times = value;
		}

		public Boolean ShowInstallment
		{
			get => Schedule.ShowInstallment;
			set => Schedule.ShowInstallment = value;
		}

		public SelectList FrequencySelectList { get; set; }


	}
}
