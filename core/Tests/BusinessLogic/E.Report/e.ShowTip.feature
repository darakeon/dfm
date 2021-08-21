Feature: Ee. Show tip

# the number of calls taken to show a tip,
# the number of calls a tip is repeated and
# the number of calls to restart after all of them are seen
# are configured in the project's tips.json

# CURRENT VALUE         | prod | tests |
# accesses to see a tip |  27  |   5   |
# repeat same tip       |   3  |   4   |
# reset after finish    |  81  |   9   |

# tips are repeated to increase the chance that the person see it

# there are 3 test tips

Background:
	Given test user login

Scenario: Ee01. First time ask for tip
Scenario: Ee02. Ask for tip when the age is enough
Scenario: Ee03. Ask for tip should repeat tip some times
Scenario: Ee04. After first tip and counting again, show another tip
Scenario: Ee05. Ask for tips and see random ones
Scenario: Ee06. Dismiss tip show another after counter
Scenario: Ee07. Restart again after show all tips
Scenario: Ee08. Do not show used features
Scenario: Ee09. Not show tip if user is marked for deletion
Scenario: Ee10. Not show tip if user requested wipe
Scenario: Ee11. Not show tip if user logoff
Scenario: Ee12. Tip return valid value
