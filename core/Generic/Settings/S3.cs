using System;
using Microsoft.Extensions.Configuration;

namespace DFM.Generic.Settings
{
	public class S3
	{
		public S3(IConfiguration s3)
		{
			Region = s3["region"];
			Bucket = s3["bucket"];
			AccessKey = s3["accessKey"];
			SecretKey = s3["secretKey"];
		}

		public readonly String Region;
		public readonly String Bucket;
		public readonly String AccessKey;
		public readonly String SecretKey;

		public Boolean Filled =>
			!String.IsNullOrEmpty(Region)
			&& !String.IsNullOrEmpty(Bucket)
			&& !String.IsNullOrEmpty(AccessKey)
			&& !String.IsNullOrEmpty(SecretKey);
	}
}
