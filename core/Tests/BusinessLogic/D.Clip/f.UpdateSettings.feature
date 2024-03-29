﻿Feature: Df. Update settings

Background:
	Given test user login
		And I enable Categories use
		And I have a category

Scenario: Df01. Disable categories use and save move with category
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

Scenario: Df02. Disable categories use and save schedule with category
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

Scenario: Df03. Disable categories use and create a category
	Given I disable Categories use
		And I have this category to create
			| Name          |
			| Category Bk03 |
	When I try to save the category
	Then I will receive this core error: CategoriesDisabled
		And the category will not be saved

Scenario: Df04. Disable categories use and select a category
	Given I pass a valid category name
		And I disable Categories use
	When I try to get the category by its name
	Then I will receive this core error: CategoriesDisabled
		And I will receive no category

Scenario: Df05. Disable categories use and disable a category
	Given I give the enabled category Bk05
		And I disable Categories use
	When I try to disable the category
	Then I will receive this core error: CategoriesDisabled

Scenario: Df06. Disable categories use and enable a category
	Given I give the disabled category Bk06
		And I disable Categories use
	When I try to enable the category
	Then I will receive this core error: CategoriesDisabled

Scenario: Df07. Change language to one that doesn't exist
	When I try to change the language to zz-ZZ
	Then I will receive this core error: LanguageUnknown

Scenario: Df08. Change timezone to one that doesn't exist
	When I try to change the timezone to UTC-99:99
	Then I will receive this core error: TimeZoneUnknown

Scenario: Df09. Disable categories use
	Given I enable Categories use
	When I try to disable Categories use
	Then I will receive no core error

Scenario: Df10. Enable categories use
	Given I disable Categories use
	When I try to enable Categories use
	Then I will receive no core error

Scenario: Df11. Disable categories use and save move without category
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

Scenario: Df12. Disable categories use and save schedule without category
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

Scenario: Df13. Change language to pt-BR
	When I try to change the language to pt-BR
	Then I will receive no core error
		And the translation will be
			| Key             | Translated |
			| CurrentLanguage | português  |

Scenario: Df14. Change language to en-US
	When I try to change the language to en-US
	Then I will receive no core error
		And the translation will be
			| Key             | Translated |
			| CurrentLanguage | english    |

Scenario: Df15. Change timezone
	When I try to change the timezone to UTC-03:00
	Then I will receive no core error

Scenario: Df16. Disable move send e-mail
	Given I enable move send e-mail
	When I try to disable move send e-mail
	Then I will receive no core error

Scenario: Df17. Enable move send e-mail
	Given I disable move send e-mail
	When I try to enable move send e-mail
	Then I will receive no core error

Scenario: Df18. Disable move check
	Given I enable move check
	When I try to disable move check
	Then I will receive no core error

Scenario: Df19. Enable move check
	Given I disable move check
	When I try to enable move check
	Then I will receive no core error

Scenario: Df20. Disable wizard
	Given I enable wizard
	When I try to disable wizard
	Then I will receive no core error

Scenario: Df21. Enable wizard
	Given I disable wizard
	When I try to enable wizard
	Then I will receive no core error

Scenario: Df22. Not update if user is marked for deletion
	Given I disable wizard
		But the user is marked for deletion
	When I try to enable wizard
	Then I will receive this core error: UserDeleted

Scenario: Df23. Not update if user requested wipe
	Given I disable wizard
		But the user asked data wipe
	When I try to enable wizard
	Then I will receive this core error: UserAskedWipe

Scenario: Df24. Disable accounts signs
	Given have enabled accounts signs
		And I have this account
			| Name         | Yellow | Red |
			| Disable Sign | 20     | 10  |
	When disable accounts signs
	Then I will receive no core error
		And account sign will not be available
		And the account list will not have sign
		And the year report will not have sign
		And the month report will not have sign

Scenario: Df25. Enable accounts signs
	Given have enabled accounts signs
		And I have this account
			| Name         | Yellow | Red |
			| Disable Sign | 20     | 10  |
		And have disabled accounts signs
	When enable accounts signs
	Then I will receive no core error
		And account sign will be available
		And the account list will have sign
		And the year report will have sign
		And the month report will have sign

Scenario: Df26. Update Settings - Categories Disable without signing contract
	Given there is a new contract
	When I try to disable Categories use
	Then I will receive this core error: NotSignedLastContract

Scenario: Df27. Update Settings - Categories Enable without signing contract
	Given there is a new contract
	When I try to enable Categories use
	Then I will receive this core error: NotSignedLastContract

Scenario: Df28. Update Settings - Move Send E-mail Disable without signing contract
	Given there is a new contract
	When I try to disable move send e-mail
	Then I will receive this core error: NotSignedLastContract

Scenario: Df29. Update Settings - Move Send E-mail Enable without signing contract
	Given there is a new contract
	When I try to enable move send e-mail
	Then I will receive this core error: NotSignedLastContract

Scenario: Df30. Update Settings - Change Language without signing contract
	Given there is a new contract
	When I try to change the language to en-US
	Then I will receive this core error: NotSignedLastContract

Scenario: Df31. Update Settings - Change TimeZone without signing contract
	Given there is a new contract
	When I try to change the timezone to UTC-03:00
	Then I will receive this core error: NotSignedLastContract
