using DFM.BusinessLogic.Helpers;
using DFM.Entities;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.Repositories.Mappings
{
    public class ScheduleMap : IAutoMappingOverride<Schedule>
    {
        public void Override(AutoMapping<Schedule> mapping)
        {
            mapping.Map(m => m.Description)
                .Length(MaximumLength.MoveDescription);

            mapping.References(m => m.Category)
                .Cascade.None();

            mapping.Map(s => s.Active)
                .Default("1");

            mapping.References(s => s.User)
                .Cascade.None();
            
            mapping.References(m => m.In)
                .Cascade.None()
                .Nullable();

            mapping.References(m => m.Out)
                .Cascade.None()
                .Nullable();

            mapping.HasMany(m => m.DetailList)
                .Cascade.None()
                .Not.LazyLoad();

        }
    }
}
