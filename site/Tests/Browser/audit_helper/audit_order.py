from json import loads, dumps

with open('audit-example.json') as file:
    content = loads(file.read())

vulnerabilities = content['vulnerabilities']

def check_effects(has: bool):
    return [
        dep
            for dep in vulnerabilities
                if bool(vulnerabilities[dep]['effects']) == has
    ]

while len(check_effects(True)):
    has_effects = check_effects(True)
    has_no_effects = check_effects(False)

    for lower_dep in has_no_effects:
        for main_dep in has_effects:
            if lower_dep in vulnerabilities[main_dep]['effects']:
                vulnerabilities[main_dep]['effects'].remove(lower_dep)
                
                if 'children' not in vulnerabilities[main_dep]:
                    vulnerabilities[main_dep]['children'] = {}

                vulnerabilities[main_dep]['children'][lower_dep] = vulnerabilities[lower_dep]

        del vulnerabilities[lower_dep]

with open('audit-result.json', 'w') as file:
    file.write(dumps(vulnerabilities, indent='\t'))

if [dep for dep in vulnerabilities] == ['micromatch']:
    print('All ok')
else:
    print(vulnerabilities)
