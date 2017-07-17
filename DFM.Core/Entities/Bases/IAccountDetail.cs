using System;
using DFM.Core.Enums;

namespace DFM.Core.Entities.Bases
{
    public interface IAccountDetail
    {
        Double Value { get; }
        String Description { get; set; }
        DateTime Next { get; set; }

        ScheduleFrequency Frequency { get; set; }
        Category Category { get; set; }
        
        Account Account { get; set; }
    }
}
