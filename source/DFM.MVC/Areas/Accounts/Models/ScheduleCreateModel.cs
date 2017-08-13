using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Ak.MVC.Forms;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Generic;
using DFM.MVC.Helpers;
using DFM.MVC.Helpers.Controllers;
using DFM.Repositories;

namespace DFM.MVC.Areas.Accounts.Models
{
    public class ScheduleCreateModel : GenericMoveModel
    {
        public ScheduleCreateModel()
            : this(new Schedule()) { }

        public ScheduleCreateModel(Schedule schedule)
            : base(schedule, OperationType.Schedule)
        {
            FrequencySelectList =
                SelectListExtension.CreateSelect(MultiLanguage.GetEnumNames<ScheduleFrequency>());
        }



        internal override void SaveOrUpdate(AccountSelector selector)
        {
            Services.Robot.SaveOrUpdateSchedule(Schedule, selector.AccountOutName, selector.AccountInName, CategoryName);
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