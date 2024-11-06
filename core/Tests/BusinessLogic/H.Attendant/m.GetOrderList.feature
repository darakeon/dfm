Feature: Hm. Get Order List

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

Scenario: Hm01. Unlogged user
	Given I have no logged user (logoff)
	When get order list
	Then I will receive this core error: Uninvited

Scenario: Hm02. User marked for deletion
	Given the user is marked for deletion
	When get order list
	Then I will receive this core error: UserDeleted

Scenario: Hm03. User requested wipe
	Given the user asked data wipe
	When get order list
	Then I will receive this core error: UserAskedWipe

Scenario: Hm04. Without sign last contract
	Given there is a new contract
	When get order list
	Then I will receive this core error: NotSignedLastContract

Scenario: Hm05. No orders
	When get order list
	Then I will receive no core error
		And order list will be
			| Status | Creation | Exportation | Expiration | Sent | Start | End | CategoryList | AccountList | Path |

Scenario: Hm06. Not exported
	Given order start date 1986-03-27
		And order end date 2024-08-24
		And order account account_out
		And order account account_in
		And order account account_out_eur
		And order account account_in_brl
		And order category Category 1
		And order category Category 2
		And an export is ordered
	When get order list
	Then I will receive no core error
		And order list will be
			| Status  | Creation | Exportation | Expiration | Sent | Start      | End        | CategoryList          | AccountList                                           | Path |
			| Pending | Filled   |             |            |      | 1986-03-27 | 2024-08-24 | Category 1,Category 2 | Account Out,Account In,Account Out EUR,Account In BRL |      |

Scenario: Hm07. Order exported
	Given order start date 1986-03-27
		And order end date 2024-08-24
		And order account account_out
		And order account account_in
		And order account account_out_eur
		And order account account_in_brl
		And order category Category 1
		And order category Category 2
		And an export is ordered
		And robot export the order
		And test user login
	When get order list
	Then I will receive no core error
		And order list will be
			| Status  | Creation | Exportation | Expiration | Sent | Start      | End        | CategoryList          | AccountList                                           | Path   |
			| Success | Filled   | Filled      | Filled     | true | 1986-03-27 | 2024-08-24 | Category 1,Category 2 | Account Out,Account In,Account Out EUR,Account In BRL | exists |

Scenario: Hm08. No Categories
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
		And robot export the order
		And test user login
	When get order list
	Then I will receive no core error
		And order list will be
			| Status  | Creation | Exportation | Expiration | Sent | Start      | End        | CategoryList | AccountList                                           | Path   |
			| Success | Filled   | Filled      | Filled     | true | 1986-03-27 | 2024-08-24 |              | Account Out,Account In,Account Out EUR,Account In BRL | exists |

Scenario: Hm09. No moves to export
	Given order start date 1955-03-15
		And order end date 1959-03-11
		And order account account_out
		And order category Category 1
		And an export is ordered
		And robot export the order
		And test user login
	When get order list
	Then I will receive no core error
		And order list will be
			| Status | Creation | Exportation | Expiration | Sent  | Start      | End        | CategoryList | AccountList | Path |
			| Error  | Filled   |             |            | False | 1955-03-15 | 1959-03-11 | Category 1   | Account Out |      |

Scenario: Hm10. Not able to send email
	Given order start date 1986-03-27
		And order end date 1989-03-17
		And order account account_out
		And order category Category 1
		And an export is ordered
		But email system is out
		And robot export the order
		And test user login
	When get order list
	Then I will receive no core error
		And order list will be
			| Status  | Creation | Exportation | Expiration | Sent  | Start      | End        | CategoryList | AccountList | Path   |
			| Success | Filled   | Filled      | Filled     | false | 1986-03-27 | 1989-03-17 | Category 1   | Account Out | exists |

Scenario: Hm11. Out of limits
	Given order start date 1986-03-27
		And order end date 2024-11-01
		And order account account_out
		And order account account_in
		And order account account_out_eur
		And order account account_in_brl
		And order category Category 1
		And order category Category 2
		And an export is ordered
		But these limits in user plan
			| OrderMonth | OrderMove |
			| 1          | 3         |
		And robot export the order
		And test user login
	When get order list
	Then I will receive no core error
		And order list will be
			| Status     | Creation | Exportation | Expiration | Sent  | Start      | End        | CategoryList          | AccountList                                           | Path |
			| OutOfLimit | Filled   |             |            | false | 1986-03-27 | 2024-11-01 | Category 1,Category 2 | Account Out,Account In,Account Out EUR,Account In BRL |      |
