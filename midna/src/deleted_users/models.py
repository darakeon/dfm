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

	def encrypt_email(self):
		self.hashed_email = hash(self.email)
		self.email = None


	def __str__(self):
		return f"{self.username_start}...@{self.domain_start}..."

	class Meta:
		managed = False
