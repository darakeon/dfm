using System.Runtime.CompilerServices;
using DFM.Generic;

[assembly: InternalsVisibleTo("DFM.BusinessLogic.Tests")]
namespace DFM.Files
{
	public class LocalFileService : IFileService
	{
		public LocalFileService(StoragePurpose purpose)
		{
			if (!Cfg.S3.LocalFilled)
				throw new SystemError("Must have section S3 whole configured for local files");

			fakeS3Path = Path.Combine(Cfg.S3.Directory, purpose.ToString());
		}

		private String fakeS3Path;

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

			File.Copy(s3Path, path, true);
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

		internal IList<String> List()
		{
			return Directory.GetFiles(fakeS3Path);
		}

		private String getS3Path(String path)
		{
			var info = new FileInfo(path);
			var filename = info.Name;
			return Path.Combine(fakeS3Path, filename);
		}
	}
}
