﻿using System;
using Keon.Util.DB;

namespace DFM.Entities.Extensions
{
	public static class EntityExtension
	{
		public static Boolean Is<T>(this T entity1, T entity2)
			where T : class, IEntityLong
		{
			if (entity1 == null && entity2 == null)
				return true;

			if (entity1 != null && entity2 != null)
				return entity1.ID == entity2.ID;

			return false;
		}
	}
}
