from os import environ

last_commit = environ.get("LAST_COMMIT")
if not last_commit:
	print("-")
	exit(1)



lines = last_commit.split("\n")
new_lines = []

for line in lines:
	if len(line) <= 72:
		new_lines.append(line)
		continue

	while len(line) > 0:
		limit = min(71, len(line))
		while len(line) > 72 and line[limit] != " " and line[limit] != "/":
			limit -= 1
		new_lines.append(line[:limit])
		line = line[limit:].lstrip()

if len(new_lines) == len(lines):
	exit(0)

for new_line in new_lines:
	print(new_line)
