# pip install google-api-python-client
from json import loads
from os import environ
from apiclient.discovery import build
from google.oauth2 import service_account


SERVICE_ACCOUNT = environ['ANDROID_SERVICE_ACCOUNT']
PACKAGE_NAME = environ['ANDROID_PACKAGE_NAME']
BUNDLE = environ['ANDROID_BUNDLE']
TRACK = environ['ANDROID_TRACK']
VERSION = environ['CIRCLE_BRANCH']


def main():
	en_us = get_translation(
		'System Updates (some only at website):',
		'en-us', VERSION
	)
	pt_br = get_translation(
		'Atualizações no Sistema (algumas apenas no site):',
		'pt-br', VERSION
	)

	edits = get_edits()
	edit_id = start_edit(edits)

	bundle = upload_bundle(edits, edit_id)

	track = update_track(edits, edit_id, bundle, en_us, pt_br)

	print(track)


def get_translation(title, lang, key):
	with open(f'../../core/Language/Language/Version/{lang}.json') as file:
		content = file.read()

	all_versions = loads(content)
	for_version = all_versions[key]

	for_version.insert(0, title)

	return '\n'.join(for_version)


def get_edits():
	credentials = service_account.Credentials.from_service_account_info(
		loads(SERVICE_ACCOUNT),
		scopes=['https://www.googleapis.com/auth/androidpublisher']
	)

	return build(
		'androidpublisher',
		'v3',
		credentials=credentials
	).edits()


def start_edit(edits):
	return edits.insert(
		body={},
		packageName=PACKAGE_NAME
	).execute()['id']


def upload_bundle(edits, edit_id):
	return edits.bundles().upload(
		editId=edit_id,
		packageName=PACKAGE_NAME,
		media_mime_type='application/octet-stream',
		media_body=BUNDLE,
	).execute()


def update_track(edits, edit_id, bundle, en_us, pt_br):
	return edits.tracks().update(
		editId=edit_id,
		track=TRACK,
		packageName=PACKAGE_NAME,
		body={
			'releases': [
				{
					'name': VERSION,
					'versionCodes': [bundle['versionCode']],
					"releaseNotes": [
						{
							"language": "en-US",
							"text": en_us,
						},
						{
							"language": "pt-BR",
							"text": pt_br,
						}
					],
					"status": "completed",
				}
			]
		}
	).execute()



if __name__ == '__main__':
	main()
