﻿Feature: Gq. Order Export

Background:
	Given test user login
		And these settings
			| UseCategories | UseCurrency |
			| true          | true        |
		And I have these accounts
			| Name            | Currency |
			| Account Out     |          |
			| Account In      |          |
			| Account Out EUR | EUR      |
			| Account In BRL  | BRL      |
		And I have these categories
			| Name       |
			| Category 1 |
			| Category 2 |
		And I have moves of
			| Description                                      | Date       | Category   | Nature   | Out             | In             | Value | Conversion | Detail |
			| Sample Move Out                                  | 1986-03-27 | Category 1 | Out      | Account Out     |                | 1     |            |        |
			| Sample Move Out with Details                     | 1988-10-17 | Category 1 | Out      | Account Out     |                | 1     |            | D1     |
			| Sample Move In                                   | 1991-05-10 | Category 1 | In       |                 | Account In     | 1     |            |        |
			| Sample Move In with Details                      | 1993-12-01 | Category 1 | In       |                 | Account In     | 1     |            | D1     |
			| Sample Move Transfer                             | 1996-06-23 | Category 1 | Transfer | Account Out     | Account In     | 1     |            |        |
			| Sample Move Transfer with Details                | 1999-01-14 | Category 1 | Transfer | Account Out     | Account In     | 1     |            | D1     |
			| Sample Move Transfer with Conversion             | 2001-08-07 | Category 1 | Transfer | Account Out EUR | Account In BRL | 1     | 10         |        |
			| Sample Move Transfer with Conversion and Details | 2004-02-28 | Category 1 | Transfer | Account Out EUR | Account In BRL | 1     | 19         | D1     |
			| Sample Move Out                                  | 2006-09-20 | Category 2 | Out      | Account Out     |                | 1     |            |        |
			| Sample Move Out with Details                     | 2009-04-13 | Category 2 | Out      | Account Out     |                | 1     |            | D1     |
			| Sample Move In                                   | 2011-11-04 | Category 2 | In       |                 | Account In     | 1     |            |        |
			| Sample Move In with Details                      | 2014-05-27 | Category 2 | In       |                 | Account In     | 1     |            | D1     |
			| Sample Move Transfer                             | 2016-12-18 | Category 2 | Transfer | Account Out     | Account In     | 1     |            |        |
			| Sample Move Transfer with Details                | 2019-07-11 | Category 2 | Transfer | Account Out     | Account In     | 1     |            | D1     |
			| Sample Move Transfer with Conversion             | 2022-01-31 | Category 2 | Transfer | Account Out EUR | Account In BRL | 1     | 10         |        |
			| Sample Move Transfer with Conversion and Details | 2024-08-24 | Category 2 | Transfer | Account Out EUR | Account In BRL | 1     | 10         | D1     |

Scenario: Gq01. Unlogged user
	Given I have no logged user (logoff)
	When order export
	Then I will receive this core error: Uninvited
		And no order will be recorded

Scenario: Gq02. User marked for deletion
	Given the user is marked for deletion
	When order export
	Then I will receive this core error: UserDeleted
		And no order will be recorded

Scenario: Gq03. User requested wipe
	Given the user asked data wipe
	When order export
	Then I will receive this core error: UserAskedWipe
		And no order will be recorded

Scenario: Gq04. Without sign last contract
	Given there is a new contract
	When order export
	Then I will receive this core error: NotSignedLastContract
		And no order will be recorded

Scenario: Gq05. Start Date empty
	Given order end date 2024-08-24
		And order account account_out
		And order account account_in
		And order account account_out_eur
		And order account account_in_brl
		And order category Category 1
		And order category Category 2
	When order export
	Then I will receive this core error: InvalidDateRange
		And no order will be recorded

Scenario: Gq06. End Date empty
	Given order start date 1986-03-27
		And order account account_out
		And order account account_in
		And order account account_out_eur
		And order account account_in_brl
		And order category Category 1
		And order category Category 2
	When order export
	Then I will receive this core error: InvalidDateRange
		And no order will be recorded

