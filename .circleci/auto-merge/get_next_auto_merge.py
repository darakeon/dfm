from json import dumps, loads
from os import environ


branch = environ['CIRCLE_BRANCH']
repo_name = 'dfm'
repo_owner = 'darakeon'
dependabot_pr = 'app/dependabot'
dependabot_commit = 'dependabot[bot]'
dk_ci_commit = 'darakeon-circleci'
allowed_to_commit = [dependabot_commit, dk_ci_commit]
mandatory_workflow = 'ci/circleci: workflow_ran'

with open(f'prs-{branch}.json') as prs_file:
	prs = loads(prs_file.read())

prs = list(filter(
	lambda pr:
		pr['baseRefName'] == branch
		and pr['author']['is_bot']
		and pr['author']['login'] == dependabot_pr
		and pr['state'] == 'OPEN'
		and not pr['closed']
		and pr['headRefName'].startswith('dependabot/')
		and pr['headRepository']['name'] == repo_name
		and pr['headRepositoryOwner']['login'] == repo_owner
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
