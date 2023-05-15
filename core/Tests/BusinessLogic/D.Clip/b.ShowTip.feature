Feature: Db. Show tip

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

Scenario: Db01. First time ask for tip
	When show a tip
	Then tip will not be shown

Scenario: Db02. Ask for tip when the age is enough
	Given asked for tip 4 times
	When show a tip
	Then tip will be shown

Scenario: Db03. Ask for tip should repeat tip some times
	Given asked for tip 4 times
	# first time
	When show a tip
	Then tip will be shown
	# second time
	When show a tip again
	Then tip shown will be equal to last one
	# third time
	When show a tip again
	Then tip shown will be equal to last one
	# forth time
	When show a tip again
	Then tip shown will be equal to last one
	# fifth time
	When show a tip
	Then tip will not be shown

Scenario: Db04. After first tip and counting again, show another tip
	Given asked for tip 4 times
	When show a tip
	Then tip will be shown
	Given dismissed tip
		And asked for tip 4 times
	When show a tip
	Then tip will be shown

Scenario: Db05. Ask for tips and see random ones
	Given asked for tip 4 times
	When show a tip
		And show a tip again
	Then tip will be shown
		And the tips will not be the first ones

Scenario: Db06. Dismiss tip show another after counter
	Given asked for tip 5 times
		And dismissed tip
		And asked for tip 4 times
	When show a tip
	Then tip will be shown

Scenario: Db07. Restart again after show all tips
	# first tip
	Given asked for tip 4 times
	When show a tip
	Then tip will be shown
	# second tip
	Given dismissed tip
		And asked for tip 4 times
	When show a tip
	Then tip will be shown
	# third tip
	Given dismissed tip
		And asked for tip 4 times
	When show a tip
	Then tip will be shown
	# there is not fourth tip in tests
	Given dismissed tip
		And asked for tip 4 times
	# not achieved the reset gap
	When show a tip
	Then tip will not be shown
	Given asked for tip 3 times
	# achieved the reset gap
	When show a tip
	Then tip will be shown

Scenario: Db08. Do not show used features
	Given asked for tip 4 times
		And disabled tip TestTip1
		And disabled tip TestTip2
		And disabled tip TestTip3
	When show a tip
	Then tip will not be shown

Scenario: Db09. Not show tip if user is marked for deletion
	Given the user is marked for deletion
	When show a tip
	Then I will receive this core error: UserDeleted
		And tip will not be shown

Scenario: Db10. Not show tip if user requested wipe
	Given the user asked data wipe
	When show a tip
	Then I will receive this core error: UserAskedWipe
		And tip will not be shown

Scenario: Db11. Not show tip if user logoff
	Given I logoff the user
	When show a tip
	Then I will receive this core error: Uninvited
		And tip will not be shown

Scenario: Db12. Tip return valid value
	Given asked for tip 4 times
	When show a tip
	Then tip will be one of
			| Tip      |
			| TestTip1 |
			| TestTip2 |
			| TestTip3 |

Scenario: Db13. Not show tip if user has not signed last contract
	Given there is a new contract
	When show a tip
	Then I will receive this core error: NotSignedLastContract
		And tip will not be shown
