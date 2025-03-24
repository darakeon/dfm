using System;
using Microsoft.Extensions.Configuration;

namespace DFM.Generic.Settings;

public class Log
{
	public Log(IConfiguration config)
	{
		Local = !String.IsNullOrEmpty(config["Local"])
		        && Boolean.Parse(config["Local"]);

		Group = config["Group"];

		if (!Local)
		{
			Account = config["Account"];
			Region = config["region"];
			AccessKey = config["accessKey"];
			SecretKey = config["secretKey"];
		}
	}

	public readonly String Account;
	public readonly String Region;
	public readonly String Group;
	public readonly String AccessKey;
	public readonly String SecretKey;

	public readonly Boolean Local;

	public Boolean LocalFilled =>
		Local && !String.IsNullOrEmpty(Group);

	public Boolean CloudWatchFilled =>
		!Local
		&& !String.IsNullOrEmpty(Account)
		&& !String.IsNullOrEmpty(Region)
		&& !String.IsNullOrEmpty(Group)
		&& !String.IsNullOrEmpty(AccessKey)
		&& !String.IsNullOrEmpty(SecretKey);
}
