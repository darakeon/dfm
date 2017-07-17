using DFM.Core.Entities;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.Core.Mappings
{
    public class ScheduleMap : IAutoMappingOverride<Schedule>
    {
        public void Override(AutoMapping<Schedule> mapping)
        {
            mapping.References(d => d.MoveList)
                .Cascade.None();

            mapping.References(d => d.User)
                .Cascade.None();
        }
    }
}
