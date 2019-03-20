using DFM.Entities;
using DFM.BusinessLogic.Helpers;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.Repositories.Mappings
{
	public class UserMap : IAutoMappingOverride<User>
	{
		public void Override(AutoMapping<User> mapping)
		{
			mapping.Map(u => u.Password)
				.Length(MaximumLength.User_Password);

			mapping.Map(u => u.Email)
				.Length(MaximumLength.User_Email)
				.Unique();

			mapping.Map(u => u.TFASecret)
				.Nullable();

			mapping.Map(u => u.Creation)
				.Default("'2011-09-21'");

			mapping.References(u => u.Config)
				.Cascade.All();

			mapping.Map(u => u.IsAdm)
				.Default("0")
				.Not.Update();
		}
	}
}
