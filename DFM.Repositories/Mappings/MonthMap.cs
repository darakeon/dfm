using DFM.Entities;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.Repositories.Mappings
{
    public class MonthMap : IAutoMappingOverride<Month>
    {
        public void Override(AutoMapping<Month> mapping)
        {
            mapping.Not.LazyLoad();

            mapping.HasMany(m => m.InList)
                .Cascade.None();

            mapping.HasMany(m => m.OutList)
                .Cascade.None();

            mapping.Map(m => m.Time)
                .UniqueKey("Month_TimeYear");

            mapping.References(m => m.Year)
                .UniqueKey("Month_TimeYear")
                .Cascade.SaveUpdate();

            mapping.HasMany(m => m.SummaryList)
                .Cascade.SaveUpdate();
        }
    }
}
