from django.contrib.auth.decorators import login_required
from django.http import JsonResponse

from deleted_users.models import Wipe


@login_required
def fix(_):
	wipes = Wipe.objects.exclude(email=None)

	response = []

	for wipe in wipes:
		wipe.encrypt_email()
		wipe.save()

		response.append({
			'hashed_email': wipe.hashed_email,
		})

	return JsonResponse(response, safe=False)
