Feature: Language

Background:
	Given the dictionary is initialized
		And I have these languages
			| Language |
			| en-US    |
			| pt-BR    |

Scenario: 01. Get layouts of e-mails
	Given I have the e-mail types
		And I have the themes
	When I try get the layout
	Then I will receive no language error

Scenario: 02. Get translations of Enums
	Given I have these enums
			| Section   | Project       | Path       | Enum              |
			| Moves     | Entities      | Enums      | MoveNature        |
			| Schedules | Entities      | Enums      | ScheduleFrequency |
			| Users     | Entities      | Enums      | RemovalReason     |
			| Tokens    | Entities      | Enums      | SecurityAction    |
			| General   | Entities      | Enums      | OperationType     |
			| Tips      | Entities      | Enums      | TipBrowser        |
			| Configs   | Generic       |            | Theme             |
			| Configs   | Generic       |            | ThemeBrightness   |
			| Configs   | Generic       |            | ThemeColor        |
			| General   | BusinessLogic | Exceptions | Error             |
			| Email     | Email         |            | EmailStatus       |
	When I try get the translate
	Then I will receive no language error

Scenario: 03. Keys should be in all languages (site)
	Then all keys should be available in all languages at site dictionary

Scenario: 04. Keys should be in all languages (e-mail)
	Then all keys should be available in all languages at e-mail dictionary

Scenario: 05. Keys should not be repeated (site)
	Then no keys should be repeated at site dictionary

Scenario: 06. Keys should not be repeated (e-mail)
	Then no keys should be repeated at e-mail dictionary

Scenario: 07. Count translations
	When count the occurrences of Wizard_Test in site/wizard
	Then it will return 3
