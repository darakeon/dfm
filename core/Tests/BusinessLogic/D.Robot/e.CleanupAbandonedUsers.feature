Feature: De. Cleanup Abandoned Users

Background:
	Given I have this user created
			| Email                           | Signed | Active |
			| {scenarioCode}@dontflymoney.com | true   | true   |

Scenario: De01. Find just active / signed term users
	When robot user login
		And call cleanup abandoned users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 0
		And and the user warning count will be 0

Scenario: De02. Find abandoned user for 15 days
	Given the user last access was 15 days before
	When robot user login
		And call cleanup abandoned users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 0
		And and the user warning count will be 0

Scenario: De03. Find abandoned user for 30 days
	Given the user last access was 30 days before
	When robot user login
		And call cleanup abandoned users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 1

Scenario: De04. Find abandoned user for 45 days not warned
	Given the user last access was 45 days before
	When robot user login
		And call cleanup abandoned users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 1

Scenario: De05. Find abandoned user for 45 days warned
	Given the user last access was 45 days before
		And the user have being warned once
	When robot user login
		And call cleanup abandoned users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 0
		And and the user warning count will be 1

Scenario: De06. Find abandoned user for 60 days not warned
	Given the user last access was 60 days before
	When robot user login
		And call cleanup abandoned users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 1

Scenario: De07. Find abandoned user for 60 days warned once
	Given the user last access was 60 days before
		And the user have being warned once
	When robot user login
		And call cleanup abandoned users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 2

Scenario: De08. Find abandoned user for 60 days warned twice
	Given the user last access was 60 days before
		And the user have being warned twice
	When robot user login
		And call cleanup abandoned users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 0
		And and the user warning count will be 2

Scenario: De09.	Find abandoned user for 75 days warned twice
	Given the user last access was 75 days before
		And the user have being warned twice
	When robot user login
		And call cleanup abandoned users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 0
		And and the user warning count will be 2

Scenario: De10. Find abandoned user for 90 days not warned
	Given the user last access was 90 days before
	When robot user login
		And call cleanup abandoned users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 1

Scenario: De11. Find abandoned user for 90 days warned once
	Given the user last access was 90 days before
		And the user have being warned once
	When robot user login
		And call cleanup abandoned users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 2

Scenario: De12. Find abandoned user for 90 days warned twice
	Given the user last access was 90 days before
		And the user have being warned twice
	When robot user login
		And call cleanup abandoned users
	Then I will receive no core error
		And the user won't exist
		And the count of warnings sent will be 0

Scenario: De13. Find abandoned user for 105 days not warned
	Given the user last access was 105 days before
	When robot user login
		And call cleanup abandoned users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 1

Scenario: De14. Find abandoned user for 105 days warned twice
	Given the user last access was 105 days before
		And the user have being warned twice
	When robot user login
		And call cleanup abandoned users
	Then I will receive no core error
		And the user won't exist
		And the count of warnings sent will be 0

Scenario: De15. Find user not signed contract for 15 days
	Given a contract from 15 days before
		And the user creation was 733 days before
		And the user last access was 0 days before
	When robot user login
		And call cleanup abandoned users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 0
		And and the user warning count will be 0

Scenario: De16. Find user not signed contract for 30 days
	Given a contract from 30 days before
		And the user creation was 733 days before
		And the user last access was 0 days before
	When robot user login
		And call cleanup abandoned users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 1

Scenario: De17. Find user not signed contract for 45 days not warned
	Given a contract from 45 days before
		And the user creation was 733 days before
		And the user last access was 0 days before
	When robot user login
		And call cleanup abandoned users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 1

Scenario: De18. Find user not signed contract for 45 days warned
	Given a contract from 45 days before
		And the user creation was 733 days before
		And the user last access was 0 days before
		And the user have being warned once
	When robot user login
		And call cleanup abandoned users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 0
		And and the user warning count will be 1

Scenario: De19. Find user not signed contract for 60 days not warned
	Given a contract from 60 days before
		And the user creation was 733 days before
		And the user last access was 0 days before
	When robot user login
		And call cleanup abandoned users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 1

Scenario: De20. Find user not signed contract for 60 days warned once
	Given a contract from 60 days before
		And the user creation was 733 days before
		And the user last access was 0 days before
		And the user have being warned once
	When robot user login
		And call cleanup abandoned users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 2

