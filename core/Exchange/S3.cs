using System;
using System.IO;
using Amazon;
using Amazon.S3.Transfer;
using CsvHelper.Configuration;
using DFM.Generic;

namespace DFM.Exchange
{
	public class S3 : IDisposable
	{
		public S3()
		{
			if (!Cfg.S3.Filled)
				throw new ConfigurationException("Must have section S3 whole configured");

			var region = RegionEndpoint.GetBySystemName(Cfg.S3.Region);
			var accessKey = Cfg.S3.AccessKey;
			var secretKey = Cfg.S3.SecretKey;

			bucket = Cfg.S3.Bucket;

			s3 = new TransferUtility(accessKey, secretKey, region);
		}

		private readonly String bucket;
		private readonly TransferUtility s3;

		public void Upload(String path)
		{
			var info = new FileInfo(path);

			if (!info.Exists)
			{
				throw new FileNotFoundException(
					$"The file should exist at {info.FullName}"
				);
			}

			s3.Upload(path, bucket);
		}

		public void Dispose()
		{
			s3?.Dispose();
		}
	}
}
