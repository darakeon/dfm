from django.test import TestCase

from deleted_users.models import Wipe
from utils.crypt import check

class WipeTests(TestCase):
	def test_encrypt_email(self):
		not_hashed_email = 'email@domain.com'

		wipe = Wipe(email=not_hashed_email)

		assert wipe.hashed_email == ''

		wipe.encrypt_email()

		assert check(not_hashed_email, wipe.hashed_email)
		assert wipe.email == None

	def test_hashed_email_base64(self):
		original = "$2b$12$ibiSaL7EiAFxPcPwDmx6vOZrnpEvUjEh95vzKCV.jj5445YlVCmJK"
		expected = "JDJiJDEyJGliaVNhTDdFaUFGeFBjUHdEbXg2dk9acm5wRXZVakVoOTV2ektDVi5qajU0NDVZbFZDbUpL"

		wipe = Wipe(hashed_email=original)

		base64 = wipe.hashed_email_base64()

		assert base64 == expected
