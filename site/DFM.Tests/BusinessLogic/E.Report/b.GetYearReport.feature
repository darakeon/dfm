Feature: Eb. Year report

Background:
	Given I have an active user
		And the user have accepted the contract
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
		And its sum value will be equal to its months sum value
