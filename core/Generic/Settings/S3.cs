using System;
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
				Bucket = s3["bucket"];
				AccessKey = s3["accessKey"];
				SecretKey = s3["secretKey"];
			}
		}

		public readonly String Region;
		public readonly String Bucket;
		public readonly String AccessKey;
		public readonly String SecretKey;

		public readonly Boolean Local;
		public readonly String Directory;

		public Boolean LocalFilled =>
			Local
			&& !String.IsNullOrEmpty(Directory);

		public Boolean S3Filled =>
			!Local
			&& !String.IsNullOrEmpty(Region)
			&& !String.IsNullOrEmpty(Bucket)
			&& !String.IsNullOrEmpty(AccessKey)
			&& !String.IsNullOrEmpty(SecretKey);
	}
}
