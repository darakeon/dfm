Feature: Gr. Export Order

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

Scenario: Gr01. Unlogged user
	Given order start date 1986-03-27
		And order end date 2024-08-24
		And order account account_out
		And order account account_in
		And order account account_out_eur
		And order account account_in_brl
		And order category Category 1
		And order category Category 2
		And an export is ordered
		But I have no logged user (logoff)
	When export order
	Then I will receive this core error: Uninvited
		And there will be no export file
		And order status will be Pending

Scenario: Gr02. No robot user
	Given order start date 1986-03-27
		And order end date 2024-08-24
		And order account account_out
		And order account account_in
		And order account account_out_eur
		And order account account_in_brl
		And order category Category 1
		And order category Category 2
		And an export is ordered
	When export order
	Then I will receive this core error: Uninvited
		And there will be no export file
		And order status will be Pending

Scenario: Gr03. User marked for deletion
	Given order start date 1986-03-27
		And order end date 2024-08-24
		And order account account_out
		And order account account_in
		And order account account_out_eur
		And order account account_in_brl
		And order category Category 1
		And order category Category 2
		And an export is ordered
		But the user is marked for deletion
	When robot user login
		And export order
	Then I will receive this core error: UserDeleted
		And there will be no export file
		And order status will be Error

Scenario: Gr04. User requested wipe
	Given order start date 1986-03-27
		And order end date 2024-08-24
		And order account account_out
		And order account account_in
		And order account account_out_eur
		And order account account_in_brl
		And order category Category 1
		And order category Category 2
		And an export is ordered
		But the user asked data wipe
	When robot user login
		And export order
	Then I will receive this core error: UserAskedWipe
		And there will be no export file
		And order status will be Error

Scenario: Gr05. Without sign last contract
	Given order start date 1986-03-27
		And order end date 2024-08-24
		And order account account_out
		And order account account_in
		And order account account_out_eur
		And order account account_in_brl
		And order category Category 1
		And order category Category 2
		And an export is ordered
		But there is a new contract
	When robot user login
		And export order
	Then I will receive this core error: NotSignedLastContract
		And there will be no export file
		And order status will be Error

Scenario: Gr06. No Categories
	Given these settings
			| UseCategories |
			| false         |
		And order start date 1986-03-27
		And order end date 2024-08-24
		And order account account_out
		And order account account_in
		And order account account_out_eur
		And order account account_in_brl
		And an export is ordered
	When robot user login
		And export order
	Then I will receive no core error
		And order status will be Success
		And there will be an export file with this content
			| Description                                      | Date       | Category   | Nature   | In              | Out            | Value | Conversion | Description1 | Amount1 | Value1 | Conversion1 |
			| Sample Move Out                                  | 1986-03-27 | Category 1 | Out      | Account Out     |                | 1     |            |              |         |        |             |
			| Sample Move Out with Details                     | 1988-10-17 | Category 1 | Out      | Account Out     |                |       |            | D1           | 1       | 1      |             |
			| Sample Move In                                   | 1991-05-10 | Category 1 | In       |                 | Account In     | 1     |            |              |         |        |             |
			| Sample Move In with Details                      | 1993-12-01 | Category 1 | In       |                 | Account In     |       |            | D1           | 1       | 1      |             |
			| Sample Move Transfer                             | 1996-06-23 | Category 1 | Transfer | Account Out     | Account In     | 1     |            |              |         |        |             |
			| Sample Move Transfer with Details                | 1999-01-14 | Category 1 | Transfer | Account Out     | Account In     |       |            | D1           | 1       | 1      |             |
			| Sample Move Transfer with Conversion             | 2001-08-07 | Category 1 | Transfer | Account Out EUR | Account In BRL | 1     | 10         |              |         |        |             |
			| Sample Move Transfer with Conversion and Details | 2004-02-28 | Category 1 | Transfer | Account Out EUR | Account In BRL |       |            | D1           | 1       | 1      | 10          |
			| Sample Move Out                                  | 2006-09-20 | Category 2 | Out      | Account Out     |                | 1     |            |              |         |        |             |
			| Sample Move Out with Details                     | 2009-04-13 | Category 2 | Out      | Account Out     |                |       |            | D1           | 1       | 1      |             |
			| Sample Move In                                   | 2011-11-04 | Category 2 | In       |                 | Account In     | 1     |            |              |         |        |             |
			| Sample Move In with Details                      | 2014-05-27 | Category 2 | In       |                 | Account In     |       |            | D1           | 1       | 1      |             |
			| Sample Move Transfer                             | 2016-12-18 | Category 2 | Transfer | Account Out     | Account In     | 1     |            |              |         |        |             |
			| Sample Move Transfer with Details                | 2019-07-11 | Category 2 | Transfer | Account Out     | Account In     |       |            | D1           | 1       | 1      |             |
			| Sample Move Transfer with Conversion             | 2022-01-31 | Category 2 | Transfer | Account Out EUR | Account In BRL | 1     | 10         |              |         |        |             |
			| Sample Move Transfer with Conversion and Details | 2024-08-24 | Category 2 | Transfer | Account Out EUR | Account In BRL |       |            | D1           | 1       | 1      | 10          |

