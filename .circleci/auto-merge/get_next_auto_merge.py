from json import dumps, loads
from re import search
from subprocess import run
from sys import argv


def main():
	with open(argv[1]) as prs_file:
		prs = loads(prs_file.read())

	prs = filter_prs(prs)

	for pr in prs:
		id = pr['id']
		number = pr['number']
		ask_auto_merge = not pr['autoMergeRequest']
		print(number)

	'''
	commits: dependabot / darakeon-ci
	check auto merge
		enable auto merge
	check linear
		comment dependabot rebase
	'''


def filter_prs(prs):
	git_info = get_git_info()
	username = git_info[0]
	reponame = git_info[1]

	version = get_version()

	dependabot_pr = 'app/dependabot'
	dependabot_commit = 'dependabot[bot]'
	owner_ci_commit = f'{username}-circleci'
	owner_commit = username
	allowed_to_commit = [dependabot_commit, owner_ci_commit, owner_commit]
	mandatory_workflow = 'ci/circleci: workflow_ran'

	return list(filter(
		lambda pr:
			pr['baseRefName'] == version
			and pr['author']['is_bot']
			and pr['author']['login'] == dependabot_pr
			and pr['state'] == 'OPEN'
			and not pr['closed']
			and pr['headRefName'].startswith('dependabot/')
			and pr['headRepository']['name'] == reponame
			and pr['headRepositoryOwner']['login'] == username
			and not pr['isDraft']
			and not pr['isCrossRepository']
			and len(list(filter(
				lambda label: label['name'] == 'dependencies'
				, pr['labels']
			)))
			and len(list(filter(
				lambda commit: len(list(filter(
					lambda author: author['login'] not in allowed_to_commit
					, commit['authors']
				))) != 0
				, pr['commits']
			))) == 0
			and len(list(filter(
				lambda status_check: status_check.get('context') == mandatory_workflow
					and status_check['state'] == 'SUCCESS',
				pr['statusCheckRollup']
			))) > 0
		, prs
	))


def get_git_info():
	git_info = run(
		['git', 'remote', 'get-url', 'origin'],
		capture_output=True,
		text=True
	).stdout

	return search(
		'git@github.com:(.+)/(.+).git',
		git_info
	).groups()


def get_version():
	pattern = 'version in development.+#(\d+\.\d+\.\d+\.\d+)'

	with open('docs/RELEASES.md') as file:
		while file.readable():
			line = file.readline()
			result = search(pattern, line)

			if result:
				return result.group(1)

	return None


main()
