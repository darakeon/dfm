import base64
from django.db import models

from utils.crypt import hash


NO_INTERACTION = 1
NOT_SIGNED_CONTRACT = 2
PERSON_ASKED = 3

REASON_CHOICES = (
	(NO_INTERACTION, "No Interaction"),
	(NOT_SIGNED_CONTRACT, "Not Signed Contract"),
	(PERSON_ASKED, "Person Asked"),
)


class Wipe(models.Model):
	hashed_email = models.CharField(max_length=60)
	username_start = models.CharField(max_length=2)
	domain_start = models.CharField(max_length=3)

	when = models.DateTimeField()
	why = models.IntegerField(choices=REASON_CHOICES)

	password = models.CharField(max_length=60)

	s3 = models.CharField(null=True, max_length=500)

	# remove
	email = models.EmailField(max_length=320, null=True)

	def masked_email(self):
		return f"{self.username_start}...@{self.domain_start}..."

	def encrypt_email(self):
		self.hashed_email = hash(self.email)
		self.email = None

	def hashed_email_base64(self):
		hashed_email_bytes = self.hashed_email.encode('utf8')
		base64_bytes = base64.b64encode(hashed_email_bytes)
		return base64_bytes.decode('utf8')

	def __str__(self):
		return self.masked_email()

	class Meta:
		managed = False
