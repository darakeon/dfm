from os import listdir, path
from regex import match


test_dir_names = filter(
	lambda name: match('[A-Z]\.\w+', name),
	listdir('.')
)

dir_letter = 'A'

for d, test_dir_name in enumerate(test_dir_names):
	dir_letter = chr(ord('A') + d)

	if test_dir_name[0] != dir_letter:
		print(f'Wrong dir name [{dir_letter}]: {test_dir_name}')
		continue

	test_file_names = filter(
		lambda name: match('[a-z]\.\w+\.feature$', name),
		listdir(test_dir_name)
	)

	file_letter = 'a'

	for f, test_file_name in enumerate(test_file_names):
		file_letter = chr(ord('a') + f)

		if test_file_name[0] != file_letter:
			print(f'Wrong file name [{dir_letter}{file_letter}]: {test_file_name}')
			continue

		complete_path = path.join(test_dir_name, test_file_name)

		test_file = open(complete_path, encoding='utf-8-sig')
		lines = test_file.readlines()

		if not match(f'^Feature: {dir_letter}{file_letter}\.', lines[0]):
			print(f'Wrong first line [{dir_letter}{file_letter}]: {lines[0]}')
			lines[0] = lines[0][0:9] + dir_letter + file_letter + lines[0][11:]

		for l, line in enumerate(lines):
			lines[l] = lines[l].replace('\r', '')

			if not match(f'^Scenario:', line):
				continue

			if not match(f'^Scenario: {dir_letter}{file_letter}\d\d.', line):
				print(f'Wrong scenario line [{dir_letter}{file_letter}]: {lines[l]}')
				lines[l] = lines[l][0:10] + dir_letter + file_letter + lines[l][12:]
		#match()

		test_file.close()

		test_file = open(complete_path, 'w', encoding='utf-8-sig', newline='\n')
		test_file.writelines(lines)
