﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DFM.BaseWeb.Starters
{
	public class Serialization : DefaultContractResolver
	{
		public static void Set(MvcNewtonsoftJsonOptions options)
		{
			options.SerializerSettings.ContractResolver = new Serialization();
		}

		public Serialization()
		{
			NamingStrategy = new KebabCaseNamingStrategy();
		}

		protected override JsonPrimitiveContract CreatePrimitiveContract(Type type)
		{
			var realType = getRealType(type);

			if (realType.IsEnum)
				return new EnumContract(type, realType);

			return base.CreatePrimitiveContract(type);
		}

		private static Type getRealType(Type type)
		{
			var isNullable =
				type.IsGenericType
				&& type.Name == typeof(Int32?).Name
				&& type.GenericTypeArguments.Length == 1;

			return isNullable
				? type.GenericTypeArguments[0] 
				: type;
		}

		protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
		{
			var property = base.CreateProperty(member, memberSerialization);

			property.ShouldSerialize = instance => shouldSerialize(member, instance);

			return property;
		}

		private static Boolean shouldSerialize(MemberInfo member, Object instance)
		{
			return member is not PropertyInfo info 
				|| info.GetValue(instance, null) != null;
		}

		class EnumContract : JsonPrimitiveContract
		{
			public EnumContract([NotNull] Type type, Type realType) : base(type)
			{
				Converter = new EnumConverter(realType);
			}
		}

		class EnumConverter(Type realType) : JsonConverter
		{
			public override void WriteJson(JsonWriter writer, Object value, JsonSerializer serializer)
			{
				if (value == null)
				{
					serializer.Serialize(writer, null);
				}
				else
				{
					serializer.Serialize(writer, new
					{
						code = (Int32)value,
						text = kebabCase(value.ToString()),
					});
				}
			}

			private String kebabCase(String text)
			{
				var regex = new Regex("([A-Z])");
				text = regex.Replace(text, "-$1");
				if (text.StartsWith("-"))
					text = text.Substring(1);
				return text.ToLower();
			}

			public override Object ReadJson(JsonReader reader, Type type, Object existingValue, JsonSerializer serializer)
			{
				var value = readEnumValue(reader, serializer);

				if (value == null) return null;

				value = value.Replace("-", "");

				Enum.TryParse(realType, value, true, out var result);

				return result;
			}

			private static String readEnumValue(JsonReader reader, JsonSerializer serializer)
			{
				if (reader.TokenType != JsonToken.StartObject)
					return serializer.Deserialize<String>(reader);

				var jsonObject = serializer.Deserialize<Dictionary<String, String>>(reader);

				return jsonObject == null ? null
					: jsonObject.TryGetValue("code", out var code) ? code
					: jsonObject.TryGetValue("text", out var text) ? text
					: null;
			}

			public override Boolean CanConvert(Type objectType)
			{
				return objectType.IsEnum;
			}
		}


	}
}
