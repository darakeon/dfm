from django.contrib import admin

from .models import Plan


@admin.register(Plan)
class PlanAdmin(admin.ModelAdmin):
	list_display = [
		'name',
		'price',
		'account_opened',
		'category_enabled',
		'schedule_active',
		'account_month_move',
		'move_detail',
		'archive_month_upload',
		'archive_line',
		'archive_sizish',
		'order_month',
		'order_move',
	]

	def has_add_permission(self, request, obj=None):
		return True

	def has_change_permission(self, request, obj=None):
		return True

	def has_delete_permission(self, request, obj=None):
		return True
