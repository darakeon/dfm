Feature: Eb. Get year report

Background:
	Given test user login
		And I enable Categories use
		And I have an account
		And I have moves of
			| Date       |
			| 2012-03-27 |
			| 2012-03-28 |
			| 2012-03-29 |
			| 2012-03-30 |
			| 2012-03-31 |
			| 2012-04-01 |
			| 2012-04-02 |
			| 2012-04-03 |
			| 2012-04-04 |
			| 2012-04-05 |
			| 2012-04-06 |

Scenario: Eb01. Get with invalid Account name
	Given I pass an invalid account url
		And I pass this date
			| Year |
			| 2012 |
	When I try to get the year report
	Then I will receive this core error: InvalidAccount
		And I will receive no year report

Scenario: Eb02. Get with Date Year Zero
	Given I pass a valid account url
		And I pass this date
			| Year |
			| 0    |
	When I try to get the year report
	Then I will receive this core error: InvalidYear
		And I will receive no year report

Scenario: Eb03. Get with info all right
	Given I pass a valid account url
		And I pass this date
			| Year |
			| 2012 |
	When I try to get the year report
	Then I will receive no core error
		And I will receive the year report
			| Number | Current In | Current Out | Current Total | Foreseen In | Foreseen Out | Foreseen Total |
			| 201201 | 0          | 0           | 0             | 0           | 0            | 0              |
			| 201202 | 0          | 0           | 0             | 0           | 0            | 0              |
			| 201203 | 0          | 50          | -50           | 0           | 0            | 0              |
			| 201204 | 0          | 60          | -60           | 0           | 0            | 0              |
			| 201205 | 0          | 0           | 0             | 0           | 0            | 0              |
			| 201206 | 0          | 0           | 0             | 0           | 0            | 0              |
			| 201207 | 0          | 0           | 0             | 0           | 0            | 0              |
			| 201208 | 0          | 0           | 0             | 0           | 0            | 0              |
			| 201209 | 0          | 0           | 0             | 0           | 0            | 0              |
			| 201210 | 0          | 0           | 0             | 0           | 0            | 0              |
			| 201211 | 0          | 0           | 0             | 0           | 0            | 0              |
			| 201212 | 0          | 0           | 0             | 0           | 0            | 0              |
		And its sum value will be equal to its months sum value
		And the year report sums will be
			| Current In | Current Out | Current Total | Foreseen In | Foreseen Out | Foreseen Total |
			| 0          | 110         | -110          | 0           | 0            | 0              |

Scenario: Eb04. Get foreseen ins, outs and balances
	Given I disable Categories use
		And I have schedules of
			| Description     | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Schedule Ea10 1 | 2220-10-08 | Out    | 10    | 3     | False     | Monthly   | True            |
			| Schedule Ea10 2 | 2220-09-08 | In     | 52    | 3     | False     | Monthly   | True            |
			| Schedule Ea10 3 | 2220-10-08 | In     | 7     | 20    | False     | Daily     | True            |
			| Schedule Ea10 4 | 2215-10-08 | Out    | 13    | 20    | False     | Yearly    | True            |
			| Schedule Ea10 5 | 2220-07-08 | Out    | 32    |       | True      | Monthly   | True            |
			| Schedule Ea10 6 | 2220-10-08 | Out    | 71    | 20    | False     | Monthly   | False           |
			| Schedule Ea10 7 | 2220-10-08 | Out    | 96    |       | True      | Monthly   | False           |
	Given I pass a valid account url
		And I pass this date
			| Year |
			| 2220 |
	When I try to get the year report
	Then I will receive no core error
		And I will receive the year report
			| Number | Current In | Current Out | Current Total | Foreseen In | Foreseen Out | Foreseen Total |
			| 222001 | 0          | 0           | 0             | 0           | 0            | 0              |
			| 222002 | 0          | 0           | 0             | 0           | 0            | 0              |
			| 222003 | 0          | 0           | 0             | 0           | 0            | 0              |
			| 222004 | 0          | 0           | 0             | 0           | 0            | 0              |
			| 222005 | 0          | 0           | 0             | 0           | 0            | 0              |
			| 222006 | 0          | 0           | 0             | 0           | 0            | 0              |
			| 222007 | 0          | 0           | 0             | 0           | 32           | -32            |
			| 222008 | 0          | 0           | 0             | 0           | 32           | -32            |
			| 222009 | 0          | 0           | 0             | 52          | 32           | +20            |
			| 222010 | 0          | 0           | 0             | 192         | 222          | -30            |
			| 222011 | 0          | 0           | 0             | 52          | 209          | -157           |
			| 222012 | 0          | 0           | 0             | 0           | 209          | -209           |
		And the year report sums will be
			| Current In | Current Out | Current Total | Foreseen In | Foreseen Out | Foreseen Total |
			| 0          | 0           | 0             | 296         | 736          | -440           |

Scenario: Eb05. Not get year report if user is marked for deletion
	Given I pass a valid account url
		And I pass this date
			| Year |
			| 2021 |
		But the user is marked for deletion
	When I try to get the year report
	Then I will receive this core error: UserDeleted

Scenario: Eb06. Not get year report if user requested wipe
	Given I pass a valid account url
		And I pass this date
			| Year |
			| 2021 |
		But the user asked data wipe
	When I try to get the year report
	Then I will receive this core error: UserAskedWipe

Scenario: Eb07. Not get year report if not signed last contract
	Given I pass a valid account url
		And I pass this date
			| Year |
			| 2023 |
		But there is a new contract
	When I try to get the year report
	Then I will receive this core error: NotSignedLastContract
