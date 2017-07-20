using DFM.Entities;
using DFM.BusinessLogic.Helpers;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.Repositories.Mappings
{
    public class MoveMap : IAutoMappingOverride<Move>
    {
        public void Override(AutoMapping<Move> mapping)
        {
            mapping.Not.LazyLoad();

            mapping.Map(m => m.Description)
                .Length(MaximumLength.MoveDescription);

            mapping.HasMany(m => m.DetailList)
                .Cascade.Delete();

            mapping.References(m => m.In)
                .Cascade.None()
                .Nullable();

            mapping.References(m => m.Out)
                .Cascade.None()
                .Nullable();

            mapping.References(m => m.Schedule)
                .Cascade.None()
                .Nullable();
        }
    }
}