Scenario: Gr07. All data
	Given order start date 1986-03-27
		And order end date 2024-08-24
		And order account account_out
		And order account account_in
		And order account account_out_eur
		And order account account_in_brl
		And order category Category 1
		And order category Category 2
		And an export is ordered
	When robot user login
		And export order
	Then I will receive no core error
		And order status will be Success
		And there will be an export file with this content
			| Description                                      | Date       | Category   | Nature   | In              | Out            | Value | Conversion | Description1 | Amount1 | Value1 | Conversion1 |
			| Sample Move Out                                  | 1986-03-27 | Category 1 | Out      | Account Out     |                | 1     |            |              |         |        |             |
			| Sample Move Out with Details                     | 1988-10-17 | Category 1 | Out      | Account Out     |                |       |            | D1           | 1       | 1      |             |
			| Sample Move In                                   | 1991-05-10 | Category 1 | In       |                 | Account In     | 1     |            |              |         |        |             |
			| Sample Move In with Details                      | 1993-12-01 | Category 1 | In       |                 | Account In     |       |            | D1           | 1       | 1      |             |
			| Sample Move Transfer                             | 1996-06-23 | Category 1 | Transfer | Account Out     | Account In     | 1     |            |              |         |        |             |
			| Sample Move Transfer with Details                | 1999-01-14 | Category 1 | Transfer | Account Out     | Account In     |       |            | D1           | 1       | 1      |             |
			| Sample Move Transfer with Conversion             | 2001-08-07 | Category 1 | Transfer | Account Out EUR | Account In BRL | 1     | 10         |              |         |        |             |
			| Sample Move Transfer with Conversion and Details | 2004-02-28 | Category 1 | Transfer | Account Out EUR | Account In BRL |       |            | D1           | 1       | 1      | 10          |
			| Sample Move Out                                  | 2006-09-20 | Category 2 | Out      | Account Out     |                | 1     |            |              |         |        |             |
			| Sample Move Out with Details                     | 2009-04-13 | Category 2 | Out      | Account Out     |                |       |            | D1           | 1       | 1      |             |
			| Sample Move In                                   | 2011-11-04 | Category 2 | In       |                 | Account In     | 1     |            |              |         |        |             |
			| Sample Move In with Details                      | 2014-05-27 | Category 2 | In       |                 | Account In     |       |            | D1           | 1       | 1      |             |
			| Sample Move Transfer                             | 2016-12-18 | Category 2 | Transfer | Account Out     | Account In     | 1     |            |              |         |        |             |
			| Sample Move Transfer with Details                | 2019-07-11 | Category 2 | Transfer | Account Out     | Account In     |       |            | D1           | 1       | 1      |             |
			| Sample Move Transfer with Conversion             | 2022-01-31 | Category 2 | Transfer | Account Out EUR | Account In BRL | 1     | 10         |              |         |        |             |
			| Sample Move Transfer with Conversion and Details | 2024-08-24 | Category 2 | Transfer | Account Out EUR | Account In BRL |       |            | D1           | 1       | 1      | 10          |

Scenario: Gr08. Not all Categories
	Given order start date 1986-03-27
		And order end date 2024-08-24
		And order account account_out
		And order account account_in
		And order account account_out_eur
		And order account account_in_brl
		And order category Category 1
		And an export is ordered
	When robot user login
		And export order
	Then I will receive no core error
		And order status will be Success
		And there will be an export file with this content
			| Description                                      | Date       | Category   | Nature   | In              | Out            | Value | Conversion | Description1 | Amount1 | Value1 | Conversion1 |
			| Sample Move Out                                  | 1986-03-27 | Category 1 | Out      | Account Out     |                | 1     |            |              |         |        |             |
			| Sample Move Out with Details                     | 1988-10-17 | Category 1 | Out      | Account Out     |                |       |            | D1           | 1       | 1      |             |
			| Sample Move In                                   | 1991-05-10 | Category 1 | In       |                 | Account In     | 1     |            |              |         |        |             |
			| Sample Move In with Details                      | 1993-12-01 | Category 1 | In       |                 | Account In     |       |            | D1           | 1       | 1      |             |
			| Sample Move Transfer                             | 1996-06-23 | Category 1 | Transfer | Account Out     | Account In     | 1     |            |              |         |        |             |
			| Sample Move Transfer with Details                | 1999-01-14 | Category 1 | Transfer | Account Out     | Account In     |       |            | D1           | 1       | 1      |             |
			| Sample Move Transfer with Conversion             | 2001-08-07 | Category 1 | Transfer | Account Out EUR | Account In BRL | 1     | 10         |              |         |        |             |
			| Sample Move Transfer with Conversion and Details | 2004-02-28 | Category 1 | Transfer | Account Out EUR | Account In BRL |       |            | D1           | 1       | 1      | 10          |