Scenario: Gq07. Start Date after End Date
	Given order start date 2024-08-24
		And order end date 1986-03-27
		And order account account_out
		And order account account_in
		And order account account_out_eur
		And order account account_in_brl
		And order category Category 1
		And order category Category 2
	When order export
	Then I will receive this core error: InvalidDateRange
		And no order will be recorded

Scenario: Gq08. End Date after today
	Given order start date 1986-03-27
		And order end date 3024-08-24
		And order account account_out
		And order account account_in
		And order account account_out_eur
		And order account account_in_brl
		And order category Category 1
		And order category Category 2
	When order export
	Then I will receive this core error: InvalidDateRange
		And no order will be recorded

Scenario: Gq09. No Categories
	Given order start date 1986-03-27
		And order end date 2024-08-24
		And order account account_out
		And order account account_in
		And order account account_out_eur
		And order account account_in_brl
	When order export
	Then I will receive this core error: OrderNoCategories
		And no order will be recorded

Scenario: Gq10. Categories with Categories use disabled
	Given these settings
			| UseCategories |
			| False         |
		And order start date 1986-03-27
		And order end date 2024-08-24
		And order account account_out
		And order account account_in
		And order account account_out_eur
		And order account account_in_brl
		And order category Category 1
		And order category Category 2
	When order export
	Then I will receive this core error: CategoriesDisabled
		And no order will be recorded

Scenario: Gq11. No Categories with Categories use disabled
	Given these settings
			| UseCategories |
			| False         |
		And order start date 1986-03-27
		And order end date 2024-08-24
		And order account account_out
		And order account account_in
		And order account account_out_eur
		And order account account_in_brl
	When order export
	Then I will receive no core error
		And order will be recorded

Scenario: Gq12. Invalid Category
	Given order start date 1986-03-27
		And order end date 2024-08-24
		And order account account_out
		And order account account_in
		And order account account_out_eur
		And order account account_in_brl
		And order category Category 1
		And order category Category 2
		And order category Category 3
	When order export
	Then I will receive this core error: InvalidCategory
		And no order will be recorded

Scenario: Gq13. No Accounts
	Given order start date 1986-03-27
		And order end date 2024-08-24
		And order category Category 1
		And order category Category 2
	When order export
	Then I will receive this core error: OrderNoAccounts
		And no order will be recorded

Scenario: Gq14. Invalid Account
	Given order start date 1986-03-27
		And order end date 2024-08-24
		And order account account_out
		And order account account_in
		And order account account_out_eur
		And order account account_in_brl
		And order account Account
		And order category Category 1
		And order category Category 2
	When order export
	Then I will receive this core error: InvalidAccount
		And no order will be recorded

Scenario: Gq15. All data
	Given order start date 1986-03-27
		And order end date 2024-08-24
		And order account account_out
		And order account account_in
		And order account account_out_eur
		And order account account_in_brl
		And order category Category 1
		And order category Category 2
	When order export
	Then I will receive no core error
		And order will be recorded

Scenario: Gq16. Not all Categories
	Given order start date 1986-03-27
		And order end date 2024-08-24
		And order account account_out
		And order account account_in
		And order account account_out_eur
		And order account account_in_brl
		And order category Category 1
	When order export
	Then I will receive no core error
		And order will be recorded

Scenario: Gq17. Not all Accounts
	Given order start date 1986-03-27
		And order end date 2024-08-24
		And order account account_out
		And order category Category 1
		And order category Category 2
	When order export
	Then I will receive no core error
		And order will be recorded

Scenario: Gq18. Not all dates
	Given order start date 1996-12-12
		And order end date 2011-01-08
		And order account account_out
		And order account account_in
		And order account account_out_eur
		And order account account_in_brl
		And order category Category 1
		And order category Category 2
	When order export
	Then I will receive no core error
		And order will be recorded