from datetime import datetime


class Log:
	RED = 31
	BLACK = 30
	PURPLE = 35

	def title_red(self, text):
		self.title_colored(text, self.RED)

	def title_black(self, text):
		self.title_colored(text, self.BLACK)

	def title_purple(self, text):
		self.title_colored(text, self.PURPLE)

	def title_colored(self, text, color):
		print(f"\033[01;{color}m")
		self.empty()
		self.text("---------------------------------------------------------------------------------")
		self.text(f"--- {text} {datetime.now()}")
		self.text("---------------------------------------------------------------------------------")
		self.empty()
		print("\033[00m")

	def empty(self):
		self.text("")

	def text(self, text):
		print(text)
