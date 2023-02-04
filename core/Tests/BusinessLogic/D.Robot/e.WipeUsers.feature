Feature: De. Wipe Users

Background:
	Given I have this user created
			| Email                           | Signed | Active |
			| {scenarioCode}@dontflymoney.com | true   | true   |

Scenario: De01. Find just active / signed term users
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 0
		And and the user warning count will be 0
		And it will not be registered at wipe table

Scenario: De02. Find abandoned user for 15 days
	Given the user last access was 15 days before
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 0
		And and the user warning count will be 0
		And it will not be registered at wipe table

Scenario: De03. Find abandoned user for 30 days
	Given the user last access was 30 days before
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 1
		And it will not be registered at wipe table

Scenario: De04. Find abandoned user for 45 days not warned
	Given the user last access was 45 days before
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 1
		And it will not be registered at wipe table

Scenario: De05. Find abandoned user for 45 days warned
	Given the user last access was 45 days before
		And the user have being warned once
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 0
		And and the user warning count will be 1
		And it will not be registered at wipe table

Scenario: De06. Find abandoned user for 60 days not warned
	Given the user last access was 60 days before
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 1
		And it will not be registered at wipe table

Scenario: De07. Find abandoned user for 60 days warned once
	Given the user last access was 60 days before
		And the user have being warned once
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 2
		And it will not be registered at wipe table

Scenario: De08. Find abandoned user for 60 days warned twice
	Given the user last access was 60 days before
		And the user have being warned twice
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 0
		And and the user warning count will be 2
		And it will not be registered at wipe table

Scenario: De09.	Find abandoned user for 75 days warned twice
	Given the user last access was 75 days before
		And the user have being warned twice
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 0
		And and the user warning count will be 2
		And it will not be registered at wipe table

Scenario: De10. Find abandoned user for 90 days not warned
	Given the user last access was 90 days before
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 1
		And it will not be registered at wipe table

Scenario: De11. Find abandoned user for 90 days warned once
	Given the user last access was 90 days before
		And the user have being warned once
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 2
		And it will not be registered at wipe table

Scenario: De12. Find abandoned user for 90 days warned twice
	Given the user last access was 90 days before
		And the user have being warned twice
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user won't exist
		And the count of warnings sent will be 0
		And it will be registered at wipe table with reason NoInteraction

Scenario: De13. Find abandoned user for 105 days not warned
	Given the user last access was 105 days before
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 1
		And it will not be registered at wipe table

Scenario: De14. Find abandoned user for 105 days warned twice
	Given the user last access was 105 days before
		And the user have being warned twice
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user won't exist
		And the count of warnings sent will be 0
		And it will be registered at wipe table with reason NoInteraction

Scenario: De15. Find user not signed contract for 15 days
	Given a contract from 15 days before
		And the user creation was 733 days before
		And the user last access was 0 days before
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 0
		And and the user warning count will be 0
		And it will not be registered at wipe table

Scenario: De16. Find user not signed contract for 30 days
	Given a contract from 30 days before
		And the user creation was 733 days before
		And the user last access was 0 days before
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 1
		And it will not be registered at wipe table

Scenario: De17. Find user not signed contract for 45 days not warned
	Given a contract from 45 days before
		And the user creation was 733 days before
		And the user last access was 0 days before
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 1
		And it will not be registered at wipe table

Scenario: De18. Find user not signed contract for 45 days warned
	Given a contract from 45 days before
		And the user creation was 733 days before
		And the user last access was 0 days before
		And the user have being warned once
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 0
		And and the user warning count will be 1
		And it will not be registered at wipe table

Scenario: De19. Find user not signed contract for 60 days not warned
	Given a contract from 60 days before
		And the user creation was 733 days before
		And the user last access was 0 days before
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 1
		And it will not be registered at wipe table

Scenario: De20. Find user not signed contract for 60 days warned once
	Given a contract from 60 days before
		And the user creation was 733 days before
		And the user last access was 0 days before
		And the user have being warned once
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 2
		And it will not be registered at wipe table

Scenario: De21. Find user not signed contract for 60 days warned twice
	Given a contract from 60 days before
		And the user creation was 733 days before
		And the user last access was 0 days before
		And the user have being warned twice
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 0
		And and the user warning count will be 2
		And it will not be registered at wipe table

Scenario: De22. Find user not signed contract for 75 days warned twice
	Given a contract from 75 days before
		And the user creation was 733 days before
		And the user last access was 0 days before
		And the user have being warned twice
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 0
		And and the user warning count will be 2
		And it will not be registered at wipe table

Scenario: De23. Find user not signed contract for 90 days not warned
	Given a contract from 90 days before
		And the user creation was 733 days before
		And the user last access was 0 days before
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 1
		And it will not be registered at wipe table

Scenario: De24. Find user not signed contract for 90 days warned once
	Given a contract from 90 days before
		And the user creation was 733 days before
		And the user last access was 0 days before
		And the user have being warned once
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 2
		And it will not be registered at wipe table

