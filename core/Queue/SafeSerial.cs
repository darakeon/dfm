using System.Collections;
using Keon.Util.DB;
using Newtonsoft.Json;

namespace DFM.Queue;

internal class SafeSerial
{
	private static readonly JsonSerializerSettings settings = new()
	{
		ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
	};

	internal static String Serialize(object obj)
	{
		var properties = toDictionary(obj);

		return JsonConvert.SerializeObject(properties, settings);
	}

	private static Dictionary<String, object> toDictionary(object obj)
	{
		var properties = new Dictionary<String, object>();

		foreach (var property in obj.GetType().GetProperties())
		{
			if (property.GetMethod == null)
				continue;

			var value = property.GetValue(obj);

			switch (value)
			{
				case null:
				case IEntityShort or IEntity or IEntityLong:
				case Byte[]:
					break;

				case IEnumerable enumerable and not String:
				{
					var list = new List<object>();

					foreach (var item in enumerable)
					{
						list.Add(toDictionary(item));
					}

					properties.Add(property.Name, list);
					break;
				}

				default:
					properties.Add(property.Name, value);
					break;
			}
		}

		return properties;
	}

	internal static T? Deserialize<T>(String json)
	{
		return JsonConvert.DeserializeObject<T>(json, settings);
	}
}
