using DFM.Entities;
using DFM.Core.Helpers;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.Core.Mappings
{
    public class DetailMap : IAutoMappingOverride<Detail>
    {
        public void Override(AutoMapping<Detail> mapping)
        {
            mapping.Not.LazyLoad();

            mapping.Map(d => d.Description)
                .Length(MaximumLength.DetailDescription);

            mapping.Map(d => d.Amount)
                .Default("1");
        }
    }
}
