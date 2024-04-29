using System;
using System.IO;
using CsvHelper.Configuration;
using DFM.Generic;

namespace DFM.Exchange.Exporter
{
	public class LocalFileService : IFileService
	{
		public LocalFileService()
		{
			if (!Cfg.S3.LocalFilled)
				throw new ConfigurationException("Must have section S3 whole configured for local files");
		}

		public void Upload(String path)
		{
			if (!File.Exists(path))
			{
				throw new FileNotFoundException(
					$"The file should exist at {path}"
				);
			}

			var s3Path = getS3Path(path);
			File.Copy(path, s3Path);
		}

		public void Download(String path)
		{
			var s3Path = getS3Path(path);

			if (File.Exists(path))
				File.Delete(path);

			File.Copy(s3Path, path);
		}

		public Boolean Exists(String path)
		{
			var s3Path = getS3Path(path);
			return File.Exists(s3Path);
		}

		public void Delete(String path)
		{
			var s3Path = getS3Path(path);
			File.Delete(s3Path);
		}

		private static String getS3Path(String path)
		{
			var info = new FileInfo(path);
			var filename = info.Name;
			return Path.Combine(Cfg.S3.Directory, filename);
		}
	}
}
