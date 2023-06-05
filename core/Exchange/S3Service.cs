using System;
using System.IO;
using Amazon;
using Amazon.S3.Transfer;
using CsvHelper.Configuration;
using DFM.Generic;

namespace DFM.Exchange
{
	public class S3Service : IDisposable, IFileService
	{
		public S3Service()
		{
			if (!Cfg.S3.S3Filled)
				throw new ConfigurationException("Must have section S3 whole configured for aws");

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

		public void Download(String path)
		{
			var info = new FileInfo(path);
			var key = info.Name;

			s3.Download(path, bucket, key);
		}

		public void Dispose()
		{
			s3?.Dispose();
		}
	}
}
