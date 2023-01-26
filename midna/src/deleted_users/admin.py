from django.contrib import admin

from .models import Wipe


class WipeAdmin(admin.ModelAdmin):
	list_display = ['email', 'when', 'why']
	search_fields = ['email']

	def has_add_permission(self, request, obj=None):
		return False

	def has_change_permission(self, request, obj=None):
		return False

	def has_delete_permission(self, request, obj=None):
		return False

admin.site.register(Wipe, WipeAdmin)
