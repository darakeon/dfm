using System;
using System.Collections.Generic;
using Ak.Generic.DB;

namespace DFM.Entities.Bases
{
    public interface ISummarizable : IEntity
    {
        IList<Summary> SummaryList { get; set; }
        Int16 Time { get; set; }

        Summary AddSummary(Category category);

        Summary this[String categoryName] { get; }
    }
}
