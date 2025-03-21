using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using DFM.Generic;

[assembly: InternalsVisibleTo("DFM.BusinessLogic.Tests")]
namespace DFM.Files
{
	public class LocalFileService : IFileService
	{
		public LocalFileService(StoragePurpose purpose)
		{
			if (!Cfg.Storage.LocalFilled)
				throw new SystemError("Must have section Storage whole configured for local files");

			fakeStoragePath = Path.Combine(Cfg.Storage.Directory, purpose.ToString());
		}

		private readonly String fakeStoragePath;

		public void Upload(String path)
		{
			if (!File.Exists(path))
			{
				throw new FileNotFoundException(
					$"The file should exist at {path}"
				);
			}

			var storagePath = getStoragePath(path);
			File.Copy(path, storagePath);
		}

		public void Download(String path)
		{
			var storagePath = getStoragePath(path);

			File.Copy(storagePath, path, true);
		}

		public Boolean Exists(String path)
		{
			var storagePath = getStoragePath(path);
			return File.Exists(storagePath);
		}

		public void Delete(String path)
		{
			var storagePath = getStoragePath(path);
			File.Delete(storagePath);
		}

		internal IList<String> List()
		{
			return Directory.GetFiles(fakeStoragePath);
		}

		private String getStoragePath(String path)
		{
			var info = new FileInfo(path);
			var filename = info.Name;
			return Path.Combine(fakeStoragePath, filename);
		}
	}
}
