﻿Feature: Ib. Wipe Users

Background:
	Given I have this user created
			| Email                           | Signed | Active | Theme     | Language |
			| {scenarioCode}@dontflymoney.com | true   | true   | DarkMagic | en-US    |

Scenario: Ib01. Find just active / signed term users
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 0
		And and the user warning count will be 0
		And it will not be registered at wipe table

Scenario: Ib02. Find abandoned user for 15 days
	Given the user last access was 15 days before
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 0
		And and the user warning count will be 0
		And it will not be registered at wipe table

Scenario: Ib03. Find abandoned user for 30 days
	Given the user last access was 30 days before
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 1
		And it will not be registered at wipe table

Scenario: Ib04. Find abandoned user for 45 days not warned
	Given the user last access was 45 days before
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 1
		And it will not be registered at wipe table

Scenario: Ib05. Find abandoned user for 45 days warned
	Given the user last access was 45 days before
		And the user have being warned once
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 0
		And and the user warning count will be 1
		And it will not be registered at wipe table

Scenario: Ib06. Find abandoned user for 60 days not warned
	Given the user last access was 60 days before
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 1
		And it will not be registered at wipe table

Scenario: Ib07. Find abandoned user for 60 days warned once
	Given the user last access was 60 days before
		And the user have being warned once
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 2
		And it will not be registered at wipe table

Scenario: Ib08. Find abandoned user for 60 days warned twice
	Given the user last access was 60 days before
		And the user have being warned twice
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 0
		And and the user warning count will be 2
		And it will not be registered at wipe table

Scenario: Ib09. Find abandoned user for 75 days warned twice
	Given the user last access was 75 days before
		And the user have being warned twice
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 0
		And and the user warning count will be 2
		And it will not be registered at wipe table

Scenario: Ib10. Find abandoned user for 90 days not warned
	Given the user last access was 90 days before
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 1
		And it will not be registered at wipe table

# 5 sec
Scenario: Ib11. Find abandoned user for 90 days warned once
	Given the user last access was 90 days before
		And the user have being warned once
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 2
		And it will not be registered at wipe table

# 0.593 sec
Scenario: Ib12. Find abandoned user for 90 days warned twice
	Given the user last access was 90 days before
		And the user have being warned twice
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user won't exist
		And the count of warnings sent will be 0
		And it will be registered at wipe table
			| Reason        | CSV file | Theme     | Language |
			| NoInteraction | No       | DarkMagic | en-US    |

Scenario: Ib13. Find abandoned user for 105 days not warned
	Given the user last access was 105 days before
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 1
		And it will not be registered at wipe table

Scenario: Ib14. Find abandoned user for 105 days warned twice
	Given the user last access was 105 days before
		And the user have being warned twice
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user won't exist
		And the count of warnings sent will be 0
		And it will be registered at wipe table
			| Reason        | CSV file | Theme     | Language |
			| NoInteraction | No       | DarkMagic | en-US    |

Scenario: Ib15. Find user not signed contract for 15 days
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

Scenario: Ib16. Find user not signed contract for 30 days
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

Scenario: Ib17. Find user not signed contract for 45 days not warned
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

Scenario: Ib18. Find user not signed contract for 45 days warned
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

Scenario: Ib19. Find user not signed contract for 60 days not warned
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

Scenario: Ib20. Find user not signed contract for 60 days warned once
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

Scenario: Ib21. Find user not signed contract for 60 days warned twice
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

Scenario: Ib22. Find user not signed contract for 75 days warned twice
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

Scenario: Ib23. Find user not signed contract for 90 days not warned
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

Scenario: Ib24. Find user not signed contract for 90 days warned once
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

Scenario: Ib25. Find user not signed contract for 90 days warned twice
	Given a contract from 90 days before
		And the user creation was 733 days before
		And the user last access was 0 days before
		And the user have being warned twice
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user won't exist
		And the count of warnings sent will be 0
		And it will be registered at wipe table
			| Reason            | CSV file | Theme     | Language |
			| NotSignedContract | No       | DarkMagic | en-US    |

