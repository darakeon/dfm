from django.contrib import admin

from .models import Wipe


@admin.register(Wipe)
class WipeAdmin(admin.ModelAdmin):
	list_display = ['masked_email', 'when', 'why']

	def has_add_permission(self, request, obj=None):
		return False

	def has_change_permission(self, request, obj=None):
		return False

	def has_delete_permission(self, request, obj=None):
		return False
