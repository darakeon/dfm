using System;
using Microsoft.Extensions.Configuration;

namespace DFM.Generic.Settings;

public class Queue
{
	public Queue(IConfiguration config)
	{
		Local = !String.IsNullOrEmpty(config["Local"])
		        && Boolean.Parse(config["Local"]);

		QueueImporter = config["QueueImporter"];

		if (!Local)
		{
			QueueLimit = Int32.Parse(config["QueueLimit"]);
			Account = config["Account"];
			Region = config["region"];
			AccessKey = config["accessKey"];
			SecretKey = config["secretKey"];
		}
	}

	public readonly Int32 QueueLimit = Int32.MaxValue;
	public readonly String Account;
	public readonly String Region;
	public readonly String QueueImporter;
	public readonly String AccessKey;
	public readonly String SecretKey;

	public readonly Boolean Local;

	public Boolean LocalFilled =>
		Local && !String.IsNullOrEmpty(QueueImporter);

	public Boolean SQSFilled =>
		!Local
		&& !String.IsNullOrEmpty(Account)
		&& !String.IsNullOrEmpty(Region)
		&& !String.IsNullOrEmpty(QueueImporter)
		&& !String.IsNullOrEmpty(AccessKey)
		&& !String.IsNullOrEmpty(SecretKey);
}
