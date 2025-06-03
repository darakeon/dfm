from datetime import datetime, timezone
from logging import Handler
from os import environ
from threading import Lock
from traceback import print_exc

from boto3 import client


class Boto3CloudWatchHandler(Handler):
	LOGS_OFF = environ.get('LOGS_OFF') == '1'
	LOGS_URL = environ.get('LOGS_URL')
	LOGS_ACCESS_KEY = environ.get('LOGS_ACCESS_KEY')
	LOGS_SECRET_KEY = environ.get('LOGS_SECRET_KEY')
	LOGS_REGION = environ.get('LOGS_REGION')
	LOGS_GROUP = environ.get('LOGS_GROUP')
	LOGS_STREAM = environ.get('LOGS_STREAM')

	FORMAT = '%(asctime)s %(name)s [%(levelname)s]: %(message)s'

	MAX_MSG_SIZE = 256 * 1024  # 256 KB

	sequence_token = ''
	lock = Lock()

	def __init__(self):
		super().__init__()

		if self.LOGS_OFF:
			return

		if (
			not self.LOGS_ACCESS_KEY
			or not self.LOGS_SECRET_KEY
			or not self.LOGS_REGION
			or not self.LOGS_GROUP
			or not self.LOGS_STREAM
		):
			raise Exception("One or more environment variables not configured")
		
		self.logs = client(
			'logs',
			aws_access_key_id=self.LOGS_ACCESS_KEY,
			aws_secret_access_key=self.LOGS_SECRET_KEY,
			region_name=self.LOGS_REGION,
			endpoint_url=self.LOGS_URL,
		)

		response = self.logs.describe_log_streams(
			logGroupName=self.LOGS_GROUP,
			logStreamNamePrefix=self.LOGS_STREAM,
		)

		stream_exists =	[
			stream['logStreamName']
			for stream in response['logStreams']
				if stream['logStreamName'] == self.LOGS_STREAM
		]

		if not stream_exists:
			self.logs.create_log_stream(
				logGroupName=self.LOGS_GROUP,
				logStreamName=self.LOGS_STREAM
			)


	def emit(self, record):
		with self.lock:
			try:
				msg = self.format(record)
			except Exception:
				msg = record.getMessage()

			for i in range(0, len(msg), self.MAX_MSG_SIZE):
				chunk = msg[i:i + self.MAX_MSG_SIZE]
				self.log_chunk(chunk)

	def log_chunk(self, chunk):
		event = {
			'logGroupName': self.LOGS_GROUP,
			'logStreamName': self.LOGS_STREAM,
			'logEvents': [
				{
					'timestamp': int(datetime.now(timezone.utc).timestamp() * 1000),
					'message': chunk
				},
			],
		}

		if self.sequence_token:
			event['sequenceToken'] = self.sequence_token

		self.try_log(event)

	def try_log(self, event, retry=3):
		try:
			response = self.logs.put_log_events(**event)
			self.sequence_token = response['nextSequenceToken']

		except self.logs.exceptions.InvalidSequenceTokenException as sequence_error:
			if retry > 0:
				event['sequenceToken'] = sequence_error.response['expectedSequenceToken']
				self.try_log(event, retry - 1)
			else:
				print(event)
				print("Error sending log:")
				print_exc()

		except Exception as generic_error:
			print("Error sending log:")
			print_exc()
