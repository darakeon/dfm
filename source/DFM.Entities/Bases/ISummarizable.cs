using System;
using System.Collections.Generic;

namespace DFM.Entities.Bases
{
    public interface ISummarizable : IEntity
    {
        IList<Summary> SummaryList { get; set; }
        Int16 Time { get; set; }

        Double CheckUpIn(Category category);
        Double CheckUpOut(Category category);
        Summary AddSummary(Category category);

        Summary this[String categoryName] { get; }
    }
}
