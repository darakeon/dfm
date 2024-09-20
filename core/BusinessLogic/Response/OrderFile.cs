using System;

namespace DFM.BusinessLogic.Response;

public class OrderFile
{
	public OrderFile(String path, String content)
	{
		Path = path;
		Content = content;
	}

	public String Path { get; set; }
	public String Content { get; set; }
}
