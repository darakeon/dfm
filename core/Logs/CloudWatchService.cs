using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Amazon.CloudWatchLogs;
using Amazon.CloudWatchLogs.Model;
using DFM.Generic;
using DFM.Logs.Data;

namespace DFM.Logs;

public class CloudWatchService : BaseLogService, ILogService
{
	public CloudWatchService()
	{
		if (!Cfg.Log.CloudWatchFilled)
			throw new SystemError("Must have section Log whole configured for aws");

		var region = RegionEndpoint.GetBySystemName(Cfg.Log.Region);
		var accessKey = Cfg.Log.AccessKey;
		var secretKey = Cfg.Log.SecretKey;

		client = new AmazonCloudWatchLogsClient(accessKey, secretKey, region);
	}

	private readonly AmazonCloudWatchLogsClient client;

	private protected override async Task saveLog(Division division, Object content)
	{
		var request = new PutLogEventsRequest
		{
			LogGroupName = Cfg.Log.Group,
			LogStreamName = division.ToString(),
			LogEvents = new List<InputLogEvent>
			{
				new()
				{
					Message = content.ToString(),
					Timestamp = DateTime.UtcNow
				}
			}
		};

		await client.PutLogEventsAsync(request, CancellationToken.None);
	}
}