Scenario: De21. Find user not signed contract for 60 days warned twice
	Given a contract from 60 days before
		And the user creation was 733 days before
		And the user last access was 0 days before
		And the user have being warned twice
	When robot user login
		And call cleanup abandoned users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 0
		And and the user warning count will be 2

Scenario: De22. Find user not signed contract for 75 days warned twice
	Given a contract from 75 days before
		And the user creation was 733 days before
		And the user last access was 0 days before
		And the user have being warned twice
	When robot user login
		And call cleanup abandoned users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 0
		And and the user warning count will be 2

Scenario: De23. Find user not signed contract for 90 days not warned
	Given a contract from 90 days before
		And the user creation was 733 days before
		And the user last access was 0 days before
	When robot user login
		And call cleanup abandoned users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 1

Scenario: De24. Find user not signed contract for 90 days warned once
	Given a contract from 90 days before
		And the user creation was 733 days before
		And the user last access was 0 days before
		And the user have being warned once
	When robot user login
		And call cleanup abandoned users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 2

Scenario: De25. Find user not signed contract for 90 days warned twice
	Given a contract from 90 days before
		And the user creation was 733 days before
		And the user last access was 0 days before
		And the user have being warned twice
	When robot user login
		And call cleanup abandoned users
	Then I will receive no core error
		And the user won't exist
		And the count of warnings sent will be 0

Scenario: De26. Find user not signed contract for 105 days not warned
	Given a contract from 105 days before
		And the user creation was 733 days before
		And the user last access was 0 days before
	When robot user login
		And call cleanup abandoned users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 1

Scenario: De27. Find user not signed contract for 105 days warned twice
	Given a contract from 105 days before
		And the user creation was 733 days before
		And the user last access was 0 days before
		And the user have being warned twice
	When robot user login
		And call cleanup abandoned users
	Then I will receive no core error
		And the user won't exist
		And the count of warnings sent will be 0

Scenario: De28. Do not duplicate warnings (last accessed / contract)
	Given the user last access was 60 days before
		And the user creation was 733 days before
		And a contract from 30 days before
	When robot user login
		And call cleanup abandoned users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 1

Scenario: De29. Find never accessed user after 15 days
	Given the user creation was 15 days before
	When robot user login
		And call cleanup abandoned users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 0
		And and the user warning count will be 0

Scenario: De30. Find never accessed user after 30 days
	Given the user creation was 30 days before
	When robot user login
		And call cleanup abandoned users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 1

Scenario: De31. Find never accessed user after 45 days not warned
	Given the user creation was 45 days before
	When robot user login
		And call cleanup abandoned users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 1

Scenario: De32. Find never accessed user after 45 days warned
	Given the user creation was 45 days before
		And the user have being warned once
	When robot user login
		And call cleanup abandoned users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 0
		And and the user warning count will be 1

Scenario: De33. Find never accessed user after 60 days not warned
	Given the user creation was 60 days before
	When robot user login
		And call cleanup abandoned users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 1

Scenario: De34. Find never accessed user after 60 days warned once
	Given the user creation was 60 days before
		And the user have being warned once
	When robot user login
		And call cleanup abandoned users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 2

Scenario: De35. Find never accessed user after 60 days warned twice
	Given the user creation was 60 days before
		And the user have being warned twice
	When robot user login
		And call cleanup abandoned users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 0
		And and the user warning count will be 2

Scenario: De36.	Find never accessed user after 75 days warned twice
	Given the user creation was 75 days before
		And the user have being warned twice
	When robot user login
		And call cleanup abandoned users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 0
		And and the user warning count will be 2

Scenario: De37. Find never accessed user after 90 days not warned
	Given the user creation was 90 days before
	When robot user login
		And call cleanup abandoned users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 1

Scenario: De38. Find never accessed user after 90 days warned once
	Given the user creation was 90 days before
		And the user have being warned once
	When robot user login
		And call cleanup abandoned users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 2

Scenario: De39. Find never accessed user after 90 days warned twice
	Given the user creation was 90 days before
		And the user have being warned twice
	When robot user login
		And call cleanup abandoned users
	Then I will receive no core error
		And the user won't exist
		And the count of warnings sent will be 0

Scenario: De40. Find never accessed user after 105 days not warned
	Given the user creation was 105 days before
	When robot user login
		And call cleanup abandoned users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 1

