using System;
using System.Collections.Generic;

namespace DFM.Core.Entities.Bases
{
    public interface ISummarizable : IEntity
    {
        IList<Summary> SummaryList { get; set; }
        Int32 Time { get; set; }

        Double CheckUpIn(Category category);
        Double CheckUpOut(Category category);
        Summary AddSummary(Category category);
    }
}