Scenario: Ib26. Find user not signed contract for 105 days not warned
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

Scenario: Ib27. Find user not signed contract for 105 days warned twice
	Given a contract from 105 days before
		And the user creation was 733 days before
		And the user last access was 0 days before
		And the user have being warned twice
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user won't exist
		And the count of warnings sent will be 0
		And it will be registered at wipe table
			| Reason            | CSV file | Theme     | Language |
			| NotSignedContract | No       | DarkMagic | en-US    |

Scenario: Ib28. Do not duplicate warnings (last accessed / contract)
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

Scenario: Ib29. Find never accessed user after 15 days
	Given the user creation was 15 days before
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 0
		And and the user warning count will be 0
		And it will not be registered at wipe table

Scenario: Ib30. Find never accessed user after 30 days
	Given the user creation was 30 days before
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 1
		And it will not be registered at wipe table

Scenario: Ib31. Find never accessed user after 45 days not warned
	Given the user creation was 45 days before
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 1
		And it will not be registered at wipe table

Scenario: Ib32. Find never accessed user after 45 days warned
	Given the user creation was 45 days before
		And the user have being warned once
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 0
		And and the user warning count will be 1
		And it will not be registered at wipe table

Scenario: Ib33. Find never accessed user after 60 days not warned
	Given the user creation was 60 days before
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 1
		And it will not be registered at wipe table

Scenario: Ib34. Find never accessed user after 60 days warned once
	Given the user creation was 60 days before
		And the user have being warned once
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 2
		And it will not be registered at wipe table

Scenario: Ib35. Find never accessed user after 60 days warned twice
	Given the user creation was 60 days before
		And the user have being warned twice
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 0
		And and the user warning count will be 2
		And it will not be registered at wipe table

Scenario: Ib36. Find never accessed user after 75 days warned twice
	Given the user creation was 75 days before
		And the user have being warned twice
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 0
		And and the user warning count will be 2
		And it will not be registered at wipe table

Scenario: Ib37. Find never accessed user after 90 days not warned
	Given the user creation was 90 days before
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 1
		And it will not be registered at wipe table

Scenario: Ib38. Find never accessed user after 90 days warned once
	Given the user creation was 90 days before
		And the user have being warned once
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 2
		And it will not be registered at wipe table

Scenario: Ib39. Find never accessed user after 90 days warned twice
	Given the user creation was 90 days before
		And the user have being warned twice
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user won't exist
		And the count of warnings sent will be 0
		And it will be registered at wipe table
			| Reason        | CSV file | Theme     | Language |
			| NoInteraction | No       | DarkMagic | en-US    |

Scenario: Ib40. Find never accessed user after 105 days not warned
	Given the user creation was 105 days before
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 1
		And and the user warning count will be 1
		And it will not be registered at wipe table

Scenario: Ib41. Find never accessed user after 105 days warned twice
	Given the user creation was 105 days before
		And the user have being warned twice
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user won't exist
		And the count of warnings sent will be 0
		And it will be registered at wipe table
			| Reason        | CSV file | Theme     | Language |
			| NoInteraction | No       | DarkMagic | en-US    |

Scenario: Ib42. Find new user but having contract of 30 days ago
	Given the user creation was 0 days before
		And a contract from 30 days before
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 0
		And and the user warning count will be 0
		And it will not be registered at wipe table

Scenario: Ib43. Find new user but having contract of 60 days ago
	Given the user creation was 0 days before
		And a contract from 60 days before
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 0
		And and the user warning count will be 0
		And it will not be registered at wipe table

Scenario: Ib44. Find new user but having contract of 90 days ago
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

Scenario: Ib45. Run cleanup with no user
	Given the user last access was 90 days before
	When call wipe users
	Then I will receive this core error: Uninvited
		And the user will still exist
		And the count of warnings sent will be 0
		And and the user warning count will be 0
		And it will not be registered at wipe table

Scenario: Ib46. Run cleanup with common user
	Given the user last access was 90 days before
	When test user login
		And call wipe users
	Then I will receive this core error: Uninvited
		And the user will still exist
		And the count of warnings sent will be 0
		And and the user warning count will be 0
		And it will not be registered at wipe table

