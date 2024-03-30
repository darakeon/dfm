import re


class Migration:
	def __init__(self, name, code):
		self.name = name
		self.code = code

	def set_queries(self, queries):
		self.queries = queries

	def __str__(self):
		return self.name

	def __repr__(self):
		return f"\n{self.name}\n--- {self.queries[:15]}\n"

	def create(text):
		match = re.match(r"(\d+)\.(\d+)\.(\d+)\.(\d+)", text)

		if not match:
			return None

		name = match.group(0)

		dragon = int(match.group(1)) * 1000000000
		whale = int(match.group(2)) * 1000000
		sheep = int(match.group(3)) * 1000 
		ant = int(match.group(4))

		code = dragon + whale + sheep + ant

		return Migration(name, code)
