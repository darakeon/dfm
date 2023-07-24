from boto3 import client
from botocore.exceptions import ClientError
from os import environ


class S3:
	def __init__(self):
		self.s3 = client('s3')
		self.bucket = environ.get('AWS_BUCKET')

	def exists(self, name):
		try:
			self.s3.head_object(
				Bucket = self.bucket,
				Key = name,
			)

			return True

		except ClientError as error:
			print(error)
			return False
