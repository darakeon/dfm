from json import dumps, loads
from os import environ
from re import search
from subprocess import run
from sys import argv


def main():
	with open(argv[1]) as prs_file:
		prs = loads(prs_file.read())

	prs = filter_prs(prs)

	pr = prs[-1] if prs else None

	if pr:
		number = pr['number']
		print(number)
		merge_state = pr['mergeStateStatus'] # BEHIND / DIRTY: recreate
		print(merge_state)

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
	branch = environ.get('TARGET_BRANCH')

	dependabot_pr_old = 'app/dependabot'
	dependabot_pr_new = 'dependabot'
	dependabot_commit = 'dependabot[bot]'
	owner_ci_commit = f'{username}-circleci'
	owner_commit = username
	allowed_to_commit = [dependabot_commit, owner_ci_commit, owner_commit]
	mandatory_workflow = 'ci/circleci: workflow_ran'

	return list(filter(
		lambda pr:
			pr['baseRefName'] == branch
			and (
				pr['author']['login'] == dependabot_pr_old
				or pr['author']['login'] == dependabot_pr_new
			)
			and pr['state'] == 'OPEN'
			and not pr['closed']
			and pr['mergeStateStatus'] != 'UNKNOWN'
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


main()