Scenario: Ib47. Remove User with just accessory data
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
		And it will be registered at wipe table
			| Reason        | CSV file | Theme     | Language |
			| NoInteraction | No       | DarkMagic | en-US    |

Scenario: Ib48. Remove User with admin data
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
		And it will be registered at wipe table
			| Reason        | CSV file | Theme     | Language |
			| NoInteraction | No       | DarkMagic | en-US    |

Scenario: Ib49. Remove User with moves and schedules
	Given the user last access was 578 days before
		And the user have being warned twice
		And the user have
			| System Stuff         |
			| Move                 |
			| Schedule             |
			| Move with Detail     |
			| Schedule with Detail |
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user won't exist
		And the count of warnings sent will be 0
		And there will be an export file with this content
			| Description             | Date       | Category                                  | Nature   | In                                 | Out                                 | Value | Conversion | Description1 | Amount1 | Value1 | Conversion1 | Description2 | Amount2 | Value2 | Conversion2 | Description3 | Amount3 | Value3 | Conversion3 | Description4 | Amount4 | Value4 | Conversion4 | Description5 | Amount5 | Value5 | Conversion5 | Description6 | Amount6 | Value6 | Conversion6 | Description7 | Amount7 | Value7 | Conversion7 | Description8 | Amount8 | Value8 | Conversion8 | Description9 | Amount9 | Value9 | Conversion9 | Description10 | Amount10 | Value10 | Conversion10 | Description11 | Amount11 | Value11 | Conversion11 | Description12 | Amount12 | Value12 | Conversion12 | Description13 | Amount13 | Value13 | Conversion13 | Description14 | Amount14 | Value14 | Conversion14 | Description15 | Amount15 | Value15 | Conversion15 | Description16 | Amount16 | Value16 | Conversion16 | Description17 | Amount17 | Value17 | Conversion17 | Description18 | Amount18 | Value18 | Conversion18 | Description19 | Amount19 | Value19 | Conversion19 | Description20 | Amount20 | Value20 | Conversion20 | Description21 | Amount21 | Value21 | Conversion21 | Description22 | Amount22 | Value22 | Conversion22 | Description23 | Amount23 | Value23 | Conversion23 | Description24 | Amount24 | Value24 | Conversion24 | Description25 | Amount25 | Value25 | Conversion25 | Description26 | Amount26 | Value26 | Conversion26 | Description27 | Amount27 | Value27 | Conversion27 | Description28 | Amount28 | Value28 | Conversion28 | Description29 | Amount29 | Value29 | Conversion29 | Description30 | Amount30 | Value30 | Conversion30 |
			| Move {scenarioCode}     | 2021-05-07 | Category {scenarioCode} move              | Transfer | Account {scenarioCode} in          | Account {scenarioCode} out          | 0.27  |            |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |
			| Move {scenarioCode}     | 2021-05-07 | Category {scenarioCode} move detailed     | Transfer | Account {scenarioCode} in detailed | Account {scenarioCode} out detailed |       |            | Detail 1     | 3       | 0.09   |             | Detail 2     | 3       | 0.09   |             | Detail 3     | 3       | 0.09   |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |
			| Schedule {scenarioCode} | 3000-03-27 | Category {scenarioCode} schedule          | Transfer | Account {scenarioCode} in          | Account {scenarioCode} out          | 0.27  |            |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |
			| Schedule {scenarioCode} | 3000-03-27 | Category {scenarioCode} schedule detailed | Transfer | Account {scenarioCode} in          | Account {scenarioCode} out          |       |            | Detail 1     | 3       | 0.09   |             | Detail 2     | 3       | 0.09   |             | Detail 3     | 3       | 0.09   |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |
			| Schedule {scenarioCode} | 3000-04-27 | Category {scenarioCode} schedule          | Transfer | Account {scenarioCode} in          | Account {scenarioCode} out          | 0.27  |            |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |
			| Schedule {scenarioCode} | 3000-04-27 | Category {scenarioCode} schedule detailed | Transfer | Account {scenarioCode} in          | Account {scenarioCode} out          |       |            | Detail 1     | 3       | 0.09   |             | Detail 2     | 3       | 0.09   |             | Detail 3     | 3       | 0.09   |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |
			| Schedule {scenarioCode} | 3000-05-27 | Category {scenarioCode} schedule          | Transfer | Account {scenarioCode} in          | Account {scenarioCode} out          | 0.27  |            |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |
			| Schedule {scenarioCode} | 3000-05-27 | Category {scenarioCode} schedule detailed | Transfer | Account {scenarioCode} in          | Account {scenarioCode} out          |       |            | Detail 1     | 3       | 0.09   |             | Detail 2     | 3       | 0.09   |             | Detail 3     | 3       | 0.09   |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |
			| Schedule {scenarioCode} | 3000-06-27 | Category {scenarioCode} schedule          | Transfer | Account {scenarioCode} in          | Account {scenarioCode} out          | 0.27  |            |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |
			| Schedule {scenarioCode} | 3000-06-27 | Category {scenarioCode} schedule detailed | Transfer | Account {scenarioCode} in          | Account {scenarioCode} out          |       |            | Detail 1     | 3       | 0.09   |             | Detail 2     | 3       | 0.09   |             | Detail 3     | 3       | 0.09   |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |
			| Schedule {scenarioCode} | 3000-07-27 | Category {scenarioCode} schedule          | Transfer | Account {scenarioCode} in          | Account {scenarioCode} out          | 0.27  |            |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |
			| Schedule {scenarioCode} | 3000-07-27 | Category {scenarioCode} schedule detailed | Transfer | Account {scenarioCode} in          | Account {scenarioCode} out          |       |            | Detail 1     | 3       | 0.09   |             | Detail 2     | 3       | 0.09   |             | Detail 3     | 3       | 0.09   |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |
			| Schedule {scenarioCode} | 3000-08-27 | Category {scenarioCode} schedule          | Transfer | Account {scenarioCode} in          | Account {scenarioCode} out          | 0.27  |            |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |
			| Schedule {scenarioCode} | 3000-08-27 | Category {scenarioCode} schedule detailed | Transfer | Account {scenarioCode} in          | Account {scenarioCode} out          |       |            | Detail 1     | 3       | 0.09   |             | Detail 2     | 3       | 0.09   |             | Detail 3     | 3       | 0.09   |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |
			| Schedule {scenarioCode} | 3000-09-27 | Category {scenarioCode} schedule          | Transfer | Account {scenarioCode} in          | Account {scenarioCode} out          | 0.27  |            |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |
			| Schedule {scenarioCode} | 3000-09-27 | Category {scenarioCode} schedule detailed | Transfer | Account {scenarioCode} in          | Account {scenarioCode} out          |       |            | Detail 1     | 3       | 0.09   |             | Detail 2     | 3       | 0.09   |             | Detail 3     | 3       | 0.09   |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |
			| Schedule {scenarioCode} | 3000-10-27 | Category {scenarioCode} schedule          | Transfer | Account {scenarioCode} in          | Account {scenarioCode} out          | 0.27  |            |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |
			| Schedule {scenarioCode} | 3000-10-27 | Category {scenarioCode} schedule detailed | Transfer | Account {scenarioCode} in          | Account {scenarioCode} out          |       |            | Detail 1     | 3       | 0.09   |             | Detail 2     | 3       | 0.09   |             | Detail 3     | 3       | 0.09   |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |
			| Schedule {scenarioCode} | 3000-11-27 | Category {scenarioCode} schedule          | Transfer | Account {scenarioCode} in          | Account {scenarioCode} out          | 0.27  |            |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |
			| Schedule {scenarioCode} | 3000-11-27 | Category {scenarioCode} schedule detailed | Transfer | Account {scenarioCode} in          | Account {scenarioCode} out          |       |            | Detail 1     | 3       | 0.09   |             | Detail 2     | 3       | 0.09   |             | Detail 3     | 3       | 0.09   |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |
			| Schedule {scenarioCode} | 3000-12-27 | Category {scenarioCode} schedule          | Transfer | Account {scenarioCode} in          | Account {scenarioCode} out          | 0.27  |            |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |
			| Schedule {scenarioCode} | 3000-12-27 | Category {scenarioCode} schedule detailed | Transfer | Account {scenarioCode} in          | Account {scenarioCode} out          |       |            | Detail 1     | 3       | 0.09   |             | Detail 2     | 3       | 0.09   |             | Detail 3     | 3       | 0.09   |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |              |         |        |             |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |               |          |         |              |
		And it will be registered at wipe table
			| Reason        | CSV file | Theme     | Language |
			| NoInteraction | Yes      | DarkMagic | en-US    |

