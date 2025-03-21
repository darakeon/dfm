using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace DFM.Generic.Settings
{
	public class Storage
	{
		public Storage(IConfiguration config)
		{
			Local = !String.IsNullOrEmpty(config["Local"])
				&& Boolean.Parse(config["Local"]);

			if (Local)
			{
				Directory = config["Directory"];
			}
			else
			{
				Region = config["region"];
				AccessKey = config["accessKey"];
				SecretKey = config["secretKey"];

				Buckets = new Dictionary<StoragePurpose, String>
				{
					{ StoragePurpose.Wipe, config["bucketWipe"] },
					{ StoragePurpose.Export, config["bucketExport"] },
				};
			}
		}

		public readonly String Region;
		public readonly String AccessKey;
		public readonly String SecretKey;

		public readonly IDictionary<StoragePurpose, String> Buckets;

		public readonly Boolean Local;
		public readonly String Directory;

		public Boolean LocalFilled =>
			Local
			&& !String.IsNullOrEmpty(Directory);

		public Boolean S3Filled =>
			!Local
			&& !String.IsNullOrEmpty(Region)
			&& !String.IsNullOrEmpty(AccessKey)
			&& !String.IsNullOrEmpty(SecretKey);
	}
}
