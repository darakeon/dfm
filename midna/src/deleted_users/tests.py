from django.test import TestCase

from deleted_users.models import Wipe
from utils.crypt import check

class WipeTests(TestCase):
	def test_wipe_encrypt_email(self):
		not_hashed_email = 'email@domain.com'

		wipe = Wipe(email=not_hashed_email)

		assert wipe.hashed_email == ''

		wipe.encrypt_email()

		assert check(not_hashed_email, wipe.hashed_email)
		assert wipe.email == None