Scenario: Ib50. Send e-mail using user language
	Given the user last access was 748 days before
		And user language is pt-BR
	When robot user login
		And user robot language is en-US
		And call wipe users
	Then I will receive no core error
		And the e-mail subject will be "Parece que você não está mais usando o sistema"
		And the e-mail body will contain "estão para ser excluídos"

Scenario: Ib51. Check days counting
	Given the user last access was 63 days before
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the e-mail body will contain "27</h1>days"

Scenario: Ib52. Check wipe notice
	Given the user last access was 105 days before
		And the user have being warned twice
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user won't exist
		And the count of warnings sent will be 0
		And there will be a wipe notice sent
		And it will be registered at wipe table
			| Reason        | CSV file | Theme     | Language |
			| NoInteraction | No       | DarkMagic | en-US    |

Scenario: Ib53. Wipe when user asks too
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
		And it will be registered at wipe table
			| Reason      | CSV file | Theme     | Language |
			| PersonAsked | No       | DarkMagic | en-US    |

Scenario: Ib54. Wipe just the right user
	Given the user have
			| System Stuff |
			| Move         |
			| Schedule     |
		And data wipe was asked
		And I have this user created
			| Email                                        | Signed | Active |
			| dont_wipe_me_{scenarioCode}@dontflymoney.com | true   | true   |
		And the user dont_wipe_me_{scenarioCode}@dontflymoney.com have
			| System Stuff |
			| Move         |
			| Schedule     |
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user won't exist
		But the user dont_wipe_me_{scenarioCode}@dontflymoney.com will still exist