Scenario: De41. Find never accessed user after 105 days warned twice
	Given the user creation was 105 days before
		And the user have being warned twice
	When robot user login
		And call cleanup abandoned users
	Then I will receive no core error
		And the user won't exist
		And the count of warnings sent will be 0

Scenario: De42. Find new user but having contract of 30 days ago
	Given the user creation was 0 days before
		And a contract from 30 days before
	When robot user login
		And call cleanup abandoned users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 0
		And and the user warning count will be 0

Scenario: De43. Find new user but having contract of 60 days ago
	Given the user creation was 0 days before
		And a contract from 60 days before
	When robot user login
		And call cleanup abandoned users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 0
		And and the user warning count will be 0

Scenario: De44. Find new user but having contract of 90 days ago
	Given the user creation was 0 days before
		And a contract from 90 days before
	When robot user login
		# older users warns first
		And call cleanup abandoned users
		# older users warns second
		And call cleanup abandoned users
		# older users deletes
		And call cleanup abandoned users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 0
		And and the user warning count will be 0

Scenario: De45. Run cleanup with no user
	Given the user last access was 90 days before
	When call cleanup abandoned users
	Then I will receive this core error: Uninvited
		And the user will still exist
		And the count of warnings sent will be 0
		And and the user warning count will be 0

Scenario: De46. Run cleanup with common user
	Given the user last access was 90 days before
	When test user login
		And call cleanup abandoned users
	Then I will receive this core error: Uninvited
		And the user will still exist
		And the count of warnings sent will be 0
		And and the user warning count will be 0

Scenario: De47. Remove User with just accessory data
	Given the user last access was 578 days before
		And the user have being warned twice
		And the user have
			| System Stuff |
			| Ticket       |
			| Security     |
			| Acceptance   |
	When robot user login
		And call cleanup abandoned users
	Then I will receive no core error
		And the user won't exist
		And the count of warnings sent will be 0

Scenario: De48. Remove User with admin data
	Given the user last access was 578 days before
		And the user have being warned twice
		And the user have
			| System Stuff |
			| Account      |
			| Category     |
	When robot user login
		And call cleanup abandoned users
	Then I will receive no core error
		And the user won't exist
		And the count of warnings sent will be 0

Scenario: De49. Remove User with moves
	Given the user last access was 578 days before
		And the user have being warned twice
		And the user have
			| System Stuff |
			| Move         |
	When robot user login
		And call cleanup abandoned users
	Then I will receive no core error
		And the user won't exist
		And the count of warnings sent will be 0
		And there will be an export file with this content
			| Description | Date       | Category           | Nature   | In              | Out              | Value | Details                                                                                 |
			| Move de49   | 2021-05-07 | Category de49 move | Transfer | Account de49 in | Account de49 out | 0.27  |                                                                                         |
			| Move de49   | 2021-05-07 | Category de49 move | Transfer | Account de49 in | Account de49 out |       | Detail Move de49 1 (3x0.09) + Detail Move de49 2 (3x0.09) + Detail Move de49 3 (3x0.09) |

