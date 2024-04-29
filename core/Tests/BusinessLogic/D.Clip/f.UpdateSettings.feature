Feature: Df. Update settings

Background:
	Given test user login

Scenario: Df01. Disable categories use and save move with category
	Given these settings
			| UseCategories |
			| true          |
		And I have a category
		And these settings
			| UseCategories |
			| false         |
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
	Given these settings
			| UseCategories |
			| true          |
		And I have a category
		And these settings
			| UseCategories |
			| false         |
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
	Given these settings
			| UseCategories |
			| false         |
		And I have this category to create
			| Name          |
			| Category Bk03 |
	When I try to save the category
	Then I will receive this core error: CategoriesDisabled
		And the category will not be saved

Scenario: Df04. Disable categories use and select a category
	Given these settings
			| UseCategories |
			| true          |
		And I have a category
		And I pass a valid category name
		And these settings
			| UseCategories |
			| false         |
	When I try to get the category by its name
	Then I will receive this core error: CategoriesDisabled
		And I will receive no category

Scenario: Df05. Disable categories use and disable a category
	Given these settings
			| UseCategories |
			| true          |
		And I give the enabled category Df05
		And these settings
			| UseCategories |
			| false         |
	When I try to disable the category
	Then I will receive this core error: CategoriesDisabled

Scenario: Df06. Disable categories use and enable a category
	Given these settings
			| UseCategories |
			| true          |
		And I give the disabled category Df06
		And these settings
			| UseCategories |
			| false         |
	When I try to enable the category
	Then I will receive this core error: CategoriesDisabled

Scenario: Df07. Change language to one that doesn't exist
	Given these settings
			| Language |
			| pt-BR    |
	When try update the settings
			| Language |
			| zz-ZZ    |
	Then I will receive this core error: LanguageUnknown
		And the settings will be
			| Language |
			| pt-BR    |

Scenario: Df08. Change timezone to one that doesn't exist
	Given these settings
			| TimeZone  |
			| UTC-03:00 |
	When try update the settings
			| TimeZone  |
			| UTC-99:99 |
	Then I will receive this core error: TimeZoneUnknown
		And the settings will be
			| TimeZone  |
			| UTC-03:00 |

Scenario: Df09. Disable categories use
	Given these settings
			| UseCategories |
			| true          |
	When try update the settings
			| UseCategories |
			| false         |
	Then I will receive no core error
		And the settings will be
			| UseCategories |
			| false         |			

Scenario: Df10. Enable categories use
	Given these settings
			| UseCategories |
			| false         |
	When try update the settings
			| UseCategories |
			| true          |
	Then I will receive no core error
		And the settings will be
			| UseCategories |
			| true          |

Scenario: Df11. Disable categories use and save move without category
	Given these settings
			| UseCategories |
			| false         |
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
	Given these settings
			| UseCategories |
			| false         |
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
	When try update the settings
			| Language |
			| pt-BR    |
	Then I will receive no core error
		And the settings will be
			| Language |
			| pt-BR    |
		And the translation will be
			| Key             | Translated |
			| CurrentLanguage | português  |

Scenario: Df14. Change language to en-US
	When try update the settings
			| Language |
			| en-US    |
	Then I will receive no core error
		And the settings will be
			| Language |
			| en-US    |
		And the translation will be
			| Key             | Translated |
			| CurrentLanguage | english    |

Scenario: Df15. Change timezone
	When try update the settings
			| Timezone  |
			| UTC-03:00 |
	Then I will receive no core error
		And the settings will be
			| Timezone  |
			| UTC-03:00 |

Scenario: Df16. Disable move send e-mail
	Given these settings
			| SendMoveEmail |
			| true          |
	When try update the settings
			| SendMoveEmail |
			| false         |
	Then I will receive no core error
		And the settings will be
			| SendMoveEmail |
			| false         |

Scenario: Df17. Enable move send e-mail
	Given these settings
			| SendMoveEmail |
			| false         |
	When try update the settings
			| SendMoveEmail |
			| true          |
	Then I will receive no core error
		And the settings will be
			| SendMoveEmail |
			| true          |

Scenario: Df18. Disable move check
	Given these settings
			| MoveCheck |
			| true      |
	When try update the settings
			| MoveCheck |
			| false     |
	Then I will receive no core error
		And the settings will be
			| MoveCheck |
			| false     |

