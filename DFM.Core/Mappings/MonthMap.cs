using DFM.Entities;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.Core.Mappings
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

            mapping.References(m => m.Year)
                .Cascade.SaveUpdate();

            mapping.HasMany(m => m.SummaryList)
                .Cascade.SaveUpdate();
        }
    }
}