Scenario: Gr09. Not all Accounts (Out)
	Given order start date 1986-03-27
		And order end date 2024-08-24
		And order account account_out
		And order category Category 1
		And order category Category 2
		And an export is ordered
	When robot user login
		And export order
	Then I will receive no core error
		And order status will be Success
		And there will be an export file with this content
			| Description                                      | Date       | Category   | Nature   | In              | Out            | Value | Conversion | Description1 | Amount1 | Value1 | Conversion1 |
			| Sample Move Out                                  | 1986-03-27 | Category 1 | Out      | Account Out     |                | 1     |            |              |         |        |             |
			| Sample Move Out with Details                     | 1988-10-17 | Category 1 | Out      | Account Out     |                |       |            | D1           | 1       | 1      |             |
			| Sample Move Transfer                             | 1996-06-23 | Category 1 | Transfer | Account Out     | Account In     | 1     |            |              |         |        |             |
			| Sample Move Transfer with Details                | 1999-01-14 | Category 1 | Transfer | Account Out     | Account In     |       |            | D1           | 1       | 1      |             |
			| Sample Move Out                                  | 2006-09-20 | Category 2 | Out      | Account Out     |                | 1     |            |              |         |        |             |
			| Sample Move Out with Details                     | 2009-04-13 | Category 2 | Out      | Account Out     |                |       |            | D1           | 1       | 1      |             |
			| Sample Move Transfer                             | 2016-12-18 | Category 2 | Transfer | Account Out     | Account In     | 1     |            |              |         |        |             |
			| Sample Move Transfer with Details                | 2019-07-11 | Category 2 | Transfer | Account Out     | Account In     |       |            | D1           | 1       | 1      |             |

Scenario: Gr10. Not all Accounts (In)
	Given order start date 1986-03-27
		And order end date 2024-08-24
		And order account account_in
		And order category Category 1
		And order category Category 2
		And an export is ordered
	When robot user login
		And export order
	Then I will receive no core error
		And order status will be Success
		And there will be an export file with this content
			| Description                                      | Date       | Category   | Nature   | In              | Out            | Value | Conversion | Description1 | Amount1 | Value1 | Conversion1 |
			| Sample Move In                                   | 1991-05-10 | Category 1 | In       |                 | Account In     | 1     |            |              |         |        |             |
			| Sample Move In with Details                      | 1993-12-01 | Category 1 | In       |                 | Account In     |       |            | D1           | 1       | 1      |             |
			| Sample Move Transfer                             | 1996-06-23 | Category 1 | Transfer | Account Out     | Account In     | 1     |            |              |         |        |             |
			| Sample Move Transfer with Details                | 1999-01-14 | Category 1 | Transfer | Account Out     | Account In     |       |            | D1           | 1       | 1      |             |
			| Sample Move In                                   | 2011-11-04 | Category 2 | In       |                 | Account In     | 1     |            |              |         |        |             |
			| Sample Move In with Details                      | 2014-05-27 | Category 2 | In       |                 | Account In     |       |            | D1           | 1       | 1      |             |
			| Sample Move Transfer                             | 2016-12-18 | Category 2 | Transfer | Account Out     | Account In     | 1     |            |              |         |        |             |
			| Sample Move Transfer with Details                | 2019-07-11 | Category 2 | Transfer | Account Out     | Account In     |       |            | D1           | 1       | 1      |             |
			
Scenario: Gr11. Not all dates
	Given order start date 1996-12-12
		And order end date 2011-01-08
		And order account account_out
		And order account account_in
		And order account account_out_eur
		And order account account_in_brl
		And order category Category 1
		And order category Category 2
		And an export is ordered
	When robot user login
		And export order
	Then I will receive no core error
		And order status will be Success
		And there will be an export file with this content
			| Description                                      | Date       | Category   | Nature   | In              | Out            | Value | Conversion | Description1 | Amount1 | Value1 | Conversion1 |
			| Sample Move Transfer with Details                | 1999-01-14 | Category 1 | Transfer | Account Out     | Account In     |       |            | D1           | 1       | 1      |             |
			| Sample Move Transfer with Conversion             | 2001-08-07 | Category 1 | Transfer | Account Out EUR | Account In BRL | 1     | 10         |              |         |        |             |
			| Sample Move Transfer with Conversion and Details | 2004-02-28 | Category 1 | Transfer | Account Out EUR | Account In BRL |       |            | D1           | 1       | 1      | 10          |
			| Sample Move Out                                  | 2006-09-20 | Category 2 | Out      | Account Out     |                | 1     |            |              |         |        |             |
			| Sample Move Out with Details                     | 2009-04-13 | Category 2 | Out      | Account Out     |                |       |            | D1           | 1       | 1      |             |

Scenario: Gr12. No moves to export
	Given order start date 1955-03-15
		And order end date 1959-03-11
		And order account account_out
		And order category Category 1
		And an export is ordered
	When robot user login
		And export order
	Then I will receive no core error
		And order status will be Error
		And there will be no export file
