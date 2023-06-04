using System;
using System.IO;
using Amazon;
using Amazon.S3.Transfer;
using CsvHelper.Configuration;
using DFM.Generic;

namespace DFM.Exchange
{
	public class S3 : IDisposable, IFileService
	{
		public S3()
		{
			if (!Cfg.S3.Filled)
				throw new ConfigurationException("Must have section S3 whole configured");

			if (Cfg.S3.Test)
				return;

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

			if (Cfg.S3.Test)
			{
				var filename = info.Name;
				var destinyPath = Path.Combine(Cfg.S3.Directory, filename);
				File.Copy(path, destinyPath);
			}
			else
			{
				s3.Upload(path, bucket);
			}
		}

		public void Download(String path)
		{
			var info = new FileInfo(path);
			var key = info.Name;

			if (Cfg.S3.Test)
			{
				var filename = info.Name;
				var originPath = Path.Combine(Cfg.S3.Directory, filename);
				File.Copy(originPath, path);
			}
			else
			{
				s3.Download(path, bucket, key);
			}
		}

		public void Dispose()
		{
			s3?.Dispose();
		}
	}
}
