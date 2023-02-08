from boto3 import client
from os import environ


class S3:
	def __init__(self):
		self.s3 = client('s3')
		self.bucket = environ.get('AWS_BUCKET')

	def rename_file(self, old_name, new_name):
		copy_source = {
			'Bucket': self.bucket,
			'Key': old_name,
		}

		self.s3.copy_object(
			Bucket = self.bucket,
			CopySource = copy_source,
			Key = new_name,
		)

		self.s3.delete_object(
			Bucket = self.bucket,
			Key = old_name,
		)