Scenario: De25. Find user not signed contract for 90 days warned twice
	Given a contract from 90 days before
		And the user creation was 733 days before
		And the user last access was 0 days before
		And the user have being warned twice
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user won't exist
		And the count of warnings sent will be 0
		And it will be registered at wipe table with reason NotSignedContract

Scenario: De26. Find user not signed contract for 105 days not warned
	Given a contract from 105 days before
		And the user creation was 733 days before
		And the user last access was 0 days before
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 1
		And it will not be registered at wipe table

Scenario: De27. Find user not signed contract for 105 days warned twice
	Given a contract from 105 days before
		And the user creation was 733 days before
		And the user last access was 0 days before
		And the user have being warned twice
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user won't exist
		And the count of warnings sent will be 0
		And it will be registered at wipe table with reason NotSignedContract

Scenario: De28. Do not duplicate warnings (last accessed / contract)
	Given the user last access was 60 days before
		And the user creation was 733 days before
		And a contract from 30 days before
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 1
		And it will not be registered at wipe table

Scenario: De29. Find never accessed user after 15 days
	Given the user creation was 15 days before
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 0
		And and the user warning count will be 0
		And it will not be registered at wipe table

Scenario: De30. Find never accessed user after 30 days
	Given the user creation was 30 days before
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 1
		And it will not be registered at wipe table

Scenario: De31. Find never accessed user after 45 days not warned
	Given the user creation was 45 days before
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 1
		And it will not be registered at wipe table

Scenario: De32. Find never accessed user after 45 days warned
	Given the user creation was 45 days before
		And the user have being warned once
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 0
		And and the user warning count will be 1
		And it will not be registered at wipe table

Scenario: De33. Find never accessed user after 60 days not warned
	Given the user creation was 60 days before
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 1
		And it will not be registered at wipe table

Scenario: De34. Find never accessed user after 60 days warned once
	Given the user creation was 60 days before
		And the user have being warned once
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 2
		And it will not be registered at wipe table

Scenario: De35. Find never accessed user after 60 days warned twice
	Given the user creation was 60 days before
		And the user have being warned twice
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 0
		And and the user warning count will be 2
		And it will not be registered at wipe table

Scenario: De36.	Find never accessed user after 75 days warned twice
	Given the user creation was 75 days before
		And the user have being warned twice
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 0
		And and the user warning count will be 2
		And it will not be registered at wipe table

Scenario: De37. Find never accessed user after 90 days not warned
	Given the user creation was 90 days before
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 1
		And it will not be registered at wipe table

Scenario: De38. Find never accessed user after 90 days warned once
	Given the user creation was 90 days before
		And the user have being warned once
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 2
		And it will not be registered at wipe table

Scenario: De39. Find never accessed user after 90 days warned twice
	Given the user creation was 90 days before
		And the user have being warned twice
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user won't exist
		And the count of warnings sent will be 0
		And it will be registered at wipe table with reason NoInteraction

Scenario: De40. Find never accessed user after 105 days not warned
	Given the user creation was 105 days before
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 1
		And it will not be registered at wipe table

Scenario: De41. Find never accessed user after 105 days warned twice
	Given the user creation was 105 days before
		And the user have being warned twice
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user won't exist
		And the count of warnings sent will be 0
		And it will be registered at wipe table with reason NoInteraction

Scenario: De42. Find new user but having contract of 30 days ago
	Given the user creation was 0 days before
		And a contract from 30 days before
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 0
		And and the user warning count will be 0
		And it will not be registered at wipe table

Scenario: De43. Find new user but having contract of 60 days ago
	Given the user creation was 0 days before
		And a contract from 60 days before
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 0
		And and the user warning count will be 0
		And it will not be registered at wipe table

Scenario: De44. Find new user but having contract of 90 days ago
	Given the user creation was 0 days before
		And a contract from 90 days before
	When robot user login
		# older users warns first
		And call wipe users
		# older users warns second
		And call wipe users
		# older users deletes
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 0
		And and the user warning count will be 0
		And it will not be registered at wipe table

Scenario: De45. Run cleanup with no user
	Given the user last access was 90 days before
	When call wipe users
	Then I will receive this core error: Uninvited
		And the user will still exist
		And the count of warnings sent will be 0
		And and the user warning count will be 0
		And it will not be registered at wipe table

Scenario: De46. Run cleanup with common user
	Given the user last access was 90 days before
	When test user login
		And call wipe users
	Then I will receive this core error: Uninvited
		And the user will still exist
		And the count of warnings sent will be 0
		And and the user warning count will be 0
		And it will not be registered at wipe table

Scenario: De47. Remove User with just accessory data
	Given the user last access was 578 days before
		And the user have being warned twice
		And the user have
			| System Stuff |
			| Ticket       |
			| Security     |
			| Acceptance   |
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user won't exist
		And the count of warnings sent will be 0
		And it will be registered at wipe table with reason NoInteraction

