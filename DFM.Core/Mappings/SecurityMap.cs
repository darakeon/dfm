using DFM.Core.Entities;
using DFM.Core.Helpers;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.Core.Mappings
{
    public class SecurityMap : IAutoMappingOverride<Security>
    {
        public void Override(AutoMapping<Security> mapping)
        {
            mapping.Not.LazyLoad();

            mapping.Map(s => s.Guid)
                .Length(MaximumLength.SecurityGuid);

        }
    }
}
