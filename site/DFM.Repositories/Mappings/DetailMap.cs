using DFM.Entities;
using DFM.BusinessLogic.Helpers;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.Repositories.Mappings
{
	public class DetailMap : IAutoMappingOverride<Detail>
	{
		public void Override(AutoMapping<Detail> mapping)
		{
			mapping.IgnoreProperty(d => d.Value);

			mapping.Map(d => d.Description)
				.Length(MaximumLength.Detail_Description);

			mapping.Map(d => d.Amount)
				.Default("1");

			mapping.IgnoreProperty(d => d.FakeID);

			mapping.References(d => d.Move)
				.Nullable();

			mapping.References(d => d.Schedule)
				.Nullable();

		}
	}
}
