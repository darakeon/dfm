from django.contrib.auth.decorators import login_required
from django.http import JsonResponse

from deleted_users.models import Wipe
from utils.s3 import S3


@login_required
def fix(_):
	wipes = Wipe.objects.exclude(email=None)

	response = []

	s3 = S3()

	for wipe in wipes[:3]:
		email = wipe.email

		wipe.encrypt_email()

		if wipe.s3 != None:
			hashed_email_base64 = wipe.hashed_email_base64()
			new_name = wipe.s3.replace(email, hashed_email_base64)
			s3.rename_file(wipe.s3, new_name)

		wipe.save()

		response.append({
			'hashed_email': wipe.hashed_email,
			'new_file': new_name,
		})

	return JsonResponse(response, safe=False)
