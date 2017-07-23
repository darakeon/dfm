Feature: b. Year report

Background:
	Given I have an active user
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

Scenario: 01. Get with invalid Account ID (E)
	Given I pass an invalid account ID
	And I pass this date
		| Year |
		| 2012 |
	When I try to get the year report
	Then I will receive this core error: InvalidAccount
	And I will receive no year report

Scenario: 02. Get with Date Year Zero (E)
	Given I pass a valid account ID
	And I pass this date
		| Year |
		| 0    |
	When I try to get the year report
	Then I will receive this core error: InvalidYear
	And I will receive no year report



Scenario: 99. Get with info all right (S)
	Given I pass a valid account ID
	And I pass this date
		| Year |
		| 2012 |
	When I try to get the year report
	Then I will receive no core error
	And I will receive the year report
	And its sum value will be equal to its months sum value
