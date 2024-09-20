using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace DFM.Generic.Settings
{
	public class S3
	{
		public S3(IConfiguration s3)
		{
			Local = !String.IsNullOrEmpty(s3["Local"])
				&& Boolean.Parse(s3["Local"]);

			if (Local)
			{
				Directory = s3["Directory"];
			}
			else
			{
				Region = s3["region"];
				AccessKey = s3["accessKey"];
				SecretKey = s3["secretKey"];

				Buckets = new Dictionary<StoragePurpose, String>
				{
					{ StoragePurpose.Wipe, s3["bucketWipe"] },
					{ StoragePurpose.Export, s3["bucketExport"] },
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