Scenario: Df19. Enable move check
	Given these settings
			| MoveCheck |
			| false     |
	When try update the settings
			| MoveCheck |
			| true      |
	Then I will receive no core error
		And the settings will be
			| MoveCheck |
			| true      |

Scenario: Df20. Disable wizard
	Given these settings
			| Wizard |
			| true   |
	When try update the settings
			| Wizard |
			| false  |
	Then I will receive no core error
		And the settings will be
			| Wizard |
			| false  |

Scenario: Df21. Enable wizard
	Given these settings
			| Wizard |
			| false  |
	When try update the settings
			| Wizard |
			| true   |
	Then I will receive no core error
		And the settings will be
			| Wizard |
			| true   |

Scenario: Df22. Not update if user is marked for deletion
	Given these settings
			| Wizard |
			| false  |
		But the user is marked for deletion
	When try update the settings
			| Wizard |
			| true   |
	Then I will receive this core error: UserDeleted
		And the settings will be
			| Wizard |
			| false  |

Scenario: Df23. Not update if user requested wipe
	Given these settings
			| UseCategories | UseAccountsSigns | SendMoveEmail | MoveCheck | Wizard | UseCurrency | Language | TimeZone  |
			| true          | true             | true          | true      | true   | true        | pt-BR    | UTC-03:00 |
		But the user asked data wipe
	When try update the settings
			| UseCategories | UseAccountsSigns | SendMoveEmail | MoveCheck | Wizard | UseCurrency | Language | TimeZone  |
			| false         | false            | false         | false     | false  | false       | en-US    | UTC+01:00 |
	Then I will receive this core error: UserAskedWipe
		And the settings will be
			| UseCategories | UseAccountsSigns | SendMoveEmail | MoveCheck | Wizard | UseCurrency | Language | TimeZone  |
			| true          | true             | true          | true      | true   | true        | pt-BR    | UTC-03:00 |

Scenario: Df24. Disable accounts signs
	Given these settings
			| UseAccountsSigns |
			| true             |
		And I have this account
			| Name         | Yellow | Red |
			| Disable Sign | 20     | 10  |
	When try update the settings
			| UseAccountsSigns |
			| false            |
	Then I will receive no core error
		And the settings will be
			| UseAccountsSigns |
			| false            |
		And the account list will not have sign
		And the year report will not have sign
		And the month report will not have sign

Scenario: Df25. Enable accounts signs
	Given these settings
			| UseAccountsSigns |
			| true             |
		And I have this account
			| Name         | Yellow | Red |
			| Disable Sign | 20     | 10  |
		And these settings
			| UseAccountsSigns |
			| false            |
	When try update the settings
			| UseAccountsSigns |
			| true             |
	Then I will receive no core error
		And the settings will be
			| UseAccountsSigns |
			| true             |
		And the account list will have sign
		And the year report will have sign
		And the month report will have sign

Scenario: Df26. Update Settings without signing contract
	Given these settings
			| UseCategories | UseAccountsSigns | SendMoveEmail | MoveCheck | Wizard | UseCurrency | Language | TimeZone  |
			| true          | true             | true          | true      | true   | true        | pt-BR    | UTC-03:00 |
		And there is a new contract
	When try update the settings
			| UseCategories | UseAccountsSigns | SendMoveEmail | MoveCheck | Wizard | UseCurrency | Language | TimeZone  |
			| false         | false            | false         | false     | false  | false       | en-US    | UTC+01:00 |
	Then I will receive this core error: NotSignedLastContract
		And the settings will be
			| UseCategories | UseAccountsSigns | SendMoveEmail | MoveCheck | Wizard | UseCurrency | Language | TimeZone  |
			| true          | true             | true          | true      | true   | true        | pt-BR    | UTC-03:00 |

Scenario: Df27. Enable currency in accounts
	Given these settings
			| UseCurrency |
			| false       |
	When try update the settings
			| UseCurrency |
			| true        |
	Then I will receive no core error
		And the settings will be
			| UseCurrency |
			| true        |

Scenario: Df28. Disable currency in accounts
	Given these settings
			| UseCurrency |
			| true        |
	When try update the settings
			| UseCurrency |
			| false       |
	Then I will receive no core error
		And the settings will be
			| UseCurrency |
			| false       |
