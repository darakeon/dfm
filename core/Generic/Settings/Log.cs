using System;
using Microsoft.Extensions.Configuration;

namespace DFM.Generic.Settings;

public class Log
{
	public Log(IConfiguration config)
	{
		Local = !String.IsNullOrEmpty(config["Local"])
		        && Boolean.Parse(config["Local"]);

		if (Local)
		{
			Path = config["path"];
		}
		else
		{
			Account = config["Account"];
			AccessKey = config["accessKey"];
			SecretKey = config["secretKey"];
			Region = config["region"];
			Group = config["group"];
		}
	}

	public readonly Boolean Local;

	public readonly String Path;

	public readonly String Account;
	public readonly String AccessKey;
	public readonly String SecretKey;
	public readonly String Region;
	public readonly String Group;

	public Boolean LocalFilled =>
		Local && !String.IsNullOrEmpty(Path);

	public Boolean CloudWatchFilled =>
		!Local
		&& !String.IsNullOrEmpty(Account)
		&& !String.IsNullOrEmpty(AccessKey)
		&& !String.IsNullOrEmpty(SecretKey)
		&& !String.IsNullOrEmpty(Region)
		&& !String.IsNullOrEmpty(Group);
}
