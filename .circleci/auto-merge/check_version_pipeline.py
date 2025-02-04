from json import dumps, loads
from os import environ
from urllib.request import urlopen, Request


branch = environ['CIRCLE_BRANCH']

username = environ['CIRCLE_PROJECT_USERNAME']
reponame = environ['CIRCLE_PROJECT_REPONAME']


def get_json(url):
    response = urlopen(url)
    body = response.read()
    return loads(body)


def find_pipeline(page_token):
    pipelines_url = f'https://circleci.com/api/v2/project/github/{username}/{reponame}/pipeline?branch={branch}'

    if page_token:
        pipelines_url += f'&page-token={page_token}'

    pipelines_response = get_json(pipelines_url)
    next_page_token = pipelines_response['next_page_token']
    pipelines = pipelines_response['items']

    pipeline_id = None

    for pipeline in pipelines:
        id = pipeline['id']
        state = pipeline['state']

        if pipeline['trigger']['type'] != 'scheduled_pipeline':
            if state == 'created' or state == 'success':
                pipeline_id = id

                workflow = find_workflow(pipeline_id)

                if workflow:
                    return True

        else:
            print(f'Ignoring {id} ({state})...')

    if next_page_token:
        return find_pipeline(next_page_token)
    else:
        print(f'No pipeline found for branch {branch}')
        return False


def find_workflow(pipeline_id):
    workflows_url = f'https://circleci.com/api/v2/pipeline/{pipeline_id}/workflow'
    workflows = get_json(workflows_url)['items']

    workflows = list(filter(
        lambda w: w['name'] == 'all',
        workflows
    ))

    if not workflows:
        print(f'No workflow "all" found for branch {branch} pipeline {pipeline_id}')
        return False

    workflow = workflows[0]

    status = workflow['status']

    print()
    print(f'Last "all" status for branch {branch}: {status}')

    if status == 'canceled':
        only_publish_canceled = check_jobs(workflow['id'])
        if not only_publish_canceled:
            return False

    elif status == 'running':
        print()
        print('Workflow still in progress')
        exit(1)

    elif status != 'success' and status != 'on_hold':
        print()
        print('Workflow failed')
        exit(1)

    print()
    print('Workflow succeeded, go ahead and merge anything into it!')
    return True


def check_jobs(workflow_id):
    jobs_url = f'https://circleci.com/api/v2/workflow/{workflow_id}/job'
    jobs_response = get_json(jobs_url)
    jobs = jobs_response['items']

    print()

    while jobs_response['next_page_token']:
        print('Wait for another job page...')
        page_token = jobs_response['next_page_token']
        next_jobs_url = f'{jobs_url}&page-token={page_token}'
        jobs_response = get_json(next_jobs_url)
        jobs += jobs_response['items']

    only_publish_canceled = True
    approvals = []

    for job in jobs:
        name = job['name']
        type = job['type']
        status = job['status']

        is_success = status == 'success'

        is_approval = type == 'approval'

        requires = [id for id in job['requires']]
        is_approval_dependent = len(requires) > 0
        for require in requires:
            if require not in approvals:
                is_approval_dependent = False
                break

        only_publish_canceled = (
            only_publish_canceled
                and (is_success or is_approval or is_approval_dependent)
        )

        if is_approval:
            approvals.append(job['id'])

        print(f'{name} ({type}): {status} (keep going {only_publish_canceled})')

    return only_publish_canceled


pipeline = find_pipeline(None)

if not pipeline:
    exit(1)
