from django.contrib.auth.decorators import login_required
from django.http import JsonResponse
from boto3 import client
from os import environ

from deleted_users.models import Wipe


@login_required
def fix(_):
	wipes = Wipe.objects.exclude(email=None)

	response = []

	s3 = client('s3')
	bucket = environ.get('AWS_BUCKET')

	for wipe in wipes:
		email = wipe.email

		wipe.encrypt_email()

		if wipe.s3 != None:
			import base64
			hashed_email_bytes = wipe.hashed_email.encode('utf8')
			base64_bytes = base64.b64encode(hashed_email_bytes)
			hashed_email_base64 = base64_bytes.decode('utf8')
			new_name = wipe.s3.replace(email, hashed_email_base64)
			
			copy_source = {'Bucket': bucket, 'Key': wipe.s3}

			s3.copy_object(
				Bucket = bucket,
				CopySource = copy_source,
				Key = new_name
			)

			s3.delete_object(Bucket = bucket, Key = wipe.s3)

		wipe.save()

		response.append({
			'hashed_email': wipe.hashed_email,
			'new_file': new_name,
		})

	return JsonResponse(response, safe=False)
