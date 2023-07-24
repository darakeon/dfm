from django.contrib.auth.decorators import login_required
from django.http import JsonResponse

from deleted_users.models import Wipe
from utils.s3 import S3


@login_required
def fix(_):
	wipes = Wipe.objects.filter(s3__contains='@')

	response = []

	s3 = S3()

	for wipe in wipes:
		email = wipe.email

		email = wipe.s3.split('_')[0]
		hashed_email_base64 = wipe.hashed_email_base64()
		new_name = wipe.s3.replace(email, hashed_email_base64)
		file_exists = s3.exists(new_name)

		if file_exists:
			wipe.s3 = new_name
			wipe.save()

		response.append({
			'email': email,
			'hashed_email': wipe.hashed_email,
			'new_file': new_name,
			'file_exists': file_exists
		})

	return JsonResponse(response, safe=False)
