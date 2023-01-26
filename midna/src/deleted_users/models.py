from django.db import models
from django.contrib import admin

NO_INTERACTION = 1
NOT_SIGNED_CONTRACT = 2
PERSON_ASKED = 3

REASON_CHOICES = (
	(NO_INTERACTION, "No Interaction"),
	(NOT_SIGNED_CONTRACT, "Not Signed Contract"),
	(PERSON_ASKED, "Person Asked"),
)

class Wipe(models.Model):
	email = models.EmailField(max_length=320)
	when = models.DateTimeField()
	why = models.IntegerField(choices=REASON_CHOICES)
	s3 = models.CharField(null=True, max_length=500)
	password = models.CharField(max_length=60)
	tfa = models.CharField(null=True, max_length=500)

	@admin.display(description='Why')
	def why_text(self):
		reasons = [
			"Error",
			REASON_CHOICES[1],
			REASON_CHOICES[2],
			REASON_CHOICES[3]
		]

		if self.why > len(reasons):
			return reasons[0]

		return reasons[self.why]

	def __str__(self):
		return self.email

	class Meta:
		managed = False
