using DFM.Core.Entities;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.Core.Mappings
{
    public class YearMap : IAutoMappingOverride<Year>
    {
        public void Override(AutoMapping<Year> mapping)
        {
            mapping.HasMany(y => y.MonthList)
                .Cascade.Delete();

            mapping.References(y => y.Account)
                .Cascade.None();

            mapping.IgnoreProperty(y => y.Value);
        }
    }
}
