using DFM.Core.Entities;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.Core.Mappings
{
    public class SummaryMap : IAutoMappingOverride<Summary>
    {
        public void Override(AutoMapping<Summary> mapping)
        {
            mapping.References(s => s.Month)
                .UniqueKey("Summary_CategoryTime")
                .Cascade.SaveUpdate()
                .Nullable();

            mapping.References(s => s.Year)
                .UniqueKey("Summary_CategoryTime")
                .Cascade.SaveUpdate()
                .Nullable();

            mapping.References(s => s.Category)
                .UniqueKey("Summary_CategoryTime")
                .Cascade.None();
        }
    }
}