Scenario: De50. Remove User without moves but with schedules
	Given the user last access was 578 days before
		And the user have being warned twice
		And the user have
			| System Stuff |
			| Schedule     |
		And robot run the scheduler
	When robot user login
		And call cleanup abandoned users
	Then I will receive no core error
		And the user won't exist
		And the count of warnings sent will be 0
		And there will be an export file with this content
			| Description   | Date       | Category               | Nature   | In              | Out              | Value | Details                                                                                             |
			| Schedule de50 | 2021-03-27 | Category de50 schedule | Transfer | Account de50 in | Account de50 out |       | Detail Schedule de50 1 (3x0.09) + Detail Schedule de50 2 (3x0.09) + Detail Schedule de50 3 (3x0.09) |
			| Schedule de50 | 2021-04-07 | Category de50 schedule | Transfer | Account de50 in | Account de50 out | 0.27  |                                                                                                     |
			| Schedule de50 | 2021-04-27 | Category de50 schedule | Transfer | Account de50 in | Account de50 out |       | Detail Schedule de50 1 (3x0.09) + Detail Schedule de50 2 (3x0.09) + Detail Schedule de50 3 (3x0.09) |
			| Schedule de50 | 2021-05-07 | Category de50 schedule | Transfer | Account de50 in | Account de50 out | 0.27  |                                                                                                     |
			| Schedule de50 | 2021-05-27 | Category de50 schedule | Transfer | Account de50 in | Account de50 out |       | Detail Schedule de50 1 (3x0.09) + Detail Schedule de50 2 (3x0.09) + Detail Schedule de50 3 (3x0.09) |
			| Schedule de50 | 2021-06-07 | Category de50 schedule | Transfer | Account de50 in | Account de50 out | 0.27  |                                                                                                     |
			| Schedule de50 | 2021-06-27 | Category de50 schedule | Transfer | Account de50 in | Account de50 out |       | Detail Schedule de50 1 (3x0.09) + Detail Schedule de50 2 (3x0.09) + Detail Schedule de50 3 (3x0.09) |
			| Schedule de50 | 2021-07-07 | Category de50 schedule | Transfer | Account de50 in | Account de50 out | 0.27  |                                                                                                     |
			| Schedule de50 | 2021-07-27 | Category de50 schedule | Transfer | Account de50 in | Account de50 out |       | Detail Schedule de50 1 (3x0.09) + Detail Schedule de50 2 (3x0.09) + Detail Schedule de50 3 (3x0.09) |
			| Schedule de50 | 2021-08-07 | Category de50 schedule | Transfer | Account de50 in | Account de50 out | 0.27  |                                                                                                     |
			| Schedule de50 | 2021-08-27 | Category de50 schedule | Transfer | Account de50 in | Account de50 out |       | Detail Schedule de50 1 (3x0.09) + Detail Schedule de50 2 (3x0.09) + Detail Schedule de50 3 (3x0.09) |
			| Schedule de50 | 2021-09-07 | Category de50 schedule | Transfer | Account de50 in | Account de50 out | 0.27  |                                                                                                     |
			| Schedule de50 | 2021-09-27 | Category de50 schedule | Transfer | Account de50 in | Account de50 out |       | Detail Schedule de50 1 (3x0.09) + Detail Schedule de50 2 (3x0.09) + Detail Schedule de50 3 (3x0.09) |
			| Schedule de50 | 2021-10-07 | Category de50 schedule | Transfer | Account de50 in | Account de50 out | 0.27  |                                                                                                     |
			| Schedule de50 | 2021-10-27 | Category de50 schedule | Transfer | Account de50 in | Account de50 out |       | Detail Schedule de50 1 (3x0.09) + Detail Schedule de50 2 (3x0.09) + Detail Schedule de50 3 (3x0.09) |
			| Schedule de50 | 2021-11-07 | Category de50 schedule | Transfer | Account de50 in | Account de50 out | 0.27  |                                                                                                     |
			| Schedule de50 | 2021-11-27 | Category de50 schedule | Transfer | Account de50 in | Account de50 out |       | Detail Schedule de50 1 (3x0.09) + Detail Schedule de50 2 (3x0.09) + Detail Schedule de50 3 (3x0.09) |
			| Schedule de50 | 2021-12-07 | Category de50 schedule | Transfer | Account de50 in | Account de50 out | 0.27  |                                                                                                     |
			| Schedule de50 | 2021-12-27 | Category de50 schedule | Transfer | Account de50 in | Account de50 out |       | Detail Schedule de50 1 (3x0.09) + Detail Schedule de50 2 (3x0.09) + Detail Schedule de50 3 (3x0.09) |
			| Schedule de50 | 2022-01-07 | Category de50 schedule | Transfer | Account de50 in | Account de50 out | 0.27  |                                                                                                     |

