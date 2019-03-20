Feature: Bk. Update Config

Background:
	Given I have a complete user logged in
		And I enable Categories use
		And I have a category

Scenario: Bk01. Disable categories use and save move with category
	Given I disable Categories use
		And I have this move to create
			| Description | Date       | Nature | Value |
			| Move Bk01   | 2014-03-04 | Out    | 10    |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
	When I try to save the move
	Then I will receive this core error: CategoriesDisabled
		And the move will not be saved

Scenario: Bk02. Disable categories use and save schedule with category
	Given I disable Categories use
		And I have this schedule to create
			| Description   | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Schedule Bk02 | 2014-03-04 | Out    | 10    | 10    | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
	When I try to save the schedule
	Then I will receive this core error: CategoriesDisabled
		And the schedule will not be saved

Scenario: Bk03. Disable categories use and create a category
	Given I disable Categories use
		And I have this category to create
			| Name          |
			| Category Bk03 |
	When I try to save the category
	Then I will receive this core error: CategoriesDisabled
		And the category will not be saved

Scenario: Bk04. Disable categories use and select a category
	Given I pass a valid category name
		And I disable Categories use
	When I try to get the category by its name
	Then I will receive this core error: CategoriesDisabled
		And I will receive no category

Scenario: Bk05. Disable categories use and disable a category
	Given I give the enabled category Bk05
		And I disable Categories use
	When I try to disable the category
	Then I will receive this core error: CategoriesDisabled

Scenario: Bk06. Disable categories use and enable a category
	Given I give the disabled category Bk06
		And I disable Categories use
	When I try to enable the category
	Then I will receive this core error: CategoriesDisabled

Scenario: Bk07. Change language to en-US
	When I try to change the language to zz-ZZ
	Then I will receive this core error: LanguageUnknown

Scenario: Bk08. Change timezone to en-US
	When I try to change the timezone to Someplace
	Then I will receive this core error: TimezoneUnknown

Scenario: Bk09. Disable categories use
	Given I enable Categories use
	When I try to disable Categories use
	Then I will receive no core error

Scenario: Bk10. Enable categories use
	Given I disable Categories use
	When I try to enable Categories use
	Then I will receive no core error

Scenario: Bk11. Disable categories use and save move without category
	Given I disable Categories use
		And I have this move to create
			| Description | Date       | Nature | Value |
			| Move Bk93   | 2014-03-04 | Out    | 10    |
		And it has no Details
		And it has no Category
		And it has an Account Out
		And it has no Account In
	When I try to save the move
	Then I will receive no core error
		And the move will be saved
		And the accountOut value will change in -10
		And the month-category-accountOut value will change in 10
		And the year-category-accountOut value will change in 10

Scenario: Bk12. Disable categories use and save schedule without category
	Given I disable Categories use
		And I have this schedule to create
			| Description   | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Schedule Bk94 | 2012-03-31 | Out    | 10    | 10    | False     | Monthly   | False           |
		And it has no Details
		And it has no Category
		And it has an Account Out
		And it has no Account In
	When I try to save the schedule
	Then I will receive no core error
		And the schedule will be saved

Scenario: Bk13. Change language to pt-BR
	When I try to change the language to pt-BR
	Then I will receive no core error
		And the translation will be
			| Key             | Translated |
			| CurrentLanguage | português  |

Scenario: Bk14. Change language to en-US
	When I try to change the language to en-US
	Then I will receive no core error
		And the translation will be
			| Key             | Translated |
			| CurrentLanguage | english    |

Scenario: Bk15. Change timezone
	When I try to change the timezone to E. South America Standard Time
	Then I will receive no core error

Scenario: Bk16. Disable move send e-mail
	Given I enable move send e-mail
	When I try to disable move send e-mail
	Then I will receive no core error

Scenario: Bk17. Enable move send e-mail
	Given I disable move send e-mail
	When I try to enable move send e-mail
	Then I will receive no core error

Scenario: Bk18. Disable move check
	Given I enable move check
	When I try to disable move check
	Then I will receive no core error

Scenario: Bk19. Enable move check
	Given I disable move check
	When I try to enable move check
	Then I will receive no core error

Scenario: Bk20. Disable wizard
	Given I enable wizard
	When I try to disable wizard
	Then I will receive no core error

Scenario: Bk21. Enable wizard
	Given I disable wizard
	When I try to enable wizard
	Then I will receive no core error
