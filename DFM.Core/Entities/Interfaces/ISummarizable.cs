using System;
using System.Collections.Generic;

namespace DFM.Core.Entities.Interfaces
{
    internal interface ISummarizable
    {
        IList<Summary> SummaryList { get; set; }
        Int32 Time { get; set; }

        Double CheckUp(Category category);
        void AddSummary(Summary summary);
    }
}
