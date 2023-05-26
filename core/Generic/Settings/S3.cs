using System;
using Microsoft.Extensions.Configuration;

namespace DFM.Generic.Settings
{
	public class S3
	{
		public S3(IConfiguration s3)
		{
			Test = Boolean.Parse(s3["Test"] ?? String.Empty);

			if (Test)
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

		public readonly Boolean Test;
		public readonly String Directory;

		private Boolean testFilled =>
			Test && !String.IsNullOrEmpty(Directory);

		private Boolean prodFilled =>
			!String.IsNullOrEmpty(Region)
			&& !String.IsNullOrEmpty(Bucket)
			&& !String.IsNullOrEmpty(AccessKey)
			&& !String.IsNullOrEmpty(SecretKey);

		public Boolean Filled => prodFilled || testFilled;
	}
}
