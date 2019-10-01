using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities;
using Keon.Util.DB;

namespace DFM.BusinessLogic.Truncate
{
	internal class Entities
	{
		private static List<Entity> entities;
		private static readonly Type @interface = typeof(IEntityLong);

		internal static IList<Entity> Get()
		{
			entities = typeof(User).Assembly
				.GetTypes()
				.Where(isEntity)
				.Select(t => new Entity(t))
				.ToList();

			entities.ForEach(addChildren);

			return entities;
		}

		private static void addChildren(Entity entity)
		{
			var type = entity.Me;
			var properties = type.GetProperties();

			foreach (var property in properties)
			{
				var propType = property.PropertyType;
				if (isList(propType))
				{
					var listType = propType.GenericTypeArguments[0];

					addParent(listType, type);
				}

				else if (isEntity(propType))
				{
					addParent(type, propType);
				}
			}
		}

		private static void addParent(Type entityType, Type parentType)
		{
			var entity = entities.Single(t => t.Me == entityType);
			var parent = entities.Single(t => t.Me == parentType);

			entity.Parents.Add(parent);
		}

		private static Boolean isList(Type type)
		{
			return type.IsGenericType
			       && type.GetInterface(typeof(IEnumerable).Name) != null;
		}

		private static Boolean isEntity(Type type)
		{
			return !type.IsInterface
			       && type.GetInterface(@interface.Name) != null;
		}

		public class Entity
		{
			public Entity(Type me)
			{
				Me = me;
				Parents = new List<Entity>();
			}

			public Type Me { get; }
			public IList<Entity> Parents { get; }

			public String Name => Me.Name;
		}
	}
}
