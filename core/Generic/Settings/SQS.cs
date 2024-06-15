using System;
using Microsoft.Extensions.Configuration;

namespace DFM.Generic.Settings;

public class SQS
{
	public SQS(IConfiguration sqs)
	{
		Local = !String.IsNullOrEmpty(sqs["Local"])
		        && Boolean.Parse(sqs["Local"]);

		QueueImporter = sqs["QueueImporter"];

		if (!Local)
		{
			Account = sqs["Account"];
			Region = sqs["region"];
			AccessKey = sqs["accessKey"];
			SecretKey = sqs["secretKey"];
		}
	}

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