Scenario: De48. Remove User with admin data
	Given the user last access was 578 days before
		And the user have being warned twice
		And the user have
			| System Stuff |
			| Account      |
			| Category     |
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user won't exist
		And the count of warnings sent will be 0
		And it will be registered at wipe table with reason NoInteraction

Scenario: De49. Remove User with moves and schedules
	Given the user last access was 578 days before
		And the user have being warned twice
		And the user have
			| System Stuff |
			| Move         |
			| Schedule     |
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user won't exist
		And the count of warnings sent will be 0
		And there will be an export file with this content
			| Description   | Date       | Category               | Nature   | In              | Out              | Value | Details                                                   |
			| Move de49     | 2021-05-07 | Category de49 move     | Transfer | Account de49 in | Account de49 out |       | Detail 1 (3x0.09) + Detail 2 (3x0.09) + Detail 3 (3x0.09) |
			| Schedule de49 | 3000-03-27 | Category de49 schedule | Transfer | Account de49 in | Account de49 out | 0.27  |                                                           |
			| Schedule de49 | 3000-04-27 | Category de49 schedule | Transfer | Account de49 in | Account de49 out | 0.27  |                                                           |
			| Schedule de49 | 3000-05-27 | Category de49 schedule | Transfer | Account de49 in | Account de49 out | 0.27  |                                                           |
			| Schedule de49 | 3000-06-27 | Category de49 schedule | Transfer | Account de49 in | Account de49 out | 0.27  |                                                           |
			| Schedule de49 | 3000-07-27 | Category de49 schedule | Transfer | Account de49 in | Account de49 out | 0.27  |                                                           |
			| Schedule de49 | 3000-08-27 | Category de49 schedule | Transfer | Account de49 in | Account de49 out | 0.27  |                                                           |
			| Schedule de49 | 3000-09-27 | Category de49 schedule | Transfer | Account de49 in | Account de49 out | 0.27  |                                                           |
			| Schedule de49 | 3000-10-27 | Category de49 schedule | Transfer | Account de49 in | Account de49 out | 0.27  |                                                           |
			| Schedule de49 | 3000-11-27 | Category de49 schedule | Transfer | Account de49 in | Account de49 out | 0.27  |                                                           |
			| Schedule de49 | 3000-12-27 | Category de49 schedule | Transfer | Account de49 in | Account de49 out | 0.27  |                                                           |
		And it will be registered at wipe table with reason NoInteraction

Scenario: De50. Send e-mail using user language
	Given the user last access was 748 days before
		And user language is pt-BR
	When robot user login
		And user robot language is en-US
		And call wipe users
	Then I will receive no core error
		And the e-mail subject will be "Parece que você não está mais usando o sistema"
		And the e-mail body will contain "estão para ser excluídos"

Scenario: De51. Check days counting
	Given the user last access was 63 days before
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the e-mail body will contain "27</h1>dias"

Scenario: De52. Check wipe notice
	Given the user last access was 105 days before
		And the user have being warned twice
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user won't exist
		And the count of warnings sent will be 0
		And there will be a wipe notice sent
		And it will be registered at wipe table with reason NoInteraction

Scenario: De53. Wipe when user asks too
	Given the user have
			| System Stuff |
			| Move         |
			| Schedule     |
		And data wipe was asked
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user won't exist
		And there will be a wipe notice sent
		And it will be registered at wipe table with reason PersonAsked
		And there will no be an export file

Scenario: De54. Wipe just the right user
	Given the user have
			| System Stuff |
			| Move         |
			| Schedule     |
		And data wipe was asked
		And I have this user created
			| Email                         | Signed | Active |
			| dont_wipe_me@dontflymoney.com | true   | true   |
		And the user dont_wipe_me@dontflymoney.com have
			| System Stuff |
			| Move         |
			| Schedule     |
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user won't exist
		But the user dont_wipe_me@dontflymoney.com will still exist

Scenario: De55. Do not warn robots without activity
	Given the user last access was 90 days before
		But the user is a robot
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 0
		And and the user warning count will be 0

Scenario: De56. Do not wipe robots without activity
	Given the user last access was 90 days before
		And the user have being warned twice
		But the user is a robot
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And it will not be registered at wipe table

Scenario: De57. Do not warn robots which did not accepted last contract
	Given a contract from 90 days before
		And the user creation was 100 days before
		And the user last access was 0 days before
		But the user is a robot
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 0
		And and the user warning count will be 0

Scenario: De58. Do not wipe robots which did not accepted last contract
	Given a contract from 90 days before
		And the user creation was 100 days before
		And the user last access was 0 days before
		And the user have being warned twice
		But the user is a robot
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And it will not be registered at wipe table

# it is a robot, should not "ask" this
# need to be protected, just in case
Scenario: De59. Do not wipe robots even if it "asks" too
	Given data wipe was asked
		But the user is a robot
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And it will not be registered at wipe table
