from django.test import TestCase

from .models import Plan


class PlanTests(TestCase):

	def test_price_0(self):
		plan = Plan(price_cents=0)
		human = plan.price()
		assert human == "0.00"


	def test_price_1(self):
		plan = Plan(price_cents=1)
		human = plan.price()
		assert human == "0.01"


	def test_price_100(self):
		plan = Plan(price_cents=100)
		human = plan.price()
		assert human == "1.00"


	def test_size_b_min(self):
		plan = Plan(archive_size=1)
		human = plan.archive_sizish()
		assert human == "1B"


	def test_size_b_max(self):
		plan = Plan(archive_size=1023)
		human = plan.archive_sizish()
		assert human == "1023B"


	def test_size_kb_min(self):
		plan = Plan(archive_size=1024)
		human = plan.archive_sizish()
		assert human == "1KB"


	def test_size_kb_max(self):
		plan = Plan(archive_size=1024*1023)
		human = plan.archive_sizish()
		assert human == "1023KB"


	def test_size_mb_min(self):
		plan = Plan(archive_size=1024*1024)
		human = plan.archive_sizish()
		assert human == "1MB"


	def test_size_mb_max(self):
		plan = Plan(archive_size=1024*1024*1023)
		human = plan.archive_sizish()
		assert human == "1023MB"


	def test_size_gb_min(self):
		plan = Plan(archive_size=1024*1024*1024)
		human = plan.archive_sizish()
		assert human == "1GB"


	def test_size_gb_max(self):
		plan = Plan(archive_size=1024*1024*1024*1023)
		human = plan.archive_sizish()
		assert human == "1023GB"


	def test_size_tb_min(self):
		plan = Plan(archive_size=1024*1024*1024*1024)
		human = plan.archive_sizish()
		assert human == "1TB"


	def test_size_tb_max(self):
		plan = Plan(archive_size=1024*1024*1024*1024*1023)
		human = plan.archive_sizish()
		assert human == "1023TB"


	def test_size_big(self):
		plan = Plan(archive_size=1024*1024*1024*1024*1024)
		human = plan.archive_sizish()
		assert human == "1024TB"