Scenario: Ib55. Do not warn robots without activity
	Given the user last access was 90 days before
		But the user is a robot
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And the count of warnings sent will be 0
		And and the user warning count will be 0

Scenario: Ib56. Do not wipe robots without activity
	Given the user last access was 90 days before
		And the user have being warned twice
		But the user is a robot
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And it will not be registered at wipe table

Scenario: Ib57. Do not warn robots which did not accepted last contract
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

Scenario: Ib58. Do not wipe robots which did not accepted last contract
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
Scenario: Ib59. Do not wipe robots even if it "asks" too
	Given data wipe was asked
		But the user is a robot
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user will still exist
		And it will not be registered at wipe table

Scenario: Ib60. Wipe user order with file
	Given the user have
			| System Stuff     |
			| Order            |
		And data wipe was asked
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user won't exist
		And it will be registered at wipe table
			| Reason      | CSV file | Theme     | Language |
			| PersonAsked | No       | DarkMagic | en-US    |
		And no order files will exist

Scenario: Ib61. Wipe complete user
	Given the user have
			| System Stuff     |
			| Control          |
			| Settings         |
			| Acceptance       |
			| Security         |
			| Ticket           |
			| Tips             |
			| Account          |
			| Category         |
			| Move             |
			| Detail           |
			| Summary          |
			| Schedule         |
			| Archive          |
			| Line             |
			| Line with Detail |
			| Order            |
		And data wipe was asked
	When robot user login
		And call wipe users
	Then I will receive no core error
		And the user won't exist
		And it will be registered at wipe table
			| Reason      | CSV file | Theme     | Language |
			| PersonAsked | No       | DarkMagic | en-US    |
		And no order files will exist
