from django.db import models


FILE_LEVELS = ['B','KB','MB','GB','TB']

class Plan(models.Model):
	name = models.CharField(max_length=20)
	price_cents = models.IntegerField(db_column='pricecents')

	account_opened = models.IntegerField(db_column='accountopened')
	category_enabled = models.IntegerField(db_column='categoryenabled')
	schedule_active = models.IntegerField(db_column='scheduleactive')
	account_month_move = models.IntegerField(db_column='accountmonthmove')
	move_detail = models.IntegerField(db_column='movedetail')
	archive_month_upload = models.IntegerField(db_column='archivemonthupload')
	archive_line = models.IntegerField(db_column='archiveline')
	archive_size = models.IntegerField(db_column='archivesize')
	order_month = models.IntegerField(db_column='ordermonth')
	order_move = models.IntegerField(db_column='ordermove')


	def price(self):
		return f"{self.price_cents/100:.2f}"


	def archive_sizish(self):
		return self._format_size(self.archive_size, 0)
	
	def _format_size(self, current, level):
		next = current // 1024
		if next < 1 or level + 1 == len(FILE_LEVELS):
			return str(current) + FILE_LEVELS[level]
		
		return self._format_size(next, level + 1)


	def __str__(self):
		return self.name

	class Meta:
		managed = False
		db_table = 'plan'
