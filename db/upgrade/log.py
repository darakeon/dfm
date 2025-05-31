from datetime import datetime, timezone
from os import environ

from boto3 import client


class Log:
	RED = 31
	BLACK = 30
	PURPLE = 35

	def __init__(self):
		self.cloud = AWSLog()

	def title_red(self, text):
		self.title_colored(text, self.RED)

	def title_black(self, text):
		self.title_colored(text, self.BLACK)

	def title_purple(self, text):
		self.title_colored(text, self.PURPLE)

	def title_colored(self, text, color):
		print(f"\033[01;{color}m")
		self.empty()
		print("---------------------------------------------------------------------------------")
		self.text(f"--- {text} {self.now()}")
		print("---------------------------------------------------------------------------------")
		self.empty()
		print("\033[00m")

	def empty(self):
		self.text("")

	def text(self, text):
		print(text)

		if not text:
			return

		self.cloud.log(text, self.now())

	def now(self):
		return datetime.now(timezone.utc)


class AWSLog:
	LOGS_OFF = environ.get('LOGS_OFF') == '1'
	LOGS_URL = environ.get('LOGS_URL')
	LOGS_ACCESS_KEY = environ.get('LOGS_ACCESS_KEY')
	LOGS_SECRET_KEY = environ.get('LOGS_SECRET_KEY')
	LOGS_REGION = environ.get('LOGS_REGION')
	LOGS_GROUP = environ.get('LOGS_GROUP')
	LOGS_STREAM = environ.get('LOGS_STREAM')

	def __init__(self):
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

	def log(self, text, time):
		if self.LOGS_OFF:
			return

		self.logs.put_log_events(
			logGroupName=self.LOGS_GROUP,
			logStreamName=self.LOGS_STREAM,
			logEvents=[
				{
					'timestamp': int(time.timestamp() * 1000),
					'message': text
				},
			],
		)
