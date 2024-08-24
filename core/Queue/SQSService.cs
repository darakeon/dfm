using System.Net;
using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using DFM.Entities;
using DFM.Generic;

namespace DFM.Queue;

public class SQSService : IQueueService, IDisposable
{
	public SQSService()
	{
		if (!Cfg.SQS.SQSFilled)
			throw new SystemError("Must have section SQS whole configured for aws");

		var region = RegionEndpoint.GetBySystemName(Cfg.SQS.Region);
		var accessKey = Cfg.SQS.AccessKey;
		var secretKey = Cfg.SQS.SecretKey;

		sqsClient = new(accessKey, secretKey, region);
	}

	private static String importerQueueUrl =>
		$"https://sqs.{Cfg.SQS.Region}.amazonaws.com/{Cfg.SQS.Account}/{Cfg.SQS.QueueImporter}";

	private readonly AmazonSQSClient sqsClient;

	public async Task<IDictionary<Line, Boolean>> Enqueue(IList<Line> lineList)
	{
		var lineDic = lineList.ToDictionary(
			l => l.ID.ToString(), l => l
		);

		var messages =
			lineDic
				.Select(line => new SendMessageBatchRequestEntry(
					line.Key, SafeSerial.Serialize(line.Value)
				))
				.ToList();

		var response = await sqsClient.SendMessageBatchAsync(
			importerQueueUrl, messages
		);

		validateResponse(
			response,
			$"Enqueue {String.Join(",", lineList.Select(l => l.ID))}"
		);

		var failed = response.Failed.ToDictionary(
			r => lineDic[r.Id], _ => false
		);

		var succeed = response.Successful.ToDictionary(
			r => lineDic[r.Id], _ => true
		);

		return failed.Union(succeed)
			.ToDictionary(i => i.Key, i => i.Value);
	}

	public async Task<KeyValuePair<String, Line>?> Dequeue()
	{
		var response = await sqsClient.ReceiveMessageAsync(
			new ReceiveMessageRequest(importerQueueUrl)
			{
				MaxNumberOfMessages = 1
			}
		);

		validateResponse(response, "Dequeue");

		if (!response.Messages.Any())
			return null;

		var message = response.Messages.Single();

		var line = SafeSerial.Deserialize<Line>(message.Body);

		if (line == null)
			return null;

		return new KeyValuePair<String, Line>(message.ReceiptHandle, line);
	}

	public async void Delete(String key)
	{
		var response = await sqsClient.DeleteMessageAsync(
			importerQueueUrl, key
		);

		validateResponse(response, $"Delete key {key}");
	}

	private static void validateResponse(AmazonWebServiceResponse response, String operation)
	{
		if (response.HttpStatusCode >= HttpStatusCode.BadRequest)
			throw new QueueError($"{operation} failed: {response.HttpStatusCode}");
	}

	public void Dispose()
	{
		sqsClient.Dispose();
	}
}
