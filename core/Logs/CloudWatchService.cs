using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Amazon.CloudWatchLogs;
using Amazon.CloudWatchLogs.Model;
using DFM.Generic;
using DFM.Generic.Settings;
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

	public async Task CreateStreams()
	{
		var streams = await client.DescribeLogStreamsAsync(new DescribeLogStreamsRequest
		{
			LogGroupName = Cfg.Log.Group,
		});

		foreach (var appType in EnumX.AllExcept(AppType.None))
		foreach (var division in EnumX.AllValues<Division>())
		{
			var logStreamName = getLogStreamName(appType, division);
			var logStream = streams.LogStreams
				.SingleOrDefault(ls => ls.LogStreamName == logStreamName);

			if (logStream == null)
			{
				await client.CreateLogStreamAsync(new CreateLogStreamRequest
				{
					LogGroupName = Cfg.Log.Group,
					LogStreamName = logStreamName
				});
			}
		}
	}

	private protected override async Task saveLog(Division division, Object content)
	{
		var request = new PutLogEventsRequest
		{
			LogGroupName = Cfg.Log.Group,
			LogStreamName = getLogStreamName(Cfg.AppType, division),
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

	private static String getLogStreamName(AppType appType, Division division)
	{
		return $"{appType}-{division}".ToLower();
	}
}
