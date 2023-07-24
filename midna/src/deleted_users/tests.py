from django.test import TestCase

from deleted_users.models import Wipe
from utils.crypt import check

class WipeTests(TestCase):
	def test_hashed_email_base64(self):
		original = "$2b$12$ibiSaL7EiAFxPcPwDmx6vOZrnpEvUjEh95vzKCV.jj5445YlVCmJK"
		expected = "JDJiJDEyJGliaVNhTDdFaUFGeFBjUHdEbXg2dk9acm5wRXZVakVoOTV2ektDVi5qajU0NDVZbFZDbUpL"

		wipe = Wipe(hashed_email=original)

		base64 = wipe.hashed_email_base64()

		assert base64 == expected