Scenario: De51. Remove User with moves and schedules
	Given the user last access was 578 days before
		And the user have being warned twice
		And the user have
			| System Stuff |
			| Move         |
			| Schedule     |
		And robot run the scheduler
	When robot user login
		And call cleanup abandoned users
	Then I will receive no core error
		And the user won't exist
		And the count of warnings sent will be 0
		And there will be an export file with this content
			| Description   | Date       | Category               | Nature   | In              | Out              | Value | Details                                                                                             |
			| Schedule de51 | 2021-03-27 | Category de51 schedule | Transfer | Account de51 in | Account de51 out |       | Detail Schedule de51 1 (3x0.09) + Detail Schedule de51 2 (3x0.09) + Detail Schedule de51 3 (3x0.09) |
			| Schedule de51 | 2021-04-07 | Category de51 schedule | Transfer | Account de51 in | Account de51 out | 0.27  |                                                                                                     |
			| Schedule de51 | 2021-04-27 | Category de51 schedule | Transfer | Account de51 in | Account de51 out |       | Detail Schedule de51 1 (3x0.09) + Detail Schedule de51 2 (3x0.09) + Detail Schedule de51 3 (3x0.09) |
			| Move de51     | 2021-05-07 | Category de51 move     | Transfer | Account de51 in | Account de51 out | 0.27  |                                                                                                     |
			| Move de51     | 2021-05-07 | Category de51 move     | Transfer | Account de51 in | Account de51 out |       | Detail Move de51 1 (3x0.09) + Detail Move de51 2 (3x0.09) + Detail Move de51 3 (3x0.09)             |
			| Schedule de51 | 2021-05-07 | Category de51 schedule | Transfer | Account de51 in | Account de51 out | 0.27  |                                                                                                     |
			| Schedule de51 | 2021-05-27 | Category de51 schedule | Transfer | Account de51 in | Account de51 out |       | Detail Schedule de51 1 (3x0.09) + Detail Schedule de51 2 (3x0.09) + Detail Schedule de51 3 (3x0.09) |
			| Schedule de51 | 2021-06-07 | Category de51 schedule | Transfer | Account de51 in | Account de51 out | 0.27  |                                                                                                     |
			| Schedule de51 | 2021-06-27 | Category de51 schedule | Transfer | Account de51 in | Account de51 out |       | Detail Schedule de51 1 (3x0.09) + Detail Schedule de51 2 (3x0.09) + Detail Schedule de51 3 (3x0.09) |
			| Schedule de51 | 2021-07-07 | Category de51 schedule | Transfer | Account de51 in | Account de51 out | 0.27  |                                                                                                     |
			| Schedule de51 | 2021-07-27 | Category de51 schedule | Transfer | Account de51 in | Account de51 out |       | Detail Schedule de51 1 (3x0.09) + Detail Schedule de51 2 (3x0.09) + Detail Schedule de51 3 (3x0.09) |
			| Schedule de51 | 2021-08-07 | Category de51 schedule | Transfer | Account de51 in | Account de51 out | 0.27  |                                                                                                     |
			| Schedule de51 | 2021-08-27 | Category de51 schedule | Transfer | Account de51 in | Account de51 out |       | Detail Schedule de51 1 (3x0.09) + Detail Schedule de51 2 (3x0.09) + Detail Schedule de51 3 (3x0.09) |
			| Schedule de51 | 2021-09-07 | Category de51 schedule | Transfer | Account de51 in | Account de51 out | 0.27  |                                                                                                     |
			| Schedule de51 | 2021-09-27 | Category de51 schedule | Transfer | Account de51 in | Account de51 out |       | Detail Schedule de51 1 (3x0.09) + Detail Schedule de51 2 (3x0.09) + Detail Schedule de51 3 (3x0.09) |
			| Schedule de51 | 2021-10-07 | Category de51 schedule | Transfer | Account de51 in | Account de51 out | 0.27  |                                                                                                     |
			| Schedule de51 | 2021-10-27 | Category de51 schedule | Transfer | Account de51 in | Account de51 out |       | Detail Schedule de51 1 (3x0.09) + Detail Schedule de51 2 (3x0.09) + Detail Schedule de51 3 (3x0.09) |
			| Schedule de51 | 2021-11-07 | Category de51 schedule | Transfer | Account de51 in | Account de51 out | 0.27  |                                                                                                     |
			| Schedule de51 | 2021-11-27 | Category de51 schedule | Transfer | Account de51 in | Account de51 out |       | Detail Schedule de51 1 (3x0.09) + Detail Schedule de51 2 (3x0.09) + Detail Schedule de51 3 (3x0.09) |
			| Schedule de51 | 2021-12-07 | Category de51 schedule | Transfer | Account de51 in | Account de51 out | 0.27  |                                                                                                     |
			| Schedule de51 | 2021-12-27 | Category de51 schedule | Transfer | Account de51 in | Account de51 out |       | Detail Schedule de51 1 (3x0.09) + Detail Schedule de51 2 (3x0.09) + Detail Schedule de51 3 (3x0.09) |
			| Schedule de51 | 2022-01-07 | Category de51 schedule | Transfer | Account de51 in | Account de51 out | 0.27  |                                                                                                     |
