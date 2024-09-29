from boto3 import client
from os import path, listdir, environ
from re import search, sub
from requests import get, packages


# ACCESS_KEY
# SECRET_KEY
# BUCKET


def publish_contract():
	s3 = client(
		's3', 
		aws_access_key_id=environ['S3_TERMS_ACCESS_KEY'],
		aws_secret_access_key=environ['S3_TERMS_SECRET_KEY'],
	)

	bucket = environ['S3_TERMS_BUCKET']

	upload(s3, bucket, 'terms.html', 'html', 'utf-8')
	upload(s3, bucket, '../site/MVC/Assets/styles/contract.css', 'css')


def upload(s3, bucket, file_path, file_type, encoding = None):
	extra_args = {
		'ContentType': f'text/{file_type}',
	}

	if encoding:
		extra_args['ContentEncoding'] = encoding

	object = path.basename(file_path)

	print('Upload', object)

	s3.upload_file(
		file_path,
		bucket,
		object,
		ExtraArgs=extra_args
	)



publish_contract()
