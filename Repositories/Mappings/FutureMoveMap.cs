using DFM.Entities;
using DFM.BusinessLogic.Helpers;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.Repositories.Mappings
{
    public class FutureMoveMap : IAutoMappingOverride<FutureMove>
    {
        public void Override(AutoMapping<FutureMove> mapping)
        {
            mapping.Not.LazyLoad();

            mapping.Map(m => m.Description)
                .Length(MaximumLength.MoveDescription);

            mapping.HasMany(m => m.DetailList)
                .Cascade.None()
                .Not.LazyLoad();

            mapping.References(m => m.In)
                .Cascade.None()
                .Nullable();

            mapping.References(m => m.Out)
                .Cascade.None()
                .Nullable();

            mapping.References(m => m.Schedule)
                .Cascade.None()
                .Nullable();

            mapping.References(m => m.Category)
                .Cascade.None();

        }
    }
}
