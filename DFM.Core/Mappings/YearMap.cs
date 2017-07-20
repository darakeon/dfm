using DFM.Entities;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.Core.Mappings
{
    public class YearMap : IAutoMappingOverride<Year>
    {
        public void Override(AutoMapping<Year> mapping)
        {
            mapping.Not.LazyLoad();

            mapping.HasMany(y => y.MonthList)
                .Cascade.Delete();

            mapping.References(y => y.Account)
                .Cascade.None();

            mapping.HasMany(y => y.SummaryList)
                .Cascade.SaveUpdate();
        }
    }
}
