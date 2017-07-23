using DFM.Entities;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.Repositories.Mappings
{
    public class SummaryMap : IAutoMappingOverride<Summary>
    {
        public void Override(AutoMapping<Summary> mapping)
        {
            mapping.Map(s => s.In, "In_");
            mapping.Map(s => s.Out, "Out_");

            mapping.References(s => s.Month)
                .UniqueKey("Summary_CategoryTime")
                .Cascade.None()
                .Nullable();

            mapping.References(s => s.Year)
                .UniqueKey("Summary_CategoryTime")
                .Cascade.None()
                .Nullable();

            mapping.References(s => s.Category)
                .UniqueKey("Summary_CategoryTime")
                .Cascade.None();
        }

    }
}
