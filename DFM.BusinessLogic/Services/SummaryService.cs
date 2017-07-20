using DFM.BusinessLogic.Bases;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using DFM.Entities;
using DFM.BusinessLogic.Exceptions;

namespace DFM.BusinessLogic.Services
{
    internal class SummaryService : BaseService<Summary>
    {
        internal SummaryService(IRepository<Summary> repository) : base(repository) { }


        internal void AjustValue(Summary summary)
        {
            ISummarizable summarizable;

            switch (summary.Nature)
            {
                case SummaryNature.Month:
                    summarizable = summary.Month;
                    break;
                case SummaryNature.Year:
                    summarizable = summary.Year;
                    break;
                default:
                    throw DFMCoreException.WithMessage(ExceptionPossibilities.SummaryNatureNotFound);
            }

            summary.In = summarizable.CheckUpIn(summary.Category);
            summary.Out = summarizable.CheckUpOut(summary.Category);

            SaveOrUpdate(summary);
        }

    }
}
