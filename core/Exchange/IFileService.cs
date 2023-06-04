using System;

namespace DFM.Exchange;

public interface IFileService
{
	public void Upload(String path);
	public void Download(String path);
}
