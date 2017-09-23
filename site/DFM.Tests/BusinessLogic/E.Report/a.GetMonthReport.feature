Feature: Ea. Month report

Background:
	Given I have an active user who have accepted the contract
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

Scenario: Ea01. Get with invalid Account name (E)
	Given I pass an invalid account url
	And I pass this date
		| Month | Year |
		| 4     | 2012 |
	When I try to get the month report
	Then I will receive this core error: InvalidAccount
	And I will receive no month report

Scenario: Ea02. Get with Date Year Zero (E)
	Given I pass a valid account url
	And I pass this date
		| Month | Year |
		| 4     | 0    |
	When I try to get the month report
	Then I will receive this core error: InvalidYear
	And I will receive no month report

Scenario: Ea03. Get with Date Month less than 1 (E)
	Given I pass a valid account url
	And I pass this date
		| Month | Year |
		| 0     | 2012 |
	When I try to get the month report
	Then I will receive this core error: InvalidMonth
	And I will receive no month report

Scenario: Ea04. Get with Date Month more than 12 (E)
	Given I pass a valid account url
	And I pass this date
		| Month | Year |
		| 13    | 2012 |
	When I try to get the month report
	Then I will receive this core error: InvalidMonth
	And I will receive no month report



Scenario: Ea99. Get with info all right (S)
	Given I pass a valid account url
	And I pass this date
		| Month | Year |
		| 4     | 2012 |
	When I try to get the month report
	Then I will receive no core error
	And I will receive the month report
	And its sum value will be equal to its moves sum value
