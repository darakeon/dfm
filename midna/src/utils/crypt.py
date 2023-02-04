import bcrypt

def check(text: str, hashed_text: str):
	try:
		return bcrypt.checkpw(
			text.encode('utf8'),
			hashed_text.encode('utf8')
		)
	except ValueError as e:
		return False

def hash(text: str):
	return bcrypt.hashpw(
		text.encode('utf8'),
		bcrypt.gensalt()
	).decode('utf8')
