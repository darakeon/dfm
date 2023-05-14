Feature: Ed. Get category report

Background:
	Given test user login
		And I enable Categories use
		And I have an account
		And I have these categories
			| Name          |
			| CatReport LZN |
			| CatReport USC |
			| CatReport RFB |
		And I have moves of
			| Date       | Value | Category      | Detail | Nature |
			| 1986-03-27 | 27    | CatReport LZN |        | Out    |
			| 1986-03-27 | 3     | CatReport USC |        | In     |
			| 1986-03-27 | 1986  | CatReport RFB |        | Out    |
			| 2021-07-30 | 1     | CatReport LZN |        | Out    |
			| 2021-07-31 | 2     | CatReport USC |        | Out    |
			| 2021-08-01 | 3     | CatReport RFB |        | Out    |
			| 2021-08-02 | 4     | CatReport LZN |        | In     |
			| 2021-08-03 | 5     | CatReport USC |        | In     |
			| 2021-08-04 | 6     | CatReport RFB |        | In     |
			| 2021-08-05 |       | CatReport LZN | LZN    |        |
			| 2021-08-06 |       | CatReport USC | USC    |        |
			| 2021-08-07 |       | CatReport RFB | RFB    |        |

Scenario: Ed01. Get with invalid Account name
	Given I pass an invalid account url
		And I pass this date
			| Month | Year |
			| 8     | 2021 |
	When I try to get the category report
	Then I will receive this core error: InvalidAccount
		And I will receive no category report

Scenario: Ed02. Get with Date Year Zero
	Given I pass a valid account url
		And I pass this date
			| Month | Year |
			| 8     | 0    |
	When I try to get the category report
	Then I will receive this core error: InvalidYear
		And I will receive no category report

Scenario: Ed03. Get with Date Month less than 1
	Given I pass a valid account url
		And I pass this date
			| Month | Year |
			| 0     | 2021 |
	When I try to get the category report
	Then I will receive this core error: InvalidMonth
		And I will receive no category report

Scenario: Ed04. Get with Date Month more than 12
	Given I pass a valid account url
		And I pass this date
			| Month | Year |
			| 13    | 2021 |
	When I try to get the category report
	Then I will receive this core error: InvalidMonth
		And I will receive no category report

Scenario: Ed05. Get with next year date
	Given I have moves of
			| Date       |
			| +0         |
		And I pass a valid account url
		And I pass this date
			| Month | Year |
			| +0    | +1   |
	When I try to get the category report
	Then I will receive no core error
		And I will receive empty category report

Scenario: Ed06. Get with info all right for month
	Given I pass a valid account url
		And I pass this date
			| Month | Year |
			| 8     | 2021 |
	When I try to get the category report
	Then I will receive no core error
		And I will receive this category report
			| Category      | Out | In |
			| CatReport LZN | 10  | 4  |
			| CatReport RFB | 13  | 6  |
			| CatReport USC | 10  | 5  |

Scenario: Ed07. Not get report if user is marked for deletion
	Given I pass a valid account url
		And I pass this date
			| Month | Year |
			| 8     | 2021 |
		But the user is marked for deletion
	When I try to get the category report
	Then I will receive this core error: UserDeleted

Scenario: Ed08. Not get report if user requested wipe
	Given I pass a valid account url
		And I pass this date
			| Month | Year |
			| 8     | 2021 |
		But the user asked data wipe
	When I try to get the category report
	Then I will receive this core error: UserAskedWipe

Scenario: Ed09. Get with info all right for year
	Given I pass a valid account url
		And I pass this date
			| Year |
			| 2021 |
	When I try to get the category report
	Then I will receive no core error
		And I will receive this category report
			| Category      | Out | In |
			| CatReport LZN | 11  | 4  |
			| CatReport RFB | 13  | 6  |
			| CatReport USC | 12  | 5  |

Scenario: Ed10. Not get report if user logoff
	Given I pass a valid account url
		And I pass this date
			| Month | Year |
			| 8     | 2021 |
		But I logoff the user
	When I try to get the category report
	Then I will receive this core error: Uninvited

Scenario: Ed11. Not get report if not signed last contract
	Given I pass a valid account url
		And I pass this date
			| Month | Year |
			| 4     | 2023 |
		But there is a new contract
	When I try to get the category report
	Then I will receive this core error: NotSignedLastContract
