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
    pipeline_url = f'https://circleci.com/api/v2/project/github/{username}/{reponame}/pipeline?branch={branch}'

    if page_token:
        pipeline_url += f'&page-token={page_token}'

    pipeline_response = get_json(pipeline_url)
    next_page_token = pipeline_response['next_page_token']
    pipelines = pipeline_response['items']

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
        find_pipeline(next_page_token)
    else:
        print(f'No pipeline found for branch {branch}')
        return False


def find_workflow(pipeline_id):
    workflow_url = f'https://circleci.com/api/v2/pipeline/{pipeline_id}/workflow'
    workflows = get_json(workflow_url)['items']

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
    print()

    if status == 'running':
        print('Workflow still in progress')
        exit(1)

    if status != 'success' and status != 'on_hold':
        print('Workflow failed')
        exit(1)

    print('Workflow succeeded, go ahead and merge anything into it!')
    return True


pipeline = find_pipeline(None)

if not pipeline:
    exit(1)
